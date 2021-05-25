using System.Xml.Linq;

namespace R7.Enrollment.Models
{
    public class Competition
    {
        public string EduProgramForm { get; set; }
        
        public string EduLevel { get; set; }
        
        public string EduProgramTitle { get; set; }
        
        public string OrgUnitTitle { get; set; }
        
        public string CompetitionType { get; set; }
        
        public string CompensationType { get; set; }

        public static Competition FromXElement (XElement xelem)
        {
            return new Competition {
                EduProgramForm = xelem.Attribute ("eduProgramForm").Value,
                EduLevel = xelem.Attribute ("eduLevel").Value,
                EduProgramTitle = xelem.Attribute ("programSetPrintTitle").Value,
                OrgUnitTitle = xelem.Attribute ("formativeOrgUnitTitle").Value,
                CompetitionType = xelem.Attribute ("competitionType").Value,
                CompensationType = xelem.Attribute ("compensationTypeShortTitle").Value
            };
        }
    }
}