using System;
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
            Directory.CreateDirectory ("output");

            var dataFiles = Directory.GetFiles ("./data", "enr_rating_*.xml");
            if (dataFiles.Length == 0) {
                Console.WriteLine ("No data files found in the \"data\" folder!");
                return;
            }

            foreach (var dataFile in dataFiles) {
                Console.Write ($"Processing \"{Path.GetFileName (dataFile)}\" file... ");
                var db = new TandemRatingsDb ();
                db.Load (dataFile);
                RenderToFile (db, $"./output/{FilenameFromCampaignTitle (db.EntrantRatingEnvironment.CampaignTitle)}-with-names.html",
                    new RatingsRendererSettings {
                        Depersonalize = false
                });
                RenderToFile (db, $"./output/{FilenameFromCampaignTitle (db.EntrantRatingEnvironment.CampaignTitle)}.html",
                    new RatingsRendererSettings ());
                Console.WriteLine ("Done!");
            }

            Console.WriteLine ("See results in the \"output\" folder.");
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
