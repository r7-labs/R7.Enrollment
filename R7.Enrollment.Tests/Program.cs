using System.IO;
using System.Linq;
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
            
            RenderToFile (db, "output-print.html", new TandemRatingRendererSettings ());
            RenderToFile (db, "output-web.html", new TandemRatingRendererSettings {
                UseBasicCompetitionHeader = true
            });
        }

        static void RenderToFile (TandemRatingsDb db, string path, TandemRatingRendererSettings settings)
        {
            var htmlRenderer = new TandemEntrantRatingHtmlRenderer (settings);
            var sb = new StringBuilder ();
            var html = XmlWriter.Create (sb);
            htmlRenderer.RenderStandalone (db.EntrantRatingEnvironment, html);
            html.Close ();
            File.WriteAllText (path, sb.ToString ());
        }
    }
}
