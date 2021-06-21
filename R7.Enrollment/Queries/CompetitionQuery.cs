using System.Collections.Generic;
using System.Linq;
using R7.Enrollment.Models;

namespace R7.Enrollment.Queries
{
    public class CompetitionQuery
    {
        public IEnumerable<Competition> ByPersonalNumber (IEnumerable<Competition> competitions, string personalNumber)
        {
            return from competition in competitions
                let entrant = competition.Entrants.FirstOrDefault (entr => entr.PersonalNumber == personalNumber)
                where entrant != null
                select competition;
        }

        public IEnumerable<Competition> BySnilsOrPersonalNumber (IEnumerable<Competition> competitions, string snils, string personalNumber)
        {
            return from competition in competitions
                let entrant = competition.Entrants.FirstOrDefault (entr => SnilsNotNullAndEquals (entr.Snils, snils) || entr.PersonalNumber == personalNumber)
                where entrant != null
                select competition;
        }

        private bool SnilsNotNullAndEquals (string snils1, string snils2)
        {
            if (string.IsNullOrEmpty (snils1) || string.IsNullOrEmpty (snils2)) {
                return false;
            }

            return snils1 == snils2;
        }
    }
}
