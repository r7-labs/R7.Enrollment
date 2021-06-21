using System.Text.RegularExpressions;

namespace R7.Enrollment.Models
{
    public class SnilsComparer
    {
        private static readonly Regex _snilsEscapeRegex = new Regex ("[^\\d]", RegexOptions.Compiled);

        public bool SnilsNotNullAndEquals (string snils1, string snils2)
        {
            if (string.IsNullOrEmpty (snils1) || string.IsNullOrEmpty (snils2)) {
                return false;
            }

            var snils1Norm = _snilsEscapeRegex.Replace (snils1, "");
            if (string.IsNullOrEmpty (snils1)) {
                return false;
            }

            var snils2Norm = _snilsEscapeRegex.Replace (snils2, "");
            if (string.IsNullOrEmpty (snils2)) {
                return false;
            }

            return snils1Norm == snils2Norm;
        }
    }
}
