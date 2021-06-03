using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using DotNetNuke.Services.FileSystem;
using R7.Enrollment.Data;

namespace R7.Enrollment.Dnn.Data
{
    public class DbEntry
    {
        public int PortalId { get; set; }
        
        public string Key { get; set; }
        
        public string Path { get; set; }
        
        public DateTime LastModified { get; set; }
        
        public TandemEntrantRatingDb Db { get; set; }
    }


    public class TandemDbManager
    {
        private static readonly Lazy<TandemDbManager> _instance = new Lazy<TandemDbManager> ();

        public static TandemDbManager Instance => _instance.Value;

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
                var db = new TandemEntrantRatingDb ();
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

        public TandemEntrantRatingDb GetDb (string campaignTitle, int portalId)
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
            var db = new TandemEntrantRatingDb ();
            db.Load (file.PhysicalPath);
            dbEntry.LastModified = file.LastModificationTime;
            dbEntry.Db = db;
            return db;
        }
    }
}