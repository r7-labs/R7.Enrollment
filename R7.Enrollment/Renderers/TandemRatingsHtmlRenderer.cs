using System;
using System.Collections.Generic;
using System.Xml;
using R7.Enrollment.Models;

namespace R7.Enrollment.Renderers
{
    public class TandemRatingsHtmlRenderer
    {
        private TandemRatingsRendererSettings Settings { get; set; }

        private readonly SnilsComparer _snilsComparer = new SnilsComparer ();

        public TandemRatingsHtmlRenderer ()
        {
            Settings = new TandemRatingsRendererSettings ();
        }

        public TandemRatingsHtmlRenderer (TandemRatingsRendererSettings settings)
        {
            Settings = settings;
        }

        public void RenderStandalone (EntrantRatingEnvironment env, XmlWriter html)
        {
            html.WriteStartDocument ();
            html.WriteDocType ("html", null,  null, null);
            html.WriteStartElement ("html");
            html.WriteStartElement ("head");

            html.WriteStartElement ("link");
            html.WriteAttributeString ("href", "https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css");
            html.WriteAttributeString ("rel", "stylesheet");
            html.WriteAttributeString ("integrity", "sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x");
            html.WriteAttributeString ("crossorigin", "anonymous");

            html.WriteEndElement ();
            html.WriteEndElement ();

            html.WriteStartElement ("body");

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
                if (competition.Entrants.Count > 0) {
                    html.WriteElementString ("hr", null);
                    RenderCompetition (competition, html);
                }
            }
        }

        public void RenderCompetition (Competition competition, XmlWriter html)
        {
            html.WriteElementString ("h5", $"{competition.OrgUnitTitle}");

            if (Settings.UseBasicCompetitionHeader) {
                html.WriteElementString ("h4", $"{competition.EduLevel}");
            }

            html.WriteElementString ("h2", $"{competition.EduProgram.FullTitle}");

            if (!competition.CompensationTypeBudget) {
                html.WriteElementString ("h3",
                    $"{competition.EduProgram.Form} форма, {competition.CompensationType}");
            }
            else {
                html.WriteElementString ("h3",
                    $"{competition.EduProgram.Form} форма, {competition.CompensationType}, {competition.CompetitionType.ToLower ()}");
            }

            if (!Settings.UseBasicCompetitionHeader) {
                RenderCompetitionHeader (competition, html);
            }

            // start table
            html.WriteStartElement ("div");
            html.WriteAttributeString ("class", "table-responsive");
            html.WriteStartElement ("table");
            html.WriteAttributeString ("class", "table table-bordered table-striped table-hover");

            RenderEntrantsTableHeader (competition, html);

            foreach (var entrant in competition.Entrants) {
                RenderEntrant (entrant, html);
            }

            // end table
            html.WriteEndElement ();
            html.WriteEndElement ();

            if (Settings.UseBasicCompetitionHeader) {
                html.WriteElementString ("p",
                    $"Заявлений — {competition.Entrants.Count}, число мест — {competition.Plan}");
            }
        }

        void RenderCompetitionHeader (Competition competition, XmlWriter html)
        {
            // start table
            html.WriteStartElement ("div");
            html.WriteAttributeString ("class", "table-responsive");
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
            html.WriteElementWithAttributeString ("td", competition.EduProgram.Subject, "colspan", "2");
            html.WriteElementString ("td", "");
            html.WriteElementString ("td", $"{competition.EduProgram.ConditionsWithForm}");
            html.WriteEndElement ();

            // 4th row
            html.WriteStartElement ("tr");

            html.WriteElementString ("td", "Набор ОП:");
            html.WriteStartElement ("td");
            html.WriteAttributeString ("colspan", "4");
            html.WriteString (competition.EduProgram.Title);
            html.WriteEndElement ();

            html.WriteEndElement();

            // 5th row
            html.WriteStartElement ("tr");

            html.WriteStartElement ("td");
            html.WriteAttributeString ("colspan", "3");
            if (competition.CompensationTypeBudget) {
                html.WriteString ($"Число мест на бюджет (КЦП) — {competition.Plan}");
            }
            else {
                html.WriteString ($"Число мест с оплатой стоимости обучения — {competition.Plan}");
            }
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
            if (competition.CompensationTypeBudget) {
                html.WriteString ($"на бюджет (КЦП) — {competition.Entrants.Count}");
            }
            else {
                html.WriteString ($"на места с оплатой стоимости обучения — {competition.Entrants.Count}");
            }
            html.WriteEndElement ();

            html.WriteEndElement ();

            // 6th row
            html.WriteStartElement ("tr");
            html.WriteElementString ("td", "Образовательные программы:");

            html.WriteElementWithAttributeString ("td",
                $"{competition.EduProgram.TitleAndConditionsShortWithForm}", "colspan", "4");

            html.WriteEndElement ();

            // end table
            html.WriteEndElement ();
            html.WriteEndElement ();

            html.WriteElementString ("h4", $"{competition.CompetitionType} (заявлений — {competition.Entrants.Count}, число мест — {competition.Plan})");
        }

        public void RenderEntrantsTableHeader (Competition competition, XmlWriter html)
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
            html.WriteEndElement ();

            html.WriteStartElement ("tr");
            foreach (var discipline in competition.EntranceDisciplines) {
                html.WriteElementString ("th", discipline.ShortTitle);
            }
            html.WriteEndElement ();

            html.WriteEndElement ();
        }

        public void RenderEntrant (Entrant entrant, XmlWriter html)
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

            if (entrant.Marks.Count >= 1) {
                foreach (var mark in entrant.Marks) {
                    html.WriteElementString ("td", mark.Mark.ToString ());
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

        string YesNoString (bool value) => value ? "да" : "нет";
    }
}
