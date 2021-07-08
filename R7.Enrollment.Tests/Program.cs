using System;
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
            if (dataFiles.Length == 0) {
                Console.WriteLine ("No data files found in the \"data\" folder!");
                return;
            }

            foreach (var dataFile in dataFiles) {
                Console.Write ($"Processing \"{Path.GetFileName (dataFile)}\" file... ");
                var db = new TandemRatingsDb ();
                db.Load (dataFile, consolidate: true);
                var env = db.EntrantRatingEnvironment;
                RenderToFile (db, $"./output/{FilenameFromCampaignTitle (env.CampaignTitle)}.html", renderer);
                RenderToFile (db, $"./output/{FilenameFromCampaignTitle (env.CampaignTitle)}-consolidated.html",
                    consolidatedRenderer);
                Console.WriteLine ("Done!");
            }

            Console.WriteLine ("See results in the \"output\" folder.");
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
