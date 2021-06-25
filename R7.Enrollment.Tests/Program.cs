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
            var printTestSettings = new TandemRatingsRendererSettings () {
                UseBasicCompetitionHeader = false,
                Depersonalize = false
            };
            var webSettings = new TandemRatingsRendererSettings ();

            Directory.CreateDirectory ("output");

            var dataFiles = Directory.GetFiles ("./data", "enr_rating_*.xml");
            foreach (var dataFile in dataFiles) {
                var db = new TandemRatingsDb ();
                db.Load (dataFile);
                RenderToFile (db, $"./output/{FilenameFromCampaignTitle (db.EntrantRatingEnvironment.CampaignTitle)}-print.html", printTestSettings);
                RenderToFile (db, $"./output/{FilenameFromCampaignTitle (db.EntrantRatingEnvironment.CampaignTitle)}-web.html", webSettings);
            }
        }

        static string FilenameFromCampaignTitle (string campaignTitle)
        {
            return campaignTitle.Replace ("21/22", "").Replace ("/", "_").Trim ().ToLower ();
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
