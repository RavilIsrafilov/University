using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace BusinessLogic
{
    public class Connector:IDisposable
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=May;Integrated Security=True";
        SqlConnection connection;
        public bool ConnectToDB()
        {
            connection = new SqlConnection(connectionString);
            try
            {   
                connection.Open();                
                return true;
            }
            catch (SqlException ex)
            {
                string exMes = ex.Message;
                return false;
            }            
        }



        public bool GetTableInfo(string table)
        {
            string sqlExpression = String.Format("SELECT * FROM {0}", table);
            SqlCommand command = new SqlCommand(sqlExpression, connection);

            var schema = command.ExecuteReader().GetSchemaTable();
            var Columns = schema.Rows.Cast<DataRow>().Select(row => new DbColumnInfo
            {
                Name = row.Field<string>("ColumnName"),
                SqlDataType = GetSqlTypeFromSchemaRow(row) //extract information about SQL type which I need, using schemaRow.Field<string>("DataTypeName"), schemaRow.Field<short>("NumericPrecision"), schemaRow.Field<short>("NumericScale") etc.
            }).ToArray();


            return true;
        }




        public void Dispose()
        {
            connection.Close();
        }
    }
}
