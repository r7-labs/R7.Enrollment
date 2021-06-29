using System;

namespace R7.Enrollment.Models
{
    public class EduProgram
    {
        public string Subject { get; set; }

        public string Specialization { get; set; }

        public string Title { get; set; }

        [Obsolete ("Related XML attr 'fullTitleWithoutSubjectIndex' doesn't really contains full edu. program title", false)]
        public string FullTitle { get; set; }

        public string Form { get; set; }

        public string Duration { get; set; }

        public string ConditionsWithForm { get; set; }

        public string TitleAndConditionsShortWithForm { get; set; }
    }
}
