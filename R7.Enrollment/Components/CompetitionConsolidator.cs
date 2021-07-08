using System.Collections.Generic;
using R7.Enrollment.Models;

namespace R7.Enrollment.Components
{
    /// <remarks>
    /// Бессмысленная деятельность по слепой реимплементации бизнес-логики Тандем.Университет
    /// в приложении, предназначенном для отображения заранее подготовленных данных
    /// </remarks>
    public class CompetitionConsolidator
    {
        public IList<ConsolidatedCompetition> Consolidate (IList<Competition> competitions)
        {
            var ccs = new List<ConsolidatedCompetition> ();
            foreach (var c in competitions) {
                var cc = FindRoot (c, ccs);
                if (cc == null) {
                    ccs.Add (new ConsolidatedCompetition (c));
                    continue;
                }
                cc.ConsolidateWith (c);
            }

            return ccs;
        }

        ConsolidatedCompetition FindRoot (Competition c, IList<ConsolidatedCompetition> ccs)
        {
            foreach (var cc in ccs) {
                if (c.EduProgram.Form == cc.EduProgram.Form
                    && c.EduLevel == cc.EduLevel
                    && c.EduProgram.Subject == cc.EduProgram.Subject
                    && c.EduProgram.Specialization == cc.EduProgram.Specialization
                    && c.OrgUnitTitle == cc.OrgUnitTitle // same edu. program on different faculties
                    && c.EduLevelRequirement == cc.EduLevelRequirement // for college
                    && c.CompensationTypeBudget == cc.CompensationTypeBudget) {
                    return cc;
                }
            }
            return null;
        }
    }
}
