using System;
using System.Collections.Generic;
using System.Xml.Linq;
using R7.Enrollment.Models;
using R7.Enrollment.Renderers;

namespace R7.Enrollment.Data
{
    public class TandemEntrantRatingDb
    {
        public IList<EntrantRatingEnvironment> EntrantRatingEnvironments { get; set; } = new List<EntrantRatingEnvironment> ();

        public void LoadEntrantRatingEnvironment (string path)
        {
            var xml = XDocument.Load (path);

            var entrantRatingEnv = new EntrantRatingEnvironment ();
            entrantRatingEnv.CurrentDateTime = DateTime.Parse (xml.Root.Attribute ("currentDateTime").Value);
            entrantRatingEnv.EnrollmentCampaignTitle = xml.Root.Attribute ("enrollmentCampaignTitle").Value;

            var competitionElem = xml.Root.Element ("competition");
            foreach (var competitionRow in competitionElem.Elements ("row"))
            {
                var competition = TandemXmlModelFactory.CreateCompetition (competitionRow);
                competition.CurrentDateTime = entrantRatingEnv.CurrentDateTime;
                entrantRatingEnv.Competitions.Add (competition);

                foreach (var entranceDisciplineRow in competitionRow.Element ("entranceDiscipline").Elements ("row"))
                {
                    var entranceDiscipline = TandemXmlModelFactory.CreateEntranceDiscipline (entranceDisciplineRow);
                    competition.EntranceDisciplines.Add (entranceDiscipline);
                }

                foreach (var entrantRow in competitionRow.Element ("entrant").Elements ("row"))
                {
                    var entrant = TandemXmlModelFactory.CreateCompetitionEntrant (entrantRow);
                    competition.Entrants.Add (entrant);

                    foreach (var markRow in entrantRow.Descendants ("markRows"))
                    {
                        var entrantMark = TandemXmlModelFactory.CreateEntrantMark (markRow, competition);
                        entrant.Marks.Add (entrantMark);
                    }
                }
            }

            EntrantRatingEnvironments.Add (entrantRatingEnv);
        }

        public void Dump ()
        {
            foreach (var entrantRatingEnv in EntrantRatingEnvironments)
            {
                foreach (var competition in entrantRatingEnv.Competitions)
                {
                    Console.WriteLine ("---");
                    Console.WriteLine (competition.CompetitionType);
                    Console.WriteLine (competition.EduProgramSubject);
                    Console.WriteLine (competition.EduProgramTitle);
                    Console.WriteLine (competition.EduLevel);
                    Console.WriteLine (competition.EduProgramForm);
                    Console.WriteLine (competition.OrgUnitTitle);
                    foreach (var entrant in competition.Entrants)
                    {
                        Console.Write ($"{entrant.Position}. {entrant.Name} finalMark:{entrant.FinalMark} ");
                        foreach (var mark in entrant.Marks)
                        {
                            Console.Write ($"{mark.EntranceDiscipline.ShortTitle}:{mark.Mark} ");
                        }

                        Console.Write ($"achievementMark:{entrant.AchievementMark} ");
                        Console.Write ($"originalIn:{entrant.OriginalIn} ");
                        Console.Write ($"acceptedEntrant:{entrant.AcceptedEntrant} ");

                        Console.WriteLine ();
                    }
                }
            }
        }
    }
}