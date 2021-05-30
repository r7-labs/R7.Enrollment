using System;
using System.Collections.Generic;

namespace R7.Enrollment.Models
{
    public class Competition
    {
        // mirroring parent prop here
        public DateTime CurrentDateTime { get; set; }
        
        public string EduProgramForm { get; set; }
        
        public string EduLevel { get; set; }
        
        public string EduProgramSubject { get; set; }
        
        public string EduProgramTitle { get; set; }
        
        public string OrgTitle { get; set; }
        
        public string OrgUnitTitle { get; set; }
        
        public string CompetitionType { get; set; }
        
        public string CompensationType { get; set; }
        
        public string EduLevelRequirement { get; set; }
        
        public int Plan { get; set; }
        
        public int FirstStepPlan { get; set; }

        public IList<EntranceDiscipline> EntranceDisciplines { get; set; } = new List<EntranceDiscipline> ();
        
        public IList<Entrant> Entrants { get; set; } = new List<Entrant> ();
    }
}