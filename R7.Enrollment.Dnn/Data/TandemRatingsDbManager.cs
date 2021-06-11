using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.FileSystem;
using R7.Enrollment.Data;

namespace R7.Enrollment.Dnn.Data
{
    public class TandemRatingsDbManager: TandemRatingsDbManagerBase
    {
        private static readonly Lazy<TandemRatingsDbManager> _instance = new Lazy<TandemRatingsDbManager> ();

        public static TandemRatingsDbManager Instance => _instance.Value;

        protected override IEnumerable<System.IO.FileInfo> GetSourceFiles (int portalId)
        {
            var folderPath = GetDataFolderSetting (portalId);
            var dbFolder = FolderManager.Instance.GetFolder (portalId, folderPath);
            if (dbFolder == null) {
                return Enumerable.Empty<System.IO.FileInfo> ();
            }

            return FolderManager.Instance.GetFiles (dbFolder)
                .Where (f => Regex.IsMatch (f.FileName, "^enr_rating_[^.]*\\.xml$"))
                .Select (f => new System.IO.FileInfo (f.PhysicalPath));
        }

        string GetDataFolderSetting (int portalId)
        {
            var portalSettings = PortalController.Instance.GetPortalSettings (portalId);
            portalSettings.TryGetValue ("R7_Enrollment_Ratings__DataFolder", out string dataFolderPath);
            if (string.IsNullOrEmpty (dataFolderPath)) {
                return "enrollment";
            }
            return dataFolderPath;
        }
    }
}