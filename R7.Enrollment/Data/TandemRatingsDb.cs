using System;
using System.Xml.Linq;
using R7.Enrollment.Models;

namespace R7.Enrollment.Data
{
    public class TandemRatingsDb
    {
        public EntrantRatingEnvironment EntrantRatingEnvironment { get; set; }

        private readonly TandemXmlModelFactory _modelFactory = new TandemXmlModelFactory ();

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
            var env = new EntrantRatingEnvironment ();
            env.CurrentDateTime = DateTime.Parse (root.Attribute ("currentDateTime").Value);
            env.CampaignTitle = root.Attribute ("enrollmentCampaignTitle").Value;

            var competitionElem = root.Element ("competition");
            foreach (var competitionRow in competitionElem.Elements ("row")) {
                var competition = _modelFactory.CreateCompetition (competitionRow);
                competition.CurrentDateTime = env.CurrentDateTime;
                env.Competitions.Add (competition);

                foreach (var entranceDisciplineRow in competitionRow.Element ("entranceDiscipline").Elements ("row")) {
                    var entranceDiscipline = _modelFactory.CreateEntranceDiscipline (entranceDisciplineRow);
                    competition.EntranceDisciplines.Add (entranceDiscipline);
                }

                foreach (var entrantRow in competitionRow.Element ("entrant").Elements ("row")) {
                    var entrant = _modelFactory.CreateCompetitionEntrant (entrantRow);
                    competition.Entrants.Add (entrant);

                    foreach (var markRow in entrantRow.Descendants ("markRows")) {
                        var entrantMark = _modelFactory.CreateEntrantMark (markRow, competition, entrant);
                        entrant.Marks.Add (entrantMark);
                    }
                }

                foreach (var eduProgramRow in competitionRow.Element ("eduProgram").Elements ("row")) {
                    _modelFactory.FillEduProgram (eduProgramRow, competition.EduProgram);
                }
            }

            return env;
        }
    }
}
