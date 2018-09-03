using System;
using static Enum.Enumeration;

namespace BusinessObjects.Entities.Error
{
    public class ErrorDetails
    {
        public DateTime ? Time { get; set; }
        public ErrorSeverityLevel SeverityLevel { get; set; }
        public string Type { get; set; }
        public string  Source { get; set; }
        public string  Message { get; set; }
        public int LineNumber { get; set; }
    }
}
