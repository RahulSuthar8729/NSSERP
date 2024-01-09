using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data;
using System.Data.SqlClient;
using System;

namespace NSSERP.Models
{
    public class GetAllUsersDetails
    {
        private readonly IConfiguration _configuration;

        public GetAllUsersDetails(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DataTable GetUsers()
        {
            var dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("Constr");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = null;

                try
                {
                    transaction = connection.BeginTransaction();

                    string getUsersQuery = "SELECT * FROM Users";

                    using (SqlCommand getUsersCommand = new SqlCommand(getUsersQuery, connection, transaction))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(getUsersCommand))
                        {
                            // Fill the DataTable with the data from the Users table
                            adapter.Fill(dt);
                        }
                    }

                    // Commit the transaction
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Handle the exception (log, throw, etc.)
                    transaction?.Rollback();
                }
                finally
                {
                    // Close the connection
                    connection.Close();
                }

                return dt;
            }
        }
    }
}
