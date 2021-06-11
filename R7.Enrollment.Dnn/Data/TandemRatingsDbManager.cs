using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetNuke.Services.FileSystem;
using R7.Enrollment.Data;

namespace R7.Enrollment.Dnn.Data
{
    public class TandemRatingsDbManager: TandemRatingsDbManagerBase
    {
        private const string FolderPath = "enrollment";

        private static readonly Lazy<TandemRatingsDbManager> _instance = new Lazy<TandemRatingsDbManager> ();

        public static TandemRatingsDbManager Instance => _instance.Value;

        protected override IEnumerable<System.IO.FileInfo> GetSourceFiles (int portalId)
        {
            var dbFolder = FolderManager.Instance.GetFolder (portalId, FolderPath);
            if (dbFolder == null) {
                return Enumerable.Empty<System.IO.FileInfo> ();
            }

            return FolderManager.Instance.GetFiles (dbFolder)
                .Where (f => Regex.IsMatch (f.FileName, "^enr_rating_[^.]*\\.xml$"))
                .Select (f => new System.IO.FileInfo (f.PhysicalPath));
        }
    }
}