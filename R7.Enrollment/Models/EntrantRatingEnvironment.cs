using System;
using System.Collections.Generic;

namespace R7.Enrollment.Models
{
    public class EntrantRatingEnvironment
    {
        public DateTime CurrentDateTime { get; set; }
        
        public string EnrollmentCampaignTitle { get; set; }
        
        public IList<Competition> Competitions { get; set; } = new List<Competition> ();
    }
}