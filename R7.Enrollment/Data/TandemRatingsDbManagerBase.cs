using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using R7.Enrollment.Models;

namespace R7.Enrollment.Data
{
    public abstract class TandemRatingsDbManagerBase
    {
        private IList<DbSetEntry> DbSets = new List<DbSetEntry> ();

        protected abstract IEnumerable<FileInfo> GetSourceFiles (int portalId);
        
        DbSetEntry GetDbSet (int portalId) => DbSets.FirstOrDefault (dbs => dbs.PortalId == portalId);
        
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