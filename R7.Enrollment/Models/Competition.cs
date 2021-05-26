using System.Collections.Generic;

namespace R7.Enrollment.Models
{
    public class Competition
    {
        public string EduProgramForm { get; set; }
        
        public string EduLevel { get; set; }
        
        public string EduProgramSubject { get; set; }
        
        public string EduProgramTitle { get; set; }
        
        public string OrgUnitTitle { get; set; }
        
        public string CompetitionType { get; set; }
        
        public string CompensationType { get; set; }

        public IList<EntranceDiscipline> EntranceDisciplines { get; set; } = new List<EntranceDiscipline> ();
        
        public IList<CompetitionEntrant> Entrants { get; set; } = new List<CompetitionEntrant> ();
    }
}