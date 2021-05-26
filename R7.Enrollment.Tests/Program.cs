using System;
using System.Text;
using System.Xml;
using R7.Enrollment.Data;
using R7.Enrollment.Views;

namespace R7.Enrollment.Tests
{
    public class Program
    {
        static void Main (string[] args)
        {
            var db = new TandemEntrantRatingDb ("./data/sample.xml");
            db.Dump ();
            
            var htmlRenderer = new TandemEntrantRatingHtmlRenderer (db);
            var sb = new StringBuilder ();
            var html = XmlWriter.Create (sb);
            htmlRenderer.Render (html);
            html.Close ();
            Console.WriteLine (sb.ToString ());
        }
    }
}
