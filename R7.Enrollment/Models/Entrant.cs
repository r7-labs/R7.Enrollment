using System;
using System.Collections.Generic;

namespace R7.Enrollment.Models
{
    public class Entrant
    {
        public string Name { get; set; }

        public string Snils { get; set; }

        public string PersonalNumber { get; set; }

        public int Position { get; set; }

        public int FinalMark { get; set; }

        public int AchievementMark { get; set; }

        public bool OriginalIn { get; set; }

        public bool AcceptedEntrant { get; set; }

        public bool Recommended { get; set; }

        public bool RefusedToBeEnrolled { get; set; }

        public string Status { get; set; }

        public int StatusCode { get; set; }

        public bool IsRanked () =>
            StatusCode != (int) EntrantStatus.TookAwayTheDocs &&
            StatusCode != (int) EntrantStatus.RefusedToEnroll &&
            StatusCode != (int) EntrantStatus.DroppedOut;

        public string PreferenceCategory { get; set; }

        public bool HasPreference () => PreferenceCategory != null;

        public IList<string> MarkStrings { get; set; }

        [Obsolete ("Could be used later on over/with MarkStrings", false)]
        public IList<EntrantMark> Marks { get; set; } = new List<EntrantMark> ();
    }
}
