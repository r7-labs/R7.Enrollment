using System;
using System.Xml.Linq;
using R7.Enrollment.Models;

namespace R7.Enrollment.Data
{
    public class TandemRatingsDb
    {
        public EntrantRatingEnvironment EntrantRatingEnvironment { get; set; }

        public void Load (string path)
        {
            var xml = XDocument.Load (path);
            if (xml.Root.Name == "enrEntrantRatingEnvironmentNode") {
                EntrantRatingEnvironment = ParseEntrantRatingEnvironmentNode (xml.Root);
                return;
            }
            throw new ArgumentException ($"Unsupported node type: {xml.Root.Name}");
        }

        EntrantRatingEnvironment ParseEntrantRatingEnvironmentNode (XElement root)
        {
            var entrantRatingEnv = new EntrantRatingEnvironment ();
            entrantRatingEnv.CurrentDateTime = DateTime.Parse (root.Attribute ("currentDateTime").Value);
            entrantRatingEnv.CampaignTitle = root.Attribute ("enrollmentCampaignTitle").Value;

            var competitionElem = root.Element ("competition");
            foreach (var competitionRow in competitionElem.Elements ("row")) {
                var competition = TandemXmlModelFactory.CreateCompetition (competitionRow);
                competition.CurrentDateTime = entrantRatingEnv.CurrentDateTime;
                entrantRatingEnv.Competitions.Add (competition);

                foreach (var entranceDisciplineRow in competitionRow.Element ("entranceDiscipline").Elements ("row")) {
                    var entranceDiscipline = TandemXmlModelFactory.CreateEntranceDiscipline (entranceDisciplineRow);
                    competition.EntranceDisciplines.Add (entranceDiscipline);
                }

                foreach (var entrantRow in competitionRow.Element ("entrant").Elements ("row")) {
                    var entrant = TandemXmlModelFactory.CreateCompetitionEntrant (entrantRow);
                    competition.Entrants.Add (entrant);

                    foreach (var markRow in entrantRow.Descendants ("markRows")) {
                        var entrantMark = TandemXmlModelFactory.CreateEntrantMark (markRow, competition);
                        entrant.Marks.Add (entrantMark);
                    }
                }

                foreach (var eduProgramRow in competitionRow.Element ("eduProgram").Elements ("row")) {
                    TandemXmlModelFactory.FillEduProgram (eduProgramRow, competition.EduProgram);
                }
            }

            return entrantRatingEnv;
        }

        public void Dump ()
        {
            foreach (var competition in EntrantRatingEnvironment.Competitions) {
                Console.WriteLine ("---");
                Console.WriteLine (competition.CompetitionType);
                Console.WriteLine (competition.EduProgram.Subject);
                Console.WriteLine (competition.EduProgram.Title);
                Console.WriteLine (competition.EduLevel);
                Console.WriteLine (competition.EduProgram.Form);
                Console.WriteLine (competition.OrgUnitTitle);
                foreach (var entrant in competition.Entrants) {
                    Console.Write ($"{entrant.Position}. {entrant.Name} finalMark:{entrant.FinalMark} ");
                    foreach (var mark in entrant.Marks) {
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
