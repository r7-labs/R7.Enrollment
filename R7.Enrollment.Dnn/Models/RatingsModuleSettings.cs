using DotNetNuke.Entities.Modules.Settings;

namespace R7.Enrollment.Dnn
{
    public class RatingsModuleSettings
    {
        [ModuleSetting]
        public string DataFolderPath { get; set; }

        [ModuleSetting]
        public bool IncludeEmptyCompetitions { get; set; } = true;
    }

    public class RatingsModuleSettingsRepository : SettingsRepository<RatingsModuleSettings>
    {
    }
}
