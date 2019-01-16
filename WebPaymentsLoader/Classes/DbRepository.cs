using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using EntityFrameworkExtras.EF6;

namespace WebPaymentsLoader.Classes
{
    public class DbRepository
    {
    }

    [UserDefinedTableType("IDList")]
    public class IDList
    {
        [UserDefinedTableTypeColumn(1)]
        public int ID { get; set; }

     }

    [StoredProcedure("[up_create_payments]")]
    public class CreatePayments
    {
        [StoredProcedureParameter(SqlDbType.Int, ParameterName = "template_id")]
        public int TemplateId { get; set; }

        
        [StoredProcedureParameter(SqlDbType.Udt, ParameterName = "List")]
        public List<IDList> IDList { get; set; }
    }

    [StoredProcedure("[up_export_data_set_confirmed]")]
    public class PaymentsConfirmed
    {
        [StoredProcedureParameter(SqlDbType.VarChar, ParameterName = "file_name")]
        public string FileName { get; set; }


        [StoredProcedureParameter(SqlDbType.Udt, ParameterName = "List")]
        public List<IDList> IDList { get; set; }
    }
}