using System;
using static Enum.Enumeration;

namespace BusinessLogicLayer.Repository.Interface
{
    public interface ILog
    {
        void WriteLog(Exception ex, ErrorSeverityLevel level);
    }
}
