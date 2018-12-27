using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;

namespace WebPaymentsLoader.Classes
{


    public class DBFUploader
    {
        public static void DataSetIntoDBF(string fileName, DataSet dataSet)
        {
            ArrayList list = new ArrayList();

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            string createSql = "create table " + fileName + " (";

            foreach (DataColumn dc in dataSet.Tables[0].Columns)
            {
                string fieldName = dc.ColumnName;

                string type = dc.DataType.ToString();

                switch (type)
                {
                    case "System.String":
                        type = "varchar(100)";
                        break;

                    case "System.Boolean":
                        type = "varchar(10)";
                        break;

                    case "System.Int32":
                        type = "int";
                        break;

                    case "System.Double":
                        type = "Double";
                        break;

                    case "System.DateTime":
                        type = "TimeStamp";
                        break;
                }

                createSql = createSql + "[" + fieldName + "]" + " " + type + ",";

                list.Add(fieldName);
            }

            createSql = createSql.Substring(0, createSql.Length - 1) + ")";

            OleDbConnection con = new OleDbConnection(GetConnection(fileName));

            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = con;

            con.Open();

            cmd.CommandText = createSql;

            cmd.ExecuteNonQuery();

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                string insertSql = "insert into " + fileName + " values(";

                for (int i = 0; i < list.Count; i++)
                {
                    insertSql = insertSql + "'" + ReplaceEscape(row[list[i].ToString()].ToString()) + "',";
                }

                insertSql = insertSql.Substring(0, insertSql.Length - 1) + ")";

                cmd.CommandText = insertSql;

                cmd.ExecuteNonQuery();
            }

            con.Close();
        }

        private static string GetConnection(string path)
        {
            return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=dBASE IV;";
        }

        public static string ReplaceEscape(string str)
        {
            str = str.Replace("'", "''");
            return str;
        }

    }
}