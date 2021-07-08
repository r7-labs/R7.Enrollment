using System.Collections.Generic;
using R7.Enrollment.Models;

namespace R7.Enrollment.Components
{
    public class ConsolidatedEntrantBudgetComparer : IComparer<ConsolidatedEntrant>
    {
        public int Compare (ConsolidatedEntrant x, ConsolidatedEntrant y)
        {
            var xctw = CompetitionTypeWeight ((CompetitionType) x.CompetitionTypeCode);
            var yctw = CompetitionTypeWeight ((CompetitionType) y.CompetitionTypeCode);

            if (xctw != yctw) {
                return xctw.CompareTo (yctw);
            }

            if (x.AcceptedEntrant != y.AcceptedEntrant) {
                return x.AcceptedEntrant.CompareTo (y.AcceptedEntrant);
            }

            return x.FinalMark.CompareTo (y.FinalMark);
        }

        int CompetitionTypeWeight (CompetitionType competitionType)
        {
            switch (competitionType) {
                case CompetitionType.SpecialRights: return 0;
                case CompetitionType.Target: return 1;
                case CompetitionType.NoExamCommon: return 2;
                case CompetitionType.Common: return 3;
                case CompetitionType.ByContract: return 4;
                case CompetitionType.NoExamByContract: return 4;
            }

            return int.MaxValue;
        }
    }
}
