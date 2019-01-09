using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;


namespace WebPaymentsLoader.Controllers
{
    public class UploadFileController : ApiController
    {

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("UploadFile/UploadFile")]
        public IHttpActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["XlsFolder"], _FileName);
                    file.SaveAs(_path);
                }
                
                return Ok();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }

    public class UploadController : Controller
    {
        // GET: Upload  
        public ActionResult Index()
        {
            return View();
        }
        [System.Web.Mvc.HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    UploadFileController uploadFileController = new UploadFileController();
                    uploadFileController.UploadFile(file);
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return View();
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }
        }

    }
}  
 
