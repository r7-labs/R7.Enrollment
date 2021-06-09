using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetNuke.Services.FileSystem;
using R7.Enrollment.Data;
using R7.Enrollment.Models;

namespace R7.Enrollment.Dnn.Data
{
    public class DbEntry
    {
        public int PortalId { get; set; }
        
        public string Key { get; set; }
        
        public string Path { get; set; }
        
        public DateTime LastModified { get; set; }
        
        public TandemRatingsDb Db { get; set; }
    }


    public class TandemRatingsDbManager
    {
        private static readonly Lazy<TandemRatingsDbManager> _instance = new Lazy<TandemRatingsDbManager> ();

        public static TandemRatingsDbManager Instance => _instance.Value;

        private IList<DbEntry> Databases = new List<DbEntry> ();

        void LoadDatabases (int portalId, string dbFolderPath)
        {
            var dbFolder = FolderManager.Instance.GetFolder (portalId, dbFolderPath);
            if (dbFolder == null) {
                return;
            }

            var dbFiles = FolderManager.Instance.GetFiles (dbFolder)
                .Where (f => Regex.IsMatch (f.FileName, "^enr_[^.]*\\.xml$"));

            foreach (var dbFile in dbFiles) {
                var db = new TandemRatingsDb ();
                db.Load (dbFile.PhysicalPath);
                Databases.Add (new DbEntry {
                    PortalId = 0,
                    Path = dbFile.RelativePath,
                    LastModified = dbFile.LastModificationTime,
                    Db = db,
                    Key = db.EntrantRatingEnvironment.CampaignTitle
                });
            }
        }

        public IList<EntrantRatingEnvironment> GetCampaigns (int portalId)
        {
            if (Databases.Count == 0) {
                LoadDatabases (portalId, "enrollment");
            }

            return Databases.Select (dbe => dbe.Db.EntrantRatingEnvironment).ToList ();
        }

        public TandemRatingsDb GetDb (string campaignTitle, int portalId)
        {
            if (Databases.Count == 0) {
                LoadDatabases (portalId, "enrollment");
            }
            
            var dbEntry = Databases.FirstOrDefault (dbe => dbe.Key == campaignTitle);
            if (dbEntry == null) {
                return null;
            }
            
            var file = FileManager.Instance.GetFile (portalId, dbEntry.Path);
            if (file == null) {
                // file was deleted
                Databases.Remove (dbEntry);
                return null;
            }

            if (file.LastModificationTime == dbEntry.LastModified) {
                // file was not modified
                return dbEntry.Db;
            }
            
            // file was modified
            var db = new TandemRatingsDb ();
            db.Load (file.PhysicalPath);
            dbEntry.LastModified = file.LastModificationTime;
            dbEntry.Db = db;
            return db;
        }
    }
}