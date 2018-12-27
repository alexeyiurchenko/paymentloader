using Microsoft.Owin;
using Owin;
using System.Threading;
using WebPaymentsLoader.Classes;

[assembly: OwinStartupAttribute(typeof(WebPaymentsLoader.Startup))]
namespace WebPaymentsLoader
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            var threadParser = new Thread(ExcelParser.ParserThread);
            threadParser.Start();
            
        }


    }
}
