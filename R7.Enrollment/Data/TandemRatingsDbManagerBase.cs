using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using R7.Enrollment.Models;

namespace R7.Enrollment.Data
{
    public abstract class TandemRatingsDbManagerBase
    {
        private DbSetEntry _dbSet;
        private DbSetEntry DbSet {
            get {
                lock (this) {
                    if (!DbSetIsActual (_dbSet)) {
                        _dbSet = CreateDbSet (ModuleId);
                    }
                    return _dbSet;
                }
            }
        }

        protected int ModuleId;

        protected abstract IEnumerable<FileInfo> GetModuleSourceFiles (int moduleId);

        protected abstract void LogException (Exception ex);

        bool DbSetIsActual (DbSetEntry dbSet)
        {
            if (dbSet == null) {
                return false;
            }

            var srcFiles = GetModuleSourceFiles (ModuleId).ToList ();
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

        DbSetEntry CreateDbSet (int moduleId)
        {
            var dbSet = new DbSetEntry ();
            var srcFiles = GetModuleSourceFiles (moduleId).ToList ();
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

        public TandemRatingsDb GetDb (string campaignToken)
        {
            return DbSet.Databases.FirstOrDefault (dbs =>
                dbs.EntrantRatingEnvironment.CampaignToken == campaignToken);
        }

        public IEnumerable<TandemRatingsDb> GetDbs ()
        {
            return DbSet.Databases;
        }
    }
}
