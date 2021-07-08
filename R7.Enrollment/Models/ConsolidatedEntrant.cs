using System.Collections.Generic;

namespace R7.Enrollment.Models
{
    // TODO: Rename this
    public class ConsolidatedEntrant: IEntrant
    {
        private IEntrant _entrant;

        #region IEntrant implementation

        public string Name => _entrant.Name;

        public string Snils => _entrant.Snils;

        public string PersonalNumber => _entrant.PersonalNumber;

        public int Position => _entrant.Position;

        public int AbsolutePosition => _entrant.AbsolutePosition;

        public int CommonRating => _entrant.CommonRating;

        public int AgreedRating => _entrant.AgreedRating;

        public int FinalMark => _entrant.FinalMark;

        public int AchievementMark => _entrant.AchievementMark;

        public bool OriginalIn => _entrant.OriginalIn;

        public bool AcceptedEntrant => _entrant.AcceptedEntrant;

        public bool Recommended => _entrant.Recommended;

        public bool RefusedToBeEnrolled => _entrant.RefusedToBeEnrolled;

        public string Status => _entrant.Status;

        public IList<string> MarkStrings => _entrant.MarkStrings;

        public IList<EntrantMark> Marks => _entrant.Marks;

        #endregion

        public int CompetitionTypeCode { get; }

        public string CompetitionType { get; }

        public string CompensationType { get; }

        public ConsolidatedEntrant (IEntrant entrant, Competition competition)
        {
            _entrant  = entrant;

            CompetitionTypeCode = competition.CompetitionTypeCode;
            CompetitionType = competition.CompetitionType;
            CompensationType = competition.CompensationType;
        }
    }
}
