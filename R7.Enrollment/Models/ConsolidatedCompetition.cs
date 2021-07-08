using System;
using System.Collections.Generic;
using System.Linq;

namespace R7.Enrollment.Models
{
    public class ConsolidatedCompetition: ICompetition
    {
        private readonly Competition _competition;

        #region ICompetition implementation

        public DateTime CurrentDateTime => _competition.CurrentDateTime;

        public string EduLevel => _competition.EduLevel;

        public string OrgTitle => _competition.OrgTitle;

        public string OrgUnitTitle => _competition.OrgUnitTitle;

        public bool CompensationTypeBudget => _competition.CompensationTypeBudget;

        public string EduLevelRequirement => _competition.EduLevelRequirement;

        public string EduLevelRequirementGenetiveTitle => _competition.EduLevelRequirementGenetiveTitle;

        public EduProgram EduProgram => _competition.EduProgram;

        public IList<EntranceDiscipline> EntranceDisciplines => _competition.EntranceDisciplines;

        #endregion

        #region Consolidated props

        public int Plan => CalcPlanValue ();

        public IList<ConsolidatedEntrant> Entrants = new List<ConsolidatedEntrant> ();

        public IList<Competition> Competitions { get; set; } = new List<Competition> ();

        #endregion

        public ConsolidatedCompetition (Competition competition)
        {
            _competition = competition;
            ConsolidateWith (competition);
        }

        public void ConsolidateWith (Competition competition)
        {
            Competitions.Add (competition);
            foreach (var entrant in competition.Entrants) {
                Entrants.Add (new ConsolidatedEntrant (entrant, competition));
            }
        }

        int CalcPlanValue ()
        {
            if (CompensationTypeBudget) {
                return Competitions.Where (c => c.CompetitionTypeCode != (int) CompetitionType.NoExamCommon).Sum (c => c.Plan);
            }
            return Competitions.FirstOrDefault (c => c.CompetitionTypeCode == (int) CompetitionType.ByContract)?.Plan ?? -1;
        }
    }
}
