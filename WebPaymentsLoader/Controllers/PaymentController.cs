using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebPaymentsLoader.Models;

namespace WebPaymentsLoader.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private static Payments_OBEntities entities = new Payments_OBEntities();

        public PaymentController()
        {
        }


        //
        // GET: /Payment/GetPayments
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetPayments()
        {
            var data = entities.RawXlsData.Select(q => q.Confirmed == false).ToList();

            return View(data);
        }

        
        
    }
}