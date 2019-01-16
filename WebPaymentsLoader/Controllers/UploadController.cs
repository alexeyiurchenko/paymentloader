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
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

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
        
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("UploadFile/UploadFile_v2")]
        public HttpResponseMessage Post()
        {
            var httpContext = HttpContext.Current;
            logger.Info("UploadFile/UploadFile_v2:Count files" + httpContext.Request.Files.Count.ToString());

            // Check for any uploaded file  
            if (httpContext.Request.Files.Count > 0)
                {
                logger.Info("httpContext.Request.Files.Count:" + httpContext.Request.Files.Count.ToString());

                //Loop through uploaded files  
                for (int i = 0; i < httpContext.Request.Files.Count; i++)
                    {
                        HttpPostedFile httpPostedFile = httpContext.Request.Files[i];
                        if (httpPostedFile != null)
                        {
                            // Construct file save path  
                            var fileSavePath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["XlsFolder"], httpPostedFile.FileName);

                        logger.Info("fileSavePath:" + fileSavePath);

                        // Save the uploaded file  
                        httpPostedFile.SaveAs(fileSavePath);
                        }
                    }
                }

                // Return status code  
                return Request.CreateResponse(HttpStatusCode.Created);
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
 
