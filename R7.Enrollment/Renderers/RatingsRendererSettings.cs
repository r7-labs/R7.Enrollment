namespace R7.Enrollment.Renderers
{
    public class RatingsRendererSettings
    {
        public bool Depersonalize { get; set; } = true;

        public string Snils { get; set; }

        public string PersonalNumber { get; set; }

        public bool IncludeEmptyCompetitions { get; set; } = true;
    }
}
