using R7.Enrollment.Data;

namespace R7.Enrollment.Tests
{
    public class Program
    {
        static void Main (string[] args)
        {
            var db = new TandemEntrantRatingDb ("./data/sample.xml");
            db.Dump ();
        }
    }
}
