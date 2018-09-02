using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entities
{
     public class Response
    {
        public Response()
        {
            this.IsSucces = true;
            this.Token = string.Empty;
            this.responseMsg = new HttpResponseMessage() { StatusCode=System.Net.HttpStatusCode.Unauthorized };
        }
        public bool IsSucces { get; set; }
        public string Token { get; set; }
        public HttpResponseMessage responseMsg { get; set; }
    }
}
