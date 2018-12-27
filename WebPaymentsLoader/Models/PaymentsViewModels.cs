using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace WebPaymentsLoader.Models
{
    public class RawDataViewModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string row_1 { get; set; }
        public string row_2 { get; set; }
        public string row_3 { get; set; }
        public string row_4 { get; set; }
        public string row_5 { get; set; }
        public string row_6 { get; set; }
        public string row_7 { get; set; }
        public string row_8 { get; set; }
        public string row_9 { get; set; }
        public string row_10 { get; set; }
        public string row_11 { get; set; }
        public string row_12 { get; set; }
        public string row_13 { get; set; }
        public bool Confirmed { get; set; }
    }

    public class ExportToDBFViewModel
    {
        public int Id { get; set; }
        public int KB_A { get; set; }
        public string KK_A { get; set; }
        public int KB_B { get; set; }
        public string KK_B { get; set; }
        public bool DK { get; set; }
        public int SUMMA { get; set; }
        public string VID { get; set; }
        public string NDOC { get; set; }
        public int I_VA { get; set; }
        public System.DateTime DA { get; set; }
        public System.DateTime? DA_DOC { get; set; }
        public string NK_A { get; set; }
        public string NK_B { get; set; }
        public string NAZN { get; set; }
        public string KOD_A { get; set; }
        public string KOD_B { get; set; }
        public string CPU { get; set; }
        public bool Confirmed { get; set; }
    }
}