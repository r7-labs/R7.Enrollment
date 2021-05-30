using System.Xml;
using R7.Enrollment.Data;
using R7.Enrollment.Models;

namespace R7.Enrollment.Renderers
{
    public class TandemEntrantRatingHtmlRenderer
    {
        public void RenderStandalone (TandemEntrantRatingDb db, XmlWriter html)
        {
            html.WriteStartDocument ();
            html.WriteStartElement ("html");
            html.WriteStartElement ("head");
            
            html.WriteStartElement ("link");
            html.WriteAttributeString ("href", "https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css");
            html.WriteAttributeString ("rel", "stylesheet");
            html.WriteAttributeString ("integrity", "sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x");
            html.WriteAttributeString ("crossorigin", "anonymous");

            html.WriteEndElement ();
            
            html.WriteStartElement ("body");
            Render (db, html);
            html.WriteEndElement ();
            html.WriteEndElement ();
            html.WriteEndDocument ();
        }

        public void Render (TandemEntrantRatingDb db, XmlWriter html)
        {
            foreach (var competition in db.Competitions) {
                if (competition.Entrants.Count > 0) {
                    RenderCompetition (competition, html);
                }
            }
        }

        public void RenderCompetition (Competition competition, XmlWriter html)
        {
            RenderCompetitionHeader (competition, html);

            html.WriteElementString ("h4", $"{competition.CompetitionType} (заявлений — {competition.Entrants.Count}, число мест — {competition.Plan})");
            
            html.WriteStartElement ("table");
            html.WriteAttributeString ("class", "table table-bordered table-striped table-hover");
            
            RenderEntrantsTableHeader (competition, html);
                
            foreach (var entrant in competition.Entrants) {
                RenderEntrant (entrant, html);
            }
            html.WriteEndElement ();
        }

        void RenderCompetitionHeader (Competition competition, XmlWriter html)
        {
            html.WriteStartElement ("table");
            html.WriteAttributeString ("class", "table");
            
            // 1st row
            html.WriteStartElement ("tr");
            html.WriteElementWithAttributeString ("td", "Рейтинговый (конкурсный) список, список поступающих на", "colspan", "2");
            html.WriteElementWithAttributeString ("td", $"{competition.CurrentDateTime.ToShortDateString ()} {competition.CurrentDateTime.ToShortTimeString ()}", "colspan", "2");
            html.WriteElementString ("td", "");
            html.WriteEndElement ();
            
            // 2nd row
            html.WriteStartElement ("tr");
            html.WriteElementWithAttributeString ("td", competition.OrgTitle, "colspan", "3");
            html.WriteElementString ("td", "");
            html.WriteElementString ("td", competition.OrgUnitTitle);
            html.WriteEndElement ();
            
            // 3rd row
            html.WriteStartElement ("tr");
            html.WriteElementString ("td", "Направление подготовки:");
            html.WriteElementWithAttributeString ("td", competition.EduProgramSubject, "colspan", "2");
            html.WriteElementString ("td", "");
            html.WriteElementString ("td", $"{competition.EduProgramForm} форма обучения, {{4 года}}, {{на базе соо}}, {{АТФ}} {competition.EduLevel}");
            html.WriteEndElement ();
            
            // 4th row
            html.WriteStartElement ("tr");
            
            html.WriteElementString ("td", "Набор ОП:");
            html.WriteStartElement ("td");
            html.WriteAttributeString ("colspan", "4");
            html.WriteString (competition.EduProgramTitle);
            html.WriteEndElement ();
            
            html.WriteEndElement();
            
            // 5th row
            html.WriteStartElement ("tr");
            
            html.WriteStartElement ("td");
            html.WriteAttributeString ("colspan", "3");
            html.WriteString ("Число мест на бюджет (КЦП) — {15}, из них:");
            html.WriteRaw ("<br />");
            html.WriteString ("Принятые сокращения:");
            html.WriteRaw ("<br />");
            html.WriteString ("КЦП – контрольные цифры приёма");
            html.WriteRaw ("<br />");
            html.WriteString ("ИД – индивидуальные достижения");
            html.WriteRaw ("<br />");
            html.WriteString ("ВИ – вступительные испытания:");
            html.WriteRaw ("<br />");
            foreach (var discipline in competition.EntranceDisciplines) {
                html.WriteString($"{discipline.ShortTitle} - {discipline.Title}; ");
            }
            html.WriteEndElement();
            
            html.WriteElementString ("td", "");

            html.WriteStartElement ("td");
            html.WriteString ("Число заявлений:");
            html.WriteRaw ("<br />");
            html.WriteString ($"на бюджет (КЦП) — {{1}}");
            html.WriteEndElement ();
            
            html.WriteEndElement ();
            
            // 6th row
            html.WriteStartElement ("tr");
            html.WriteElementString ("td", "Образовательные программы:");

            html.WriteElementWithAttributeString ("td",
                "{ВЛиИо – Воспроизводство лесов и их использование (ВЛиИо)}, {4 года}, {на базе соо}, {АТФ}", "colspan", "4");
            
            html.WriteEndElement ();
            
            // end table
            html.WriteEndElement ();
        }

        public void RenderEntrantsTableHeader (Competition competition, XmlWriter html)
        {
            html.WriteStartElement ("thead");
            
            html.WriteStartElement ("tr");
            html.WriteElementWithAttributeString ("th", "№", "rowspan", "2");
            html.WriteElementWithAttributeString ("th", "Сумма баллов", "rowspan", "2");
            html.WriteElementWithAttributeString ("th", "Результаты ВИ", "colspan", competition.EntranceDisciplines.Count.ToString ());
            html.WriteElementWithAttributeString ("th", "Сумма баллов за ИД", "rowspan", "2");
            html.WriteElementWithAttributeString ("th", "Сдан оригинал", "rowspan", "2");
            html.WriteElementWithAttributeString ("th", "Согласие на зачисление", "rowspan", "2");
            html.WriteElementWithAttributeString ("th", "Примечание", "rowspan", "2");
            html.WriteElementWithAttributeString ("th", "Информация о зачислении", "rowspan", "2");
            html.WriteEndElement ();
            
            html.WriteStartElement ("tr");
            foreach (var discipline in competition.EntranceDisciplines) {
                html.WriteElementString ("th", discipline.ShortTitle);
            }
            html.WriteEndElement ();
            
            html.WriteEndElement ();
        }
        
        public void RenderEntrant (CompetitionEntrant entrant, XmlWriter html)
        {
            html.WriteStartElement ("tr");
            html.WriteElementString ("td", entrant.Position.ToString ());
            html.WriteElementString ("td", entrant.Name);
            html.WriteElementString ("td", entrant.FinalMark.ToString ());

            foreach (var mark in entrant.Marks) {
                html.WriteElementString ("td", mark.Mark.ToString ());
            }
            
            html.WriteElementString ("td", YesNoString (entrant.OriginalIn));
            html.WriteElementString ("td", YesNoString (entrant.AcceptedEntrant));
            html.WriteElementString ("td", "");
            html.WriteElementString ("td", "");
            html.WriteEndElement ();
        }

        string YesNoString (bool value) => value ? "да" : "нет";
    }
}