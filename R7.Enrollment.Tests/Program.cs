using System.IO;
using System.Text;
using System.Xml;
using R7.Enrollment.Data;
using R7.Enrollment.Renderers;

namespace R7.Enrollment.Tests
{
    public class Program
    {
        static void Main (string[] args)
        {
            var db = new TandemRatingsDb ();
            db.Load ("./data/enr_rating_1696453372720271613.xml");
            //db.Load ("./data/enr_rating_1697112374642823421.xml");

            RenderToFile (db, "output-print.html", new TandemRatingsRendererSettings ());
            RenderToFile (db, "output-web.html", new TandemRatingsRendererSettings {
                UseBasicCompetitionHeader = true
            });
        }

        static void RenderToFile (TandemRatingsDb db, string path, TandemRatingsRendererSettings settings)
        {
            var htmlRenderer = new TandemRatingsHtmlRenderer (settings);
            var sb = new StringBuilder ();
            var html = XmlWriter.Create (sb);
            htmlRenderer.RenderStandalone (db.EntrantRatingEnvironment, html);
            html.Close ();
            File.WriteAllText (path, sb.ToString ());
        }
    }
}
