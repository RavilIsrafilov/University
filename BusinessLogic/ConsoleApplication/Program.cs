using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Connector connector = new Connector();
            connector.ConnectToDB();
            connector.GetTableInfo("Users");




            connector.Dispose();

        }
    }
}
