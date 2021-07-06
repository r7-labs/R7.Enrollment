using System.Collections.Generic;

namespace R7.Enrollment.Data
{
    public class DbSetEntry
    {
        public IList<DbSourceFile> SourceFiles { get; set; }

        public IList<TandemRatingsDb> Databases { get; set; } = new List<TandemRatingsDb> ();
    }
}
