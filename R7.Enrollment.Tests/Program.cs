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
            var db = new TandemEntrantRatingDb ();
            db.LoadEntrantRatingEnvironment ("./data/sample.xml");
            db.Dump ();
            
            var htmlRenderer = new TandemEntrantRatingHtmlRenderer ();
            var sb = new StringBuilder ();
            var html = XmlWriter.Create (sb);
            htmlRenderer.RenderStandalone (db.EntrantRatingEnvironments.First (), html);
            html.Close ();
            File.WriteAllText ("output.html", sb.ToString ());
        }
    }
}
