using System;
using System.Collections.Generic;
using R7.Enrollment.Models;

namespace R7.Enrollment.Components
{
    public class EntrantComparer: IComparer<Entrant>
    {
        private IList<EntranceDiscipline> _entranceDisciplines;

        public EntrantComparer (IList<EntranceDiscipline> entranceDisciplines)
        {
            _entranceDisciplines = entranceDisciplines;
        }

        public int Compare (Entrant x, Entrant y)
        {
            if (x.FinalMark != y.FinalMark) {
                return x.FinalMark.CompareTo (y.FinalMark);
            }

            if (x.HasPreference () != y.HasPreference ()) {
                return x.HasPreference ().CompareTo (y.HasPreference ());
            }

            var xMarks = GetMarkIntegers (x.MarkStrings);
            var yMarks = GetMarkIntegers (y.MarkStrings);

            for (int i = 0; i < Math.Min (xMarks.Count, yMarks.Count); i++) {
                if (xMarks[i] != yMarks[i]) {
                    return xMarks[i].CompareTo (yMarks[i]);
                }
            }

            return 0;
        }

        private IList<int> GetMarkIntegers (IList<string> markStrings)
        {
            var markIntegers = new List<int> ();
            foreach (var markString in markStrings) {
                if (int.TryParse (markString, out int mark)) {
                    markIntegers.Add (mark);
                }
                else {
                    markIntegers.Add (0);
                }
            }

            return markIntegers;
        }
    }
}
