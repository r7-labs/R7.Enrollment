using System.Collections.Generic;
using System.Net.Mime;

namespace R7.Enrollment.Models
{
    public class Entrant
    {
        public string Name { get; set; }
        
        public string PersonalNumber { get; set; }
        
        public int Position { get; set; }
        
        public int FinalMark { get; set; }
        
        public int AchievementMark { get; set; }
        
        public bool OriginalIn { get; set; }
        
        public bool AcceptedEntrant { get; set; }
        
        public bool Recommended { get; set; }
        
        public bool RefusedToBeEnrolled { get; set; }

        public IList<EntrantMark> Marks { get; set; } = new List<EntrantMark> ();
    }
}