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
            var data = entities.RawXlsData.Where(q => q.Confirmed == false).ToList();

            return View(data);
        }

        //
        // GET: /Payment/GetPaymentsData
        [HttpGet]
        [AllowAnonymous]
        [Route ("Payment/GetPaymentsData")]
        public JsonResult GetPaymentsData(DateTime? FileDate)
        {
            var data = entities.RawXlsData.Where(q => q.Confirmed == false && q.row_11 !=null && q.row_11.Length>2 && q.FileDate == FileDate).ToList();

            return Json(data,JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Payment/GetDBFExportData
        [HttpGet]
        [AllowAnonymous]
        [Route("Payment/GetDBFExportData")]
        public JsonResult GetDBFExportData()
        {
            var data = entities.ExportToDBF.Where(q => q.Confirmed == false).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Payment/GetDBFExportData
        [HttpPost]
        [AllowAnonymous]
        [Route("Payment/ExportToDBF")]
        public void ExportToDBF()
        {


            var data = entities.ExportToDBF.Where(q => q.Confirmed == false).ToList();
            string fileName = "PP" +DateTime.Now.ToString("HHmmss");
            WebPaymentsLoader.Classes.DBFUploader.ListIntoDBF<ExportToDBF>(fileName, data);



        }


    }
}