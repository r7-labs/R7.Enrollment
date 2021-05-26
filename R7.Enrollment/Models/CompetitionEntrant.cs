namespace R7.Enrollment.Models
{
    public class CompetitionEntrant
    {
        public string Name { get; set; }
        
        public string PersonalNumber { get; set; }
        
        public int Position { get; set; }
        
        public int FinalMark { get; set; }
        
        public int AchievementMark { get; set; }
        
        public bool OriginalIn { get; set; }
        
        public bool AcceptedEntrant { get; set; }
    }
}