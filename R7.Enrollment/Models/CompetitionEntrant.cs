using System.Linq;
using System.Xml.Linq;

namespace R7.Enrollment.Models
{
    public class CompetitionEntrant
    {
        public string Name { get; set; }
        
        public int Position { get; set; }
        
        public int FinalMark { get; set; }
        
        public string PersonalNumber { get; set; }

        public static CompetitionEntrant FromXElement (XElement xelem)
        {
            return new CompetitionEntrant {
                PersonalNumber = xelem.Descendants ("entrantPersonalNumber").First ().Value,
                Name = xelem.Attribute ("fio").Value,
                Position = int.Parse (xelem.Attribute ("position").Value),
                FinalMark = int.Parse (xelem.Attribute ("finalMark").Value)
            };
        }
    }
}