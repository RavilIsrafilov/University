using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic;
using System.Data.SqlClient;
using System.Data;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Connector connector = new Connector();
            connector.ConnectToDB();
            connector.GetTableInfo("Users");

            SqlConnection src = new SqlConnection("Data Source = localhost; Initial Catalog = Northwind; Integrated Security = True");
            OdbcConnection dest = new OdbcConnection("DSN=my_database;Uid=northwind_user;Pwd=password");

            Utils.CopyTable(src, dest, "select * from Products", "ProductsCopy");


            connector.Dispose();

        }
    }
}
