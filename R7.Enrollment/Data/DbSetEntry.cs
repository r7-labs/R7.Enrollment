using System.Collections.Generic;

namespace R7.Enrollment.Data
{
    public class DbSetEntry
    {
        public int PortalId { get; set; }
        
        public IList<DbSourceFile> SourceFiles { get; set; }
        
        public IList<TandemRatingsDb> Databases { get; set; } = new List<TandemRatingsDb> ();
    }
}