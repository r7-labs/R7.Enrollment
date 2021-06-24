using System;
using System.Collections.Generic;
using System.Text;

namespace R7.Enrollment.Models
{
    public class EntrantRatingEnvironment
    {
        public DateTime CurrentDateTime { get; set; }

        public string CampaignTitle { get; set; }

        public string CampaignToken => Convert.ToBase64String (Encoding.UTF8.GetBytes (CampaignTitle.GetHashCode ().ToString ()));

        public IList<Competition> Competitions { get; set; } = new List<Competition> ();
    }
}
