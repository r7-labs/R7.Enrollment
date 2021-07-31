namespace R7.Enrollment.Models
{
    /// <summary>
    /// Enumeration for status code (incomplete)
    /// </summary>
    public enum EntrantStatus
    {
        /// <summary>
        /// Активный
        /// </summary>
        Active = 1,

        /// <summary>
        /// Выбыл из конкурса
        /// </summary>
        DroppedOut = 2,

        /// <summary>
        /// Зачислен
        /// </summary>
        Enrolled  = 3,

        /// <summary>
        /// Сданы ВИ
        /// </summary>
        PassedExams = 4,

        // TODO: Which are status codes 5 and 6, possible also >= 9?

        /// <summary>
        /// Забрал документы
        /// </summary>
        TookAwayTheDocs = 7,

        /// <summary>
        /// Отказ от зачисления
        /// </summary>
        RefusedToEnroll = 8
    }
}
