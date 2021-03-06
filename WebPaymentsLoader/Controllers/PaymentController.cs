﻿using System;
using System.Data.Entity;
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
using EntityFrameworkExtras;
using WebPaymentsLoader.Classes;
using System.Collections.Generic;
using EntityFrameworkExtras.EF6;


namespace WebPaymentsLoader.Controllers
{
    [System.Web.Mvc.Authorize]
    public class PaymentController : Controller
    {
        private static Payments_OBEntities entities = new Payments_OBEntities();
        private static DbContext context = new DbContext("name=Payments_OBEntities");
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public PaymentController()
        {
        }



        ///GET: /Payment/GetPaymentsData
       [HttpGet]
       [AllowAnonymous]
       [Route("Payment/GetPaymentsData")]
        public JsonResult GetPaymentsData(DateTime? FileDateFrom, DateTime? FileDateTo, Boolean? Confirmed = false)
        {
            logger.Info("Payment/GetPaymentsData?FileDateFrom=" + FileDateFrom.ToString() + "&FileDateTo=" + FileDateTo.ToString());
            var data = entities.RawXlsData.Where(q => q.Confirmed == false && q.row_11 != null && q.row_11.Length > 2
                                            && q.FileDate >= FileDateFrom && q.FileDate <= FileDateTo).AsNoTracking().ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Payment/SetPaymentsData")]
        public void SetPaymentsData(int RowId, Boolean Accepted)
        {

            var row = entities.RawXlsData.SingleOrDefault(q => q.Id == RowId);
            if (row != null)
            {
                row.row_11 = Accepted.ToString();
                entities.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Помічає строки як завершені в завантаженому файлі, щоб більше їх не відображати
        /// </summary>
        /// <param name="listId"></param>
        [HttpPost]
        [AllowAnonymous]
        [Route("Payment/SetPaymentsDataConfirmed")]
        public void SetPaymentsDataConfirmed(List<IDList> listId)
        {
            logger.Info("Payment/SetPaymentsDataConfirmed");
            List<int> IDs = listId.Select(q => q.ID).ToList();

            entities.RawXlsData.Where(q => IDs.Contains(q.Id)).ToList().ForEach(u => u.Confirmed=true);
            entities.SaveChanges();
        }


        ////
        //// GET: /Payment/GetExportedData
        [HttpGet]
        [AllowAnonymous]
        [Route("Payment/GetExportData")]
        public JsonResult GetExportData()
        {
            logger.Info("Payment/GetExportData");
            var data = entities.ExportToDBF.Where(q => q.Confirmed == false).AsNoTracking().ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        ////
        //// GET: /Payment/GetExportedData
        [HttpGet]
        [AllowAnonymous]
        [Route("Payment/GetExportedData")]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public JsonResult GetExportedData()
        {
            logger.Info("Payment/GetExportedData");
            var data = entities.ExportToDBF.Where(q => q.Confirmed == true).AsNoTracking().ToList();

            logger.Info(data.First().FileName);
            return Json(data, JsonRequestBehavior.AllowGet);
        }



        //POST: /Payment/SetExportData
        [HttpPost]
        [AllowAnonymous]
        [Route("Payment/SetExportData")]
        public void SetExportData(int RowId, string Narrative)
        {
            logger.Info("Payment/SetExportData?RowId=" + RowId.ToString());
            var row = entities.ExportToDBF.SingleOrDefault(q => q.Id == RowId);
            if (row != null)
            {
                row.NAZN = Narrative;
                entities.SaveChangesAsync();
            }
        }

        ////
        //// GET: /Payment/GetDBFExportData
        [HttpGet]
        [AllowAnonymous]
        [Route("Payment/GetAccountsData")]
        public JsonResult GetAccountsData()
        {
            logger.Info("Payment/GetAccountsData");
            var data = entities.vw_paymentsAccountTemplates.ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        ////
        //// GET: /Payment/SetAccountsData
        [HttpPost]
        [AllowAnonymous]
        [Route("Payment/SetAccountsData")]
        public void SetAccountsData(vw_paymentsAccountTemplates model)
        {
            logger.Info("Payment/SetAccountsData");
            entities.up_merge_template(model.Id, model.EDRPO, model.MFO, model.Account, model.Narrative, model.NAME, model.NAME);
            entities.SaveChanges();
           
            
        }


        //
        // POST: /Payment/ExportToDBF
        /// <summary>
        /// Create DBF file from selected rows
        /// </summary>
        /// <param name="FileName"></param>
        [HttpPost]
        [AllowAnonymous]
        [Route("Payment/ExportToDBF")]
        public void ExportToDBF(string FileName, List<IDList> listId)
        {
            logger.Info("Payment/ExportToDBF?FileName=" + FileName);
            List<int> IDs = listId.Select(q => q.ID ).ToList();


            var data = entities.ExportToDBF.Where(q => q.Confirmed == false && IDs.Contains( q.Id) ).ToList();
            string fileName = FileName.Substring(0, 8);//"PP" +DateTime.Now.ToString("HHmmss");
            WebPaymentsLoader.Classes.DBFUploader.ListIntoDBF<ExportToDBF>(fileName, data);

            var procedure = new PaymentsConfirmed()
            {
                FileName = fileName + ".dbf",
                IDList = listId
            };
            context.Database.ExecuteStoredProcedure(procedure);

            Response.Redirect(@"~/files/" + fileName + ".dbf");

        }

        //
        // POST: /Payment/Confirmed
        /// <summary>
        /// set payments confirmed for selected rows
        /// </summary>
        /// <param name="FileName"></param>
        [HttpPost]
        [AllowAnonymous]
        [Route("Payment/Confirmed")]
        public void Confirmed(List<IDList> listId)
        {
            logger.Info("Payment/Confirmed");
            List<int> IDs = listId.Select(q => q.ID).ToList();

            var procedure = new PaymentsConfirmed()
            {
                FileName = "",
                IDList = listId
            };
            context.Database.ExecuteStoredProcedure(procedure);
            
        }


        //
        // POST: /Payment/CreatePayments
        [HttpPost]
        [AllowAnonymous]
        [Route("Payment/CreatePayments")]
        public void CreatePayments(int TemplateId , List<IDList> listId)
        {
            logger.Info("Payment/CreatePayments?TemplateId=" + TemplateId.ToString());


            var procedure = new CreatePayments()
            {
                TemplateId  = TemplateId,
                IDList = listId
            };
            context.Database.ExecuteStoredProcedure(procedure);
        }



        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            //Log the error!!
            logger.Error(filterContext.Exception);

            //Redirect or return a view, but not both.
            filterContext.Result = RedirectToAction("Index", "ErrorHandler");
            // OR 
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml"
            };
        }


    }
}