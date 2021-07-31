using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using R7.Enrollment.Components;
using R7.Enrollment.Models;

namespace R7.Enrollment.Renderers
{
    public class RatingsHtmlRenderer
    {
        private RatingsRendererSettings Settings { get; set; }

        private readonly SnilsComparer _snilsComparer = new SnilsComparer ();

        public RatingsHtmlRenderer ()
        {
            Settings = new RatingsRendererSettings ();
        }

        public RatingsHtmlRenderer (RatingsRendererSettings settings)
        {
            Settings = settings;
        }

        public void RenderStandalone (EntrantRatingEnvironment env, XmlWriter html)
        {
            html.WriteStartDocument ();
            html.WriteDocType ("html", null, null, null);
            html.WriteStartElement ("html");
            html.WriteStartElement ("head");

            html.WriteStartElement ("link");
            html.WriteAttributeString ("href",
                "https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css");
            html.WriteAttributeString ("rel", "stylesheet");
            html.WriteAttributeString ("integrity",
                "sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x");
            html.WriteAttributeString ("crossorigin", "anonymous");

            html.WriteEndElement ();
            html.WriteEndElement ();

            html.WriteStartElement ("body");

            html.WriteStartElementWithAttributeString ("style", "type", "text/css");
            html.WriteString (".enr-entrant-row-cutoff td { border-bottom: 6px solid #555; }");
            html.WriteEndElement ();

            html.WriteStartElementWithAttributeString ("div", "class", "container-fluid");
            html.WriteStartElementWithAttributeString ("div", "class", "row");
            html.WriteStartElementWithAttributeString ("div", "class", "col");

            html.WriteStartElement ("h1");
            html.WriteString ($"{env.CampaignTitle}");
            html.WriteElementWithAttributeString ("small",
                $" по состоянию на {env.CurrentDateTime.ToShortDateString ()} {env.CurrentDateTime.ToShortTimeString ()}",
                "class", "text-muted");
            html.WriteEndElement ();

            Render (env, html);

            html.WriteEndElement ();
            html.WriteEndElement ();
            html.WriteEndElement ();

            html.WriteEndElement ();
            html.WriteEndDocument ();
        }

        public void Render (EntrantRatingEnvironment env, XmlWriter html)
        {
            foreach (var competition in env.Competitions) {
                if (Settings.IncludeEmptyCompetitions || competition.Entrants.Count > 0) {
                    html.WriteElementString ("hr", null);
                    RenderCompetition (competition, html);
                }
            }
        }

        public void RenderCompetition (Competition competition, XmlWriter html)
        {
            html.WriteElementString ("h5", $"{competition.OrgUnitTitle}");

            html.WriteElementString ("h4",
                $"{PatchedEduLevelString (competition)}, на базе {competition.EduLevelRequirementGenetiveTitle.ToLower ()}");

            html.WriteElementString ("h2", EduProgramTitle (competition.EduProgram));

            if (competition.CompensationTypeBudget) {
                html.WriteElementString ("h3",
                    $"{competition.EduProgram.Form} форма обучения, {competition.CompensationType} - {FirstCharToLower (competition.CompetitionType)}");
            }
            else {
                html.WriteElementString ("h3",
                    $"{competition.EduProgram.Form} форма обучения, {competition.CompensationType}");
            }

            if (competition.Entrants.Count > 0) {
                // start table
                html.WriteStartElement ("div");
                html.WriteAttributeString ("class", "table-responsive");
                html.WriteStartElement ("table");
                html.WriteAttributeString ("class", "table table-bordered table-striped table-hover");
                RenderEntrantsTableHeader (competition, html);

                foreach (var entrant in competition.Entrants) {
                    RenderEntrantTableRow (entrant, html, competition.Plan);
                }

                // end table
                html.WriteEndElement ();
                html.WriteEndElement ();
            }

            var activeEntrantsCount = competition.Entrants.Count (entr => entr.IsRanked ());
            html.WriteElementString ("p",
                $"Заявлений — {activeEntrantsCount}, число мест — {competition.Plan}");
        }

        public void RenderEntrantsTableHeader (Competition competition, XmlWriter html)
        {
            html.WriteStartElement ("thead");

            html.WriteStartElement ("tr");
            html.WriteElementWithAttributeString ("th", "№", "rowspan", "2");

            if (!Settings.Depersonalize) {
                html.WriteElementWithAttributeString ("th", "Фамилия, имя, отчество", "rowspan", "2");
            }

            html.WriteElementWithAttributeString ("th", "СНИЛС или Личный номер", "rowspan", "2");
            html.WriteElementWithAttributeString ("th", "Сумма баллов", "rowspan", "2");
            html.WriteElementWithAttributeString ("th", "Преимущественное право", "rowspan", "2");

            var disciplinesCount = Math.Max (competition.EntranceDisciplines.Count, 1);
            html.WriteElementWithAttributeString ("th", "Результаты ВИ", "colspan", disciplinesCount.ToString ());

            html.WriteElementWithAttributeString ("th", "Сумма баллов за ИД", "rowspan", "2");
            html.WriteElementWithAttributeString ("th", "Сдан оригинал", "rowspan", "2");
            html.WriteElementWithAttributeString ("th", "Согласие на зачисление", "rowspan", "2");
            html.WriteElementWithAttributeString ("th", "Статус", "rowspan", "2");
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

        public void RenderEntrantTableRow (Entrant entrant, XmlWriter html, int plan)
        {
            html.WriteStartElement ("tr");

            var cssClass = string.Empty;
            if (_snilsComparer.SnilsNotNullAndEquals (entrant.Snils, Settings.Snils) || entrant.PersonalNumber == Settings.PersonalNumber) {
                cssClass += " enr-target-entrant-row";
            }
            if (entrant.IsRanked () && entrant.Rank != null && entrant.Rank.Value == plan) {
                cssClass += " enr-entrant-row-cutoff";
            }
            if (!string.IsNullOrEmpty (cssClass)) {
                html.WriteAttributeString ("class", cssClass);
            }

            html.WriteElementString ("td", entrant.Rank?.ToString () ?? string.Empty);

            if (!Settings.Depersonalize) {
                html.WriteElementString ("td", entrant.Name);
            }

            html.WriteElementString ("td", !string.IsNullOrEmpty (entrant.Snils) ? entrant.Snils : entrant.PersonalNumber);
            html.WriteElementString ("td", entrant.FinalMark.ToString ());
            html.WriteElementString ("td", YesNoString (entrant.HasPreference ()));

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
            html.WriteEndElement ();
        }

        string EnrollmentStateString (Entrant entrant)
        {
            var values = new List<string> ();
            if (entrant.Recommended) {
                values.Add ("рекомендован к зачислению");
            }
            if (entrant.RefusedToBeEnrolled) {
                values.Add ("отказ от зачисления");
            }
            return string.Join ("; ", values);
        }

        string EduProgramTitle (EduProgram eduProgram)
        {
            if (!string.IsNullOrEmpty (eduProgram.Specialization)) {
                return $"{eduProgram.Subject} ({eduProgram.Specialization})";
            }

            return eduProgram.Subject;
        }

        string YesNoString (bool value) => value ? "да" : "нет";

        [Obsolete ("Temporary patch, need to correct edu. level strings in the datasource", false)]
        string PatchedEduLevelString (Competition competition)
        {
            var eduProgram = competition.EduProgram;
            if (eduProgram.Subject.Contains (".04.")) {
                return competition.EduLevel.Replace ("специалитет, ", "");
            }
            if (eduProgram.Subject.Contains (".05.")) {
                return competition.EduLevel.Replace (", магистратура", "");
            }

            return competition.EduLevel;
        }

        string FirstCharToLower (string text)
        {
            if (string.IsNullOrWhiteSpace (text)) {
                return text;
            }

            return text[0].ToString ().ToLower () + text.Substring (1);
        }
    }
}
