using System.Collections.Generic;
using System.Linq;
using R7.Enrollment.Models;

namespace R7.Enrollment.Queries
{
    public class CompetitionQuery
    {
        public IEnumerable<Competition> ByPersonalNumber (IEnumerable<Competition> competitions, int entrantId)
        {
            return from competition in competitions
                let entrant = competition.Entrants.FirstOrDefault (entr => entr.PersonalNumber == entrantId.ToString ())
                where entrant != null
                select competition;
        }
    }
}