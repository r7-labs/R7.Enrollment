using System;
using System.Collections.Generic;
using System.Xml.Linq;
using R7.Enrollment.Models;

namespace R7.Enrollment.Data
{
    public class TandemEntrantRatingDb
    {
        public IList<Competition> Competitions { get; set; } = new List<Competition> ();

        public TandemEntrantRatingDb (string path)
        {
            Load (path);
        }

        void Load (string path)
        {
            var xml = XDocument.Load (path);
            var competitionElem = xml.Root.Element ("competition");
            foreach (var competitionRow in competitionElem.Elements ("row")) {
                var competition = ModelFactory.CreateCompetition (competitionRow);
                Competitions.Add (competition);
                foreach (var entrantRow in competitionRow.Element ("entrant").Elements ("row")) {
                    var entrant = ModelFactory.CreateCompetitionEntrant (entrantRow);
                    competition.Entrants.Add (entrant);
                }
            }
        }

        public void Dump ()
        {
            foreach (var competition in Competitions) {
                Console.WriteLine (competition.EduProgramTitle);
                foreach (var entrant in competition.Entrants) {
                    Console.WriteLine ($"{entrant.Position} {entrant.Name} {entrant.FinalMark}");
                }
            }
        }
    }
}