using System.Collections.Generic;

namespace R7.Enrollment.Models
{
    public interface IEntrant
    {
        string Name { get; }

        string Snils { get; }

        string PersonalNumber { get; }

        int Position { get; }

        int AbsolutePosition { get; }

        int CommonRating { get; }

        int AgreedRating { get; }

        int FinalMark { get; }

        int AchievementMark { get; }

        bool OriginalIn { get; }

        bool AcceptedEntrant { get; }

        bool Recommended { get; }

        bool RefusedToBeEnrolled { get; }

        string Status { get; }

        IList<string> MarkStrings { get; }

        IList<EntrantMark> Marks { get; }
    }
}
