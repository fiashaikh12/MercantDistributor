using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entities
{
    public class ServiceRes
    {
        public bool IsSuccess { get; set; }
        public string ReturnCode { get; set; }
        public string ReturnMsg { get; set; }
    }
    public class ServiceRes<ExtraDataClass> : ServiceRes
    {
        public ExtraDataClass Data { get; set; }
    }
}
