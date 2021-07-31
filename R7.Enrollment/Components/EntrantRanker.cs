using System.Linq;
using R7.Enrollment.Models;

namespace R7.Enrollment.Components
{
    public class EntrantRanker
    {
        public void RankEntrants (Competition competition)
        {
            var entrantComparer = new EntrantComparer (competition.EntranceDisciplines);
            var rank = 1;
            foreach (var entrant in competition.Entrants.OrderByDescending (entr => entr, entrantComparer)) {
                if (entrant.IsRanked ()) {
                    entrant.Rank = rank;
                    rank++;
                }
            }
        }
    }
}
