using System;
using System.Collections.Generic;

namespace R7.Enrollment.Models
{
    public class Competition: ICompetition
    {
        #region ICompetition implementation

        public DateTime CurrentDateTime { get; set; }

        public string EduLevel { get; set; }

        public string OrgTitle { get; set; }

        public string OrgUnitTitle { get; set; }

        public bool CompensationTypeBudget { get; set; }

        public string EduLevelRequirement { get; set; }

        public string EduLevelRequirementGenetiveTitle { get; set; }

        public EduProgram EduProgram { get; set; } = new EduProgram ();

        public IList<EntranceDiscipline> EntranceDisciplines { get; set; } = new List<EntranceDiscipline> ();

        #endregion

        public int Plan { get; set; }

        public string CompetitionType { get; set; }

        public int CompetitionTypeCode { get; set; }

        public string CompensationType { get; set; }

        public IList<Entrant> Entrants { get; set; } = new List<Entrant> ();
    }
}
