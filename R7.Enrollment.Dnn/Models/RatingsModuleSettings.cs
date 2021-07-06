using DotNetNuke.Entities.Modules.Settings;

namespace R7.Enrollment.Dnn
{
    public class RatingsModuleSettings
    {
        [ModuleSetting]
        public string DataFolderPath { get; set; }
    }

    public class RatingsModuleSettingsRepository : SettingsRepository<RatingsModuleSettings>
    {
    }
}
