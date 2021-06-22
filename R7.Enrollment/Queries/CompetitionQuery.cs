using System.Collections.Generic;
using System.Linq;
using R7.Enrollment.Data;
using R7.Enrollment.Models;

namespace R7.Enrollment.Queries
{
    public class CompetitionQuery
    {
        private readonly SnilsComparer _snilsComparer = new SnilsComparer ();

        public IEnumerable<Competition> BySnilsOrPersonalNumber (TandemRatingsDb db, string snils, string personalNumber)
        {
            return from competition in db.EntrantRatingEnvironment.Competitions
                let entrant = competition.Entrants.FirstOrDefault (entr =>
                    _snilsComparer.SnilsNotNullAndEquals (entr.Snils, snils) || entr.PersonalNumber == personalNumber)
                where entrant != null
                select competition;
        }
    }
}
