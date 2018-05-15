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
            //string sqlExpression = String.Format("SELECT * FROM {0}", table);
            //SqlCommand command = new SqlCommand(sqlExpression, connection);

            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SET FMTONLY ON; select surname from users; SET FMTONLY OFF";
            SqlDataReader reader = cmd.ExecuteReader();
            SqlDbType type = (SqlDbType)(int)reader.GetSchemaTable().Rows[0]["ProviderType"];


            //var schema = command.ExecuteReader().GetSchemaTable();
            //var Columns = schema.Rows.Cast<DataRow>().Select(row => new DbColumnInfo
            //{
            //    Name = row.Field<string>("ColumnName"),
            //    SqlDataType = GetSqlTypeFromSchemaRow(row) //extract information about SQL type which I need, using schemaRow.Field<string>("DataTypeName"), schemaRow.Field<short>("NumericPrecision"), schemaRow.Field<short>("NumericScale") etc.
            //}).ToArray();


            return true;
        }
        
        public void Dispose()
        {
            connection.Close();
        }


        public static void CopyTable(IDbConnection source, IDbConnection destination, String sourceSQL, String destinationTableName)
        {
            System.Diagnostics.Debug.WriteLine(System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss") +
                " " + destinationTableName + " load started");
            IDbCommand cmd = source.CreateCommand();
            cmd.CommandText = sourceSQL;
            System.Diagnostics.Debug.WriteLine("\tSource SQL: " + sourceSQL);
            try
            {
                source.Open();
                destination.Open();
                IDataReader rdr = cmd.ExecuteReader();
                DataTable schemaTable = rdr.GetSchemaTable();

                IDbCommand insertCmd = destination.CreateCommand();
                string paramsSQL = String.Empty;

                //build the insert statement
                foreach (DataRow row in schemaTable.Rows)
                {
                    if (paramsSQL.Length > 0)
                        paramsSQL += ", ";
                    paramsSQL += "@" + row["ColumnName"].ToString();

                    IDbDataParameter param = insertCmd.CreateParameter();
                    param.ParameterName = "@" + row["ColumnName"].ToString();
                    param.SourceColumn = row["ColumnName"].ToString();

                    if (row["DataType"] == typeof(System.DateTime))
                    {
                        param.DbType = DbType.DateTime;
                    }

                    //Console.WriteLine(param.SourceColumn);
                    insertCmd.Parameters.Add(param);
                }
                insertCmd.CommandText =
                    String.Format("insert into {0} ( {1} ) values ( {2} )",
                    destinationTableName, paramsSQL.Replace("@", String.Empty),
                    paramsSQL);
                int counter = 0;
                int errors = 0;
                while (rdr.Read())
                {
                    try
                    {
                        foreach (IDbDataParameter param in insertCmd.Parameters)
                        {
                            object col = rdr[param.SourceColumn];

                            //special check for SQL Server and 
                            //datetimes less than 1753
                            if (param.DbType == DbType.DateTime)
                            {
                                if (col != DBNull.Value)
                                {
                                    //sql server can not have dates less than 1753
                                    if (((DateTime)col).Year < 1753)
                                    {
                                        param.Value = DBNull.Value;
                                        continue;
                                    }
                                }
                            }

                            param.Value = col;

                            //uncomment this line to see the 
                            //values being used for the insert
                            //System.Diagnostics.Debug.WriteLine( param.SourceColumn + " --> " + 
                            //param.ParameterName + " = " + col.ToString() );
                        }
                        insertCmd.ExecuteNonQuery();
                        //un-comment this line to get a record count. You may only want to show status for every 1000 lines
                        //this can be done by using the modulus operator against the counter variable
                        //System.Diagnostics.Debug.WriteLine(++counter);
                    }
                    catch (Exception ex)
                    {
                        if (errors == 0)
                            System.Diagnostics.Debug.WriteLine(ex.Message.ToString());
                        errors++;
                    }
                }
                System.Diagnostics.Debug.WriteLine(errors + " errors");
                System.Diagnostics.Debug.WriteLine(counter + " records copied");
                System.Diagnostics.Debug.WriteLine(System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss") +
                " " + destinationTableName + " load completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace.ToString());
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
                destination.Close();
                source.Close();
            }
        }
    }
}
