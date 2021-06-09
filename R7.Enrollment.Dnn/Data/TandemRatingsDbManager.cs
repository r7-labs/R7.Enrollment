using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetNuke.Services.FileSystem;
using R7.Enrollment.Data;
using R7.Enrollment.Models;
using SystemFileInfo = System.IO.FileInfo;

namespace R7.Enrollment.Dnn.Data
{
    public class TandemRatingsDbManager
    {
        private const string FolderPath = "enrollment";

        private static readonly Lazy<TandemRatingsDbManager> _instance = new Lazy<TandemRatingsDbManager> ();

        public static TandemRatingsDbManager Instance => _instance.Value;

        private IList<DbSetEntry> DbSets = new List<DbSetEntry> ();

        bool DbSetIsActual (int portalId)
        {
            var dbSet = DbSets.FirstOrDefault (dbs => dbs.PortalId == portalId);
            if (dbSet == null) {
                return false;
            }

            var srcFiles = GetSourceFiles (portalId).ToList ();
            if (dbSet.SourceFiles.Count != srcFiles.Count) {
                return false;
            }

            foreach (var srcFile in srcFiles) {
                var dbSrcFile = dbSet.SourceFiles.FirstOrDefault (
                    sf => sf.Name.Equals (srcFile.Name, StringComparison.CurrentCultureIgnoreCase)
                          && sf.Length == srcFile.Length
                          && sf.LastWriteTimeUtc == srcFile.LastWriteTimeUtc);
                if (dbSrcFile == null) {
                    return false;
                }
            }

            return true;
        }

        IEnumerable<SystemFileInfo> GetSourceFiles (int portalId)
        {
            var dbFolder = FolderManager.Instance.GetFolder (portalId, FolderPath);
            if (dbFolder == null) {
                return Enumerable.Empty<SystemFileInfo> ();
            }

            return FolderManager.Instance.GetFiles (dbFolder)
                .Where (f => Regex.IsMatch (f.FileName, "^enr_[^.]*\\.xml$"))
                .Select (f => new SystemFileInfo (f.PhysicalPath));
        }

        DbSetEntry GetDbSet (int portalId) => DbSets.FirstOrDefault (dbs => dbs.PortalId == portalId);
        
        void EnsureDbSetIsActual (int portalId)
        {
            if (!DbSetIsActual (portalId)) {
                ReloadDbSet (portalId);
            }
        }

        void ReloadDbSet (int portalId)
        {
            var dbSet = GetDbSet (portalId);    
            if (dbSet != null) {
                DbSets.Remove (dbSet);
            }
            
            dbSet = new DbSetEntry {
                PortalId = portalId
            };
            
            var srcFiles = GetSourceFiles (portalId).ToList ();
            dbSet.SourceFiles = srcFiles.Select (sf =>
                new DbSourceFile {
                    Name = sf.Name,
                    Length = sf.Length,
                    LastWriteTimeUtc = sf.LastWriteTimeUtc
                }
            ).ToList ();
                
            foreach (var srcFile in srcFiles) {
                var db = new TandemRatingsDb ();
                db.Load (srcFile.FullName);
                dbSet.Databases.Add (db);
            }
            
            DbSets.Add (dbSet);
        }
        
        public IEnumerable<EntrantRatingEnvironment> GetCampaigns (int portalId)
        {
            lock (this) {
                EnsureDbSetIsActual (portalId);
                var dbSet = DbSets.FirstOrDefault (dbs => dbs.PortalId == portalId);
                if (dbSet == null) {
                    return Enumerable.Empty<EntrantRatingEnvironment> ();
                }
                return dbSet.Databases.Select (dbs => dbs.EntrantRatingEnvironment);
            }
        }

        public TandemRatingsDb GetDb (string campaignTitle, int portalId)
        {
            lock (this) {
                EnsureDbSetIsActual (portalId);
                var dbSet = DbSets.FirstOrDefault (dbs => dbs.PortalId == portalId);
                if (dbSet == null) {
                    return null;
                }
                return dbSet.Databases.FirstOrDefault (dbs =>
                    dbs.EntrantRatingEnvironment.CampaignTitle == campaignTitle);
            }
        }
    }
}