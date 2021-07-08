using System;
using System.Collections.Generic;

namespace R7.Enrollment.Models
{
    public interface ICompetition
    {
        // mirroring parent prop here
        DateTime CurrentDateTime { get; }

        string EduLevel { get; }

        string OrgTitle { get; }

        string OrgUnitTitle { get; }

        bool CompensationTypeBudget { get; }

        string EduLevelRequirement { get; }

        string EduLevelRequirementGenetiveTitle { get; }

        EduProgram EduProgram { get; }

        IList<EntranceDiscipline> EntranceDisciplines { get; }
    }
}
