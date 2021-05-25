using System.Linq;
using System.Xml.Linq;
using R7.Enrollment.Models;

namespace R7.Enrollment.Data
{
    public class ModelFactory
    {
        public static CompetitionEntrant CreateCompetitionEntrant (XElement xelem)
        {
            return new CompetitionEntrant {
                PersonalNumber = xelem.Descendants ("entrantPersonalNumber").FirstOrDefault ()?.Value,
                Name = xelem.Attribute ("fio")?.Value,
                Position = int.Parse (xelem.Attribute ("position").Value),
                FinalMark = int.Parse (xelem.Attribute ("finalMark").Value)
            };
        }
        
        public static Competition CreateCompetition (XElement xelem)
        {
            return new Competition {
                EduProgramForm = xelem.Attribute ("eduProgramForm")?.Value,
                EduLevel = xelem.Attribute ("eduLevel")?.Value,
                EduProgramTitle = xelem.Attribute ("programSetPrintTitle")?.Value,
                OrgUnitTitle = xelem.Attribute ("formativeOrgUnitTitle")?.Value,
                CompetitionType = xelem.Attribute ("competitionType")?.Value,
                CompensationType = xelem.Attribute ("compensationTypeShortTitle")?.Value
            };
        }
    }
}