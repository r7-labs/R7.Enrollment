using R7.Enrollment.Models;

namespace R7.Enrollment.Dnn.ViewModels
{
    public class CampaignViewModel
    {
        public string CampaignTitle { get; set; }

        public string CampaignToken { get; set; }

        public string CurrentDateTime { get; set; }

        public CampaignViewModel (EntrantRatingEnvironment env)
        {
            CampaignTitle = env.CampaignTitle;
            CampaignToken = env.GetCampaignToken ();
            CurrentDateTime = $"{env.CurrentDateTime.ToShortDateString ()} {env.CurrentDateTime.ToShortTimeString ()}";
        }
    }
}
