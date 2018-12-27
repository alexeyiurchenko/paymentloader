using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;


namespace WebPaymentsLoader.Classes
{
    public class ExcelParser
    {

        public static string folder = System.Configuration.ConfigurationManager.AppSettings["XlsFolder"]; //@"C:\Users\aiurchenko\Documents\huspi";
        public static string dbfFolder = System.Configuration.ConfigurationManager.AppSettings["DBFFolder"]; //@"C:\Users\aiurchenko\Documents\huspi";


        public static string fileName = string.Format("{0}\\report.xls", @"C:\Users\aiurchenko\Documents\huspi" /*Directory.GetCurrentDirectory()*/);
        public static string connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", fileName);


        public static string GetNextFile()
        {
            foreach (var a in Directory.GetFiles(folder, "*.xls"))
            {
                return a;
            }
                return "";
        }


        public static void MoveFile(string fileName)
        {
            // To move a file or folder to a new location:
            System.IO.File.Move(fileName, fileName+".zip");
        }


        public static void Parse()
        {
            string nextFile = GetNextFile();

            var adapter = new OleDbDataAdapter("SELECT * FROM [objects$]", connectionString);
            var ds = new DataSet();

            adapter.Fill(ds, "RawXlsData");

            DataTable data = ds.Tables["RawXlsData"];

            Payments_OBEntities entities = new Payments_OBEntities();

            

            foreach (DataRow row in data.Rows)
            {
                RawXlsData rawXlsData = new RawXlsData() { Confirmed = false,
                    FileName = fileName,
                    row_1 = row.ItemArray[1].ToString(),
                    row_2 = row.ItemArray[2].ToString(),
                    row_3 = row.ItemArray[3].ToString(),
                    row_4 = row.ItemArray[4].ToString(),
                    row_5 = row.ItemArray[5].ToString(),
                    row_6 = row.ItemArray[6].ToString(),
                    row_7 = row.ItemArray[7].ToString(),
                    row_8 = row.ItemArray[8].ToString(),
                    row_9 = row.ItemArray[9].ToString(),
                    row_10 = row.ItemArray[10].ToString(),
                    row_11 = row.ItemArray[11].ToString()

                };

                entities.RawXlsData.Add(rawXlsData);
                

            }
            entities.SaveChangesAsync();
            MoveFile(nextFile);

        }

    }
}