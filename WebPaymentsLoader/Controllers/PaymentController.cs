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
        public JsonResult GetPaymentsData(DateTime? FileDateFrom , DateTime? FileDateTo)
        {
            var data = entities.RawXlsData.Where(q => q.Confirmed == false && q.row_11 !=null && q.row_11.Length>2 
                                            && q.FileDate >= FileDateFrom && q.FileDate <= FileDateTo).ToList();

            return Json(data,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Payment/SetPaymentsData")]
        public void SetPaymentsData(int RowId,Boolean Accepted )
        {

            var row = entities.RawXlsData.SingleOrDefault(q => q.Id == RowId);
            if (row != null)
            {
                row.row_11 = Accepted.ToString();
                entities.SaveChanges();
            }
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
        // GET: /Payment/GetDBFExportData
        [HttpGet]
        [AllowAnonymous]
        [Route("Payment/GetAccountsData")]
        public JsonResult GetAccountsData()
        {
            var data = entities.vw_paymentsAccountTemplates.ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        //
        // POST: /Payment/ExportToDBF
        [HttpPost]
        [AllowAnonymous]
        [Route("Payment/ExportToDBF")]
        public void ExportToDBF(string FileName)
        {


            var data = entities.ExportToDBF.Where(q => q.Confirmed == false).ToList();
            string fileName =  FileName.Substring(0,8) ;//"PP" +DateTime.Now.ToString("HHmmss");
            WebPaymentsLoader.Classes.DBFUploader.ListIntoDBF<ExportToDBF>(fileName, data);



        }


        //
        // POST: /Payment/CreatePayments
        [HttpPost]
        [AllowAnonymous]
        [Route("Payment/CreatePayments")]
        public void CreatePayments(int AccountId = 1)
        {

            entities.up_upload_to_dbf(AccountId);
            
        }




    }
}