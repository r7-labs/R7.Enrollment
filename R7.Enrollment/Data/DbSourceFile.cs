using System;

namespace R7.Enrollment.Data
{
    public class DbSourceFile
    {
        public string Name { get; set; }
                
        public long Length { get; set; }
        
        public DateTime LastWriteTimeUtc { get; set; } 
    }
}