using System.Collections.Generic;
using R7.Enrollment.Models;

namespace R7.Enrollment.Components
{
    public class ConsolidatedEntrantContractComparer: IComparer<ConsolidatedEntrant>
    {
        public int Compare (ConsolidatedEntrant x, ConsolidatedEntrant y)
        {
            if (x.AcceptedEntrant != y.AcceptedEntrant) {
                return x.AcceptedEntrant.CompareTo (y.AcceptedEntrant);
            }

            return x.FinalMark.CompareTo (y.FinalMark);
        }
    }
}
