using System;
using System.Collections.Generic;
using System.Text;

namespace R7.Enrollment.Models
{
    public class EntrantRatingEnvironment
    {
        public DateTime CurrentDateTime { get; set; }

        public string CampaignTitle { get; set; }

        public IList<Competition> Competitions { get; set; } = new List<Competition> ();

        public IList<ConsolidatedCompetition> ConsolidatedCompetitions { get; set; }

        public string GetCampaignToken () => Convert.ToBase64String (Encoding.UTF8.GetBytes (CampaignTitle.GetHashCode ().ToString ()));
    }
}
