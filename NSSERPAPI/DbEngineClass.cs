using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NSSERPAPI.Models.NationalGangotri;
namespace NSSERPAPI
{
    public class DbEngineClass
    {
        private readonly string _connectionString;

        public DbEngineClass(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public List<dynamic> ExecuteStoredProcedure(string storedProcedureName, object parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = connection.Query(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }

    }
}
