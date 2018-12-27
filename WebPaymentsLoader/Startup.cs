using Microsoft.Owin;
using Owin;
using WebPaymentsLoader.Classes;

[assembly: OwinStartupAttribute(typeof(WebPaymentsLoader.Startup))]
namespace WebPaymentsLoader
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ExcelParser.Parse();
        }


    }
}
