using System.Xml;
using R7.Enrollment.Data;
using R7.Enrollment.Models;

namespace R7.Enrollment.Views
{
    public class TandemEntrantRatingHtmlRenderer
    {
        protected readonly TandemEntrantRatingDb Db;

        public TandemEntrantRatingHtmlRenderer (TandemEntrantRatingDb db)
        {
            Db = db;
        }

        public void Render (XmlWriter html)
        {
            foreach (var competition in Db.Competitions) {
                RenderCompetition (competition, html);
            }
        }

        public void RenderCompetition (Competition competition, XmlWriter html)
        {
            html.WriteStartElement ("table");
            foreach (var entrant in competition.Entrants) {
                RenderEntrant (entrant, html);
            }
            html.WriteEndElement ();
        }
        
        public void RenderEntrant (CompetitionEntrant entrant, XmlWriter html)
        {
            html.WriteStartElement ("tr");
            html.WriteElementString ("td", entrant.Position.ToString ());
            html.WriteElementString ("td", entrant.Name);
            html.WriteElementString ("td", entrant.FinalMark.ToString ());
            html.WriteElementString ("td", entrant.OriginalIn.ToString ());
            html.WriteElementString ("td", entrant.AcceptedEntrant.ToString ());
            html.WriteElementString ("td", "");
            html.WriteElementString ("td", "");
            html.WriteEndElement ();
        }
    }
}