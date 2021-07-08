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
            var printTestSettings = new RatingsRendererSettings () {
                UseBasicCompetitionHeader = false,
                Depersonalize = false
            };
            var webSettings = new RatingsRendererSettings ();

            Directory.CreateDirectory ("output");

            var consolidator = new CompetitionConsolidator ();
            var dataFiles = Directory.GetFiles ("./data", "enr_rating_*.xml");
            foreach (var dataFile in dataFiles) {
                var db = new TandemRatingsDb ();
                db.Load (dataFile, consolidate: true);
                var env = db.EntrantRatingEnvironment;
                RenderToFile (db, $"./output/{FilenameFromCampaignTitle (env.CampaignTitle)}-print.html", printTestSettings);
                RenderToFile (db, $"./output/{FilenameFromCampaignTitle (env.CampaignTitle)}-web.html", webSettings);

                Console.WriteLine ($"raw: {env.Competitions.Count}, consolidated: {env.ConsolidatedCompetitions?.Count ?? -1}");
            }
        }

        static string FilenameFromCampaignTitle (string campaignTitle)
        {
            return campaignTitle.Replace ("21/22", "").Replace ("/", "_").Trim ().ToLower ();
        }

        static void RenderToFile (TandemRatingsDb db, string path, RatingsRendererSettings settings)
        {
            var htmlRenderer = new RatingsHtmlRenderer (settings);
            var sb = new StringBuilder ();
            var html = XmlWriter.Create (sb);
            htmlRenderer.RenderStandalone (db.EntrantRatingEnvironment, html);
            html.Close ();
            File.WriteAllText (path, sb.ToString ());
        }
    }
}
