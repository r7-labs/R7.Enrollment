namespace R7.Enrollment.Models
{
    public enum CompetitionType
    {
        /// <summary>
        /// Без ВИ в рамках КЦП
        /// </summary>
        NoExamCommon = 1,

        /// <summary>
        /// В рамках квоты лиц, имеющих особые права
        /// </summary>
        SpecialRights = 2,

        /// <summary>
        /// Целевой прием
        /// </summary>
        Target = 3,

        /// <summary>
        /// Общий конкурс
        /// </summary>
        Common = 4,

        /// <summary>
        /// Без ВИ по договору
        /// </summary>
        NoExamByContract = 5,

        /// <summary>
        /// По договору
        /// </summary>
        ByContract = 6
    }
}
