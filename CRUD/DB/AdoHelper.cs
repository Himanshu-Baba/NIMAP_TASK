using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CRUD.DB
{
    public class AdoHelper
    {
        private readonly string connectionString;

        public AdoHelper()
        {
            connectionString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
        }

        public DataTable ExecuteQuery(string query, CommandType commandType = CommandType.Text, Dictionary<string, object> parameters = null)
      {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = commandType;

                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                        }
                    }

                    DataTable dt = new DataTable();
                    connection.Open();
                    dt.Load(command.ExecuteReader());
                    return dt;
                }
            }
        }
        public int ExecuteNonQuery(string query, CommandType commandType = CommandType.Text, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = commandType;

                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                        }
                    }

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }
    }
}
