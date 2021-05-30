using System;
using System.Linq;
using System.Xml.Linq;
using R7.Enrollment.Models;

namespace R7.Enrollment.Data
{
    public class TandemXmlModelFactory
    {
        public static Competition CreateCompetition (XElement xelem)
        {
            return new Competition {
                EduProgramForm = xelem.Attribute ("eduProgramForm")?.Value,
                EduLevel = xelem.Attribute ("eduLevel")?.Value,
                EduProgramSubject = xelem.Attribute ("eduProgramSubject")?.Value,
                EduProgramTitle = xelem.Attribute ("programSetPrintTitle")?.Value,
                OrgTitle = xelem.Attribute ("enrOrgUnit")?.Value,
                OrgUnitTitle = xelem.Attribute ("formativeOrgUnitTitle")?.Value,
                CompetitionType = xelem.Attribute ("competitionType")?.Value,
                CompensationType = xelem.Attribute ("compensationTypeShortTitle")?.Value,
                Plan = TryParseInt (xelem.Attribute ("plan")?.Value) ?? 0
            };
        }
        
        public static CompetitionEntrant CreateCompetitionEntrant (XElement xelem)
        {
            return new CompetitionEntrant {
                PersonalNumber = xelem.Descendants ("entrantPersonalNumber").FirstOrDefault ()?.Value,
                Name = xelem.Attribute ("fio")?.Value,
                Position = TryParseInt (xelem.Attribute ("position")?.Value) ?? 0,
                FinalMark = TryParseInt (xelem.Attribute ("finalMark")?.Value) ?? 0,
                AchievementMark = TryParseInt (xelem.Attribute ("achievementMark")?.Value) ?? 0,
                OriginalIn = bool.Parse (xelem.Attribute ("originalIn").Value),
                AcceptedEntrant = bool.Parse (xelem.Attribute ("acceptedEntrant").Value)
            };
        }
        
        public static EntranceDiscipline CreateEntranceDiscipline (XElement xelem)
        {
            return new EntranceDiscipline {
                Title = xelem.Attribute ("title")?.Value,
                ShortTitle = xelem.Attribute ("shortTitle")?.Value
            };
        }
        
        public static EntrantMark CreateEntrantMark (XElement xelem, Competition competition)
        {
            var markTitle = xelem.Attribute ("markTitle")?.Value;
            return new EntrantMark {
                Mark = (TryParseInt (xelem.Attribute ("mark")?.Value) ?? 0) / 1000,
                EntranceDiscipline = competition.EntranceDisciplines
                    .FirstOrDefault (ed => ed.Title.IndexOf (markTitle, StringComparison.CurrentCultureIgnoreCase) >= 0)
            };
        }
        
        static int? TryParseInt (string value)
        {
            return int.TryParse (value, out int result) ? (int?) result : null;
        }
    }
}