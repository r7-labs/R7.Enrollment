using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.FileSystem;
using R7.Enrollment.Data;

namespace R7.Enrollment.Dnn.Data
{
    public class TandemRatingsDbManager: TandemRatingsDbManagerBase
    {
        private static readonly Lazy<ConcurrentDictionary<int, TandemRatingsDbManager>> _instances =
            new Lazy<ConcurrentDictionary<int, TandemRatingsDbManager>> ();

        public static TandemRatingsDbManager GetInstance (int moduleId) =>
            _instances.Value.GetOrAdd (moduleId,
                mid => new TandemRatingsDbManager (mid)
        );

        private TandemRatingsDbManager (int moduleId)
        {
            ModuleId = moduleId;
        }

        protected override IEnumerable<System.IO.FileInfo> GetModuleSourceFiles (int moduleId)
        {
            var moduleContext = ModuleController.Instance.GetModule (moduleId, -1, false);
            var folderPath = GetDataFolderModuleSetting (moduleContext);
            var dbFolder = FolderManager.Instance.GetFolder (moduleContext.PortalID, folderPath);
            if (dbFolder == null) {
                return Enumerable.Empty<System.IO.FileInfo> ();
            }

            return FolderManager.Instance.GetFiles (dbFolder)
                .Where (f => Regex.IsMatch (f.FileName, "^enr_rating_[^.]*\\.xml$"))
                .Select (f => new System.IO.FileInfo (f.PhysicalPath));
        }

        protected override void LogException (Exception ex)
        {
            Exceptions.LogException (ex);
        }

        string GetDataFolderModuleSetting (ModuleInfo moduleContext)
        {
            var moduleSettings = new RatingsModuleSettingsRepository ().GetSettings (moduleContext);
            if (string.IsNullOrEmpty (moduleSettings.DataFolderPath)) {
                return "enrollment";
            }
            return moduleSettings.DataFolderPath;
        }
    }
}
