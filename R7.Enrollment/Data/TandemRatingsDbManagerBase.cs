using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using R7.Enrollment.Models;

namespace R7.Enrollment.Data
{
    public abstract class TandemRatingsDbManagerBase
    {
        private readonly ConcurrentDictionary<int, DbSetEntry> DbSets = new ConcurrentDictionary<int, DbSetEntry> ();

        protected abstract IEnumerable<FileInfo> GetSourceFiles (int portalId);

        protected abstract void LogException (Exception ex);

        bool DbSetIsActual (DbSetEntry dbSet)
        {
            var srcFiles = GetSourceFiles (dbSet.PortalId).ToList ();
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

        DbSetEntry CreateOrUpdateDbSet (int portalId, DbSetEntry dbSet = null)
        {
            if (dbSet == null) {
                dbSet = new DbSetEntry {
                    PortalId = portalId
                };
            }

            var srcFiles = GetSourceFiles (portalId).ToList ();
            dbSet.SourceFiles = srcFiles.Select (sf =>
                new DbSourceFile {
                    Name = sf.Name,
                    Length = sf.Length,
                    LastWriteTimeUtc = sf.LastWriteTimeUtc
                }
            ).ToList ();

            dbSet.Databases = new List<TandemRatingsDb> ();
            foreach (var srcFile in srcFiles) {
                try {
                    var db = new TandemRatingsDb ();
                    db.Load (srcFile.FullName);
                    dbSet.Databases.Add (db);
                }
                catch (Exception ex) {
                    LogException (ex);
                }
            }

            return dbSet;
        }

        DbSetEntry SafeGetActualDbSet (int portalId)
        {
            lock (this) {
                var dbSet = DbSets.GetOrAdd (portalId, portalId2 => CreateOrUpdateDbSet (portalId2));
                if (!DbSetIsActual (dbSet)) {
                    DbSets.AddOrUpdate (portalId, CreateOrUpdateDbSet (portalId), (portalId2, dbSet2) => CreateOrUpdateDbSet (portalId2, dbSet2));
                }
                return dbSet;
            }
        }

        public IEnumerable<EntrantRatingEnvironment> GetCampaigns (int portalId)
        {
            var dbSet = SafeGetActualDbSet (portalId);
            lock (dbSet) {
                return dbSet.Databases.Select (dbs => dbs.EntrantRatingEnvironment);
            }
        }

        public TandemRatingsDb GetDb (string campaignToken, int portalId)
        {
            var dbSet = SafeGetActualDbSet (portalId);
            lock (dbSet) {
                return dbSet.Databases.FirstOrDefault (dbs =>
                    dbs.EntrantRatingEnvironment.CampaignToken == campaignToken);
            }
        }

        public IEnumerable<TandemRatingsDb> GetDbs (int portalId)
        {
            var dbSet = SafeGetActualDbSet (portalId);
            lock (dbSet) {
                return dbSet.Databases;
            }
        }
    }
}
