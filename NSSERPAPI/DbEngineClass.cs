using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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

        public string ExecuteInsertStoredProcedure(string storedProcedureName, object parameters = null)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@paramData", JsonConvert.SerializeObject(parameters));
                    dynamicParameters.Add("@returnResult", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

                    connection.Execute(storedProcedureName, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return dynamicParameters.Get<string>("@returnResult");
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string ExecuteUpdateStoredProcedure(string storedProcedureName,object id ,object parameters = null)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@paramData", JsonConvert.SerializeObject(parameters));
                    dynamicParameters.Add("@Id", id);
                    dynamicParameters.Add("@returnResult", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

                    connection.Execute(storedProcedureName, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return dynamicParameters.Get<string>("@returnResult");
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
