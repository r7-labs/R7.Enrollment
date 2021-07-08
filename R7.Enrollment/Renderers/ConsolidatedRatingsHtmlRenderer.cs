using System;
using System.Xml;
using R7.Enrollment.Models;

namespace R7.Enrollment.Renderers
{
    public class ConsolidatedRatingsHtmlRenderer: RatingsHtmlRenderer
    {
        private RatingsRendererSettings Settings { get; set; }

        private readonly SnilsComparer _snilsComparer = new SnilsComparer ();

        public ConsolidatedRatingsHtmlRenderer ()
        {
            Settings = new RatingsRendererSettings ();
        }

        public ConsolidatedRatingsHtmlRenderer (RatingsRendererSettings settings)
        {
            Settings = settings;
        }

        public override void Render (EntrantRatingEnvironment env, XmlWriter html)
        {
            foreach (var competition in env.ConsolidatedCompetitions) {
                if (Settings.IncludeEmptyCompetitions || competition.Entrants.Count > 0) {
                    html.WriteElementString ("hr", null);
                    RenderCompetition (competition, html);
                }
            }
        }

        public void RenderCompetition (ConsolidatedCompetition competition, XmlWriter html)
        {
            html.WriteElementString ("h5", $"{competition.OrgUnitTitle}");

            if (Settings.UseBasicCompetitionHeader) {
                html.WriteElementString ("h4", $"{PatchedEduLevelString (competition)}, на базе {competition.EduLevelRequirementGenetiveTitle.ToLower ()}");
            }

            html.WriteElementString ("h2", EduProgramTitle (competition.EduProgram));
            html.WriteElementString ("h3", $"{competition.EduProgram.Form} форма обучения");

            // start table
            html.WriteStartElement ("div");
            html.WriteAttributeString ("class", "table-responsive");
            html.WriteStartElement ("table");
            html.WriteAttributeString ("class", "table table-bordered table-striped table-hover");

            if (competition.Entrants.Count > 0) {
                RenderEntrantsTableHeader (competition, html);
            }

            foreach (var entrant in competition.Entrants) {
                RenderEntrantTableRow (entrant, html);
            }

            // end table
            html.WriteEndElement ();
            html.WriteEndElement ();

            if (Settings.UseBasicCompetitionHeader) {
                html.WriteElementString ("p",
                    $"Заявлений — {competition.Entrants.Count}, число мест — {competition.Plan}");
            }
        }

        public void RenderEntrantsTableHeader (ICompetition competition, XmlWriter html)
        {
            html.WriteStartElement ("thead");

            html.WriteStartElement ("tr");
            html.WriteElementWithAttributeString ("th", "№", "rowspan", "2");

            if (Settings.Depersonalize) {
                html.WriteElementWithAttributeString ("th", "Личный номер", "rowspan", "2");
            }
            else {
                html.WriteElementWithAttributeString ("th", "Фамилия, имя, отчество", "rowspan", "2");
            }

            html.WriteElementWithAttributeString ("th", "Сумма баллов", "rowspan", "2");

            var disciplinesCount = Math.Max (competition.EntranceDisciplines.Count, 1);
            html.WriteElementWithAttributeString ("th", "Результаты ВИ", "colspan", disciplinesCount.ToString ());

            html.WriteElementWithAttributeString ("th", "Сумма баллов за ИД", "rowspan", "2");
            html.WriteElementWithAttributeString ("th", "Сдан оригинал", "rowspan", "2");
            html.WriteElementWithAttributeString ("th", "Согласие на зачисление", "rowspan", "2");
            html.WriteElementWithAttributeString ("th", "Статус", "rowspan", "2");
            html.WriteElementWithAttributeString ("th", "Примечание", "rowspan", "2");
            html.WriteElementWithAttributeString ("th", "Информация о зачислении", "rowspan", "2");
            html.WriteElementWithAttributeString ("th", "Категория приема", "rowspan", "2");

            html.WriteEndElement ();

            html.WriteStartElement ("tr");
            foreach (var discipline in competition.EntranceDisciplines) {
                html.WriteElementString ("th", discipline.ShortTitle);
            }
            html.WriteEndElement ();

            html.WriteEndElement ();
        }

        public void RenderEntrantTableRow (ConsolidatedEntrant entrant, XmlWriter html)
        {
            html.WriteStartElement ("tr");

            if (_snilsComparer.SnilsNotNullAndEquals (entrant.Snils, Settings.Snils)
                    || entrant.PersonalNumber == Settings.PersonalNumber) {
                html.WriteAttributeString ("class", "enr-target-entrant-row");
            }

            html.WriteElementString ("td", entrant.Position.ToString ());

            if (Settings.Depersonalize) {
                html.WriteElementString ("td", entrant.PersonalNumber);
            }
            else {
                html.WriteElementString ("td", entrant.Name);
            }

            html.WriteElementString ("td", entrant.FinalMark.ToString ());

            if (entrant.MarkStrings.Count > 0) {
                foreach (var mark in entrant.MarkStrings) {
                    html.WriteElementString ("td", mark);
                }
            }
            else {
                html.WriteElementString ("td", "-");
            }

            html.WriteElementString ("td", entrant.AchievementMark.ToString ());
            html.WriteElementString ("td", YesNoString (entrant.OriginalIn));
            html.WriteElementString ("td", YesNoString (entrant.AcceptedEntrant));
            html.WriteElementString ("td", entrant.Status);
            html.WriteElementString ("td", "");
            html.WriteElementString ("td", EnrollmentStateString (entrant));
            html.WriteElementString ("td", entrant.CompetitionType);

            html.WriteEndElement ();
        }
    }
}
