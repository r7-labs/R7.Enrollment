using System.IO;
using System.Text;
using System.Xml;
using R7.Enrollment.Components;
using R7.Enrollment.Data;
using R7.Enrollment.Renderers;

namespace R7.Enrollment.Tests
{
    public class Program
    {
        static void Main (string[] args)
        {
            var consolidator = new CompetitionConsolidator ();
            var webSettings = new RatingsRendererSettings ();
            var renderer = new RatingsHtmlRenderer (webSettings);
            var consolidatedRenderer = new ConsolidatedRatingsHtmlRenderer (webSettings);

            Directory.CreateDirectory ("output");
            var dataFiles = Directory.GetFiles ("./data", "enr_rating_*.xml");
            foreach (var dataFile in dataFiles) {
                var db = new TandemRatingsDb ();
                db.Load (dataFile, consolidate: true);
                var env = db.EntrantRatingEnvironment;
                RenderToFile (db, $"./output/{FilenameFromCampaignTitle (env.CampaignTitle)}.html", renderer);
                RenderToFile (db, $"./output/{FilenameFromCampaignTitle (env.CampaignTitle)}-consolidated.html", consolidatedRenderer);
            }
        }

        static string FilenameFromCampaignTitle (string campaignTitle)
        {
            return campaignTitle.Replace ("21/22", "").Replace ("/", "_").Trim ().ToLower ();
        }

        static void RenderToFile (TandemRatingsDb db, string path, RatingsHtmlRenderer htmlRenderer)
        {
            var sb = new StringBuilder ();
            var html = XmlWriter.Create (sb);
            htmlRenderer.RenderStandalone (db.EntrantRatingEnvironment, html);
            html.Close ();
            File.WriteAllText (path, sb.ToString ());
        }
    }
}
