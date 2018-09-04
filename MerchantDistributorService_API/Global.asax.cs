using AutoFactory;
using Repository;
using System;
using System.Web;
using System.Web.Http;

namespace MerchantDistributorService_API
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            Bootstrapper.Run();
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            // Get the exception object.
            Exception exc = Server.GetLastError();
            // Handle HTTP errors
            LogManager.WriteLog(exc, Enum.Enums.SeverityLevel.Important);
        }
    }
}
