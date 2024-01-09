using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace NSSERPAPI.Db_functions_for_Gangotri
{
    public class Db_functions
    {
        private readonly string _connectionString;

        public Db_functions(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        // Add this method to get countries
        public List<dynamic> GetCountries()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Execute stored procedure to get countries
                var result = connection.Query("GetCountries", commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public IEnumerable<dynamic> GetDonationReciveDetails()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query("GetDonationReceiveDetails", commandType: CommandType.StoredProcedure);
            }
        }

        public List<dynamic> GetActiveCities()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Execute stored procedure to get active cities
                return connection.Query("GetActiveCities", commandType: CommandType.StoredProcedure).AsList();
            }
        }
        public IEnumerable<dynamic> GetPaymentModes()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query("GetPaymentModes", commandType: CommandType.StoredProcedure);
            }
        }
        public List<dynamic> GetStates()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Execute stored procedure to get states
                return connection.Query("GetStates", commandType: CommandType.StoredProcedure).AsList();
            }
        }

        public IEnumerable<dynamic> GetStatesByCountry(int countryId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Use Dapper to call the stored procedure
                return connection.Query<dynamic>("GetStatesByCountry", new { CountryID = countryId }, commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<dynamic> GetDistrictsByState(int stateId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<dynamic>("GetDistrictsByState", new { StateID = stateId }, commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<dynamic> GetCitiesByDistrictID(int districtId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<dynamic>("GetCitiesByDistrictID", new { DistrictID = districtId }, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<dynamic> GetCurrencyListWithCountry()
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                var currencies = dbConnection.Query<dynamic, dynamic, dynamic>(
              "GetCurrencyListWithCountry",
              (currency, country) =>
              {
                  // Assuming "CountryID" is a property in the dynamic result
                  ((IDictionary<string, object>)currency).Add("CountryMaster", country);
                  return currency;
              },
              splitOn: "CountryID",
              commandType: CommandType.StoredProcedure
          );

                return currencies;
            }
        }
        public dynamic GetLocationDetailsByPinCode(string pinCode)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new { InputPincode = pinCode };
                return connection.QueryFirstOrDefault<dynamic>("GetLocationDetailsByPincode", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<dynamic> GetAllBankMasters()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<dynamic>("GetAllBankMasters", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<dynamic> getHeads()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<dynamic>("GetHeads", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<dynamic> getReceiveHeads()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<dynamic>("GetReceiveHeads", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<dynamic> GetEvents()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<dynamic>("GetEvents", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<dynamic> GetSubHeadByHead(int HeadID,string DataFlag)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { Purpose_ID = HeadID, DataFlag=DataFlag };
                return connection.Query<dynamic>("[GetSubHeadByHeadid]", parameters, commandType: CommandType.StoredProcedure);

            }
        }
        public IEnumerable<dynamic> GetQtyAmtBySubHead(int yojnaid,string DataFlag)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { YojnaID = yojnaid,DataFlag= DataFlag };
                return connection.Query<dynamic>("GetQtyAmtBySubHead", parameters, commandType: CommandType.StoredProcedure);

            }
        }
        public IEnumerable<dynamic> getSubHeads()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<dynamic>("GetSubHeads", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<dynamic> GetallCampaigns()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<dynamic>("GetAllCampaigns", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<dynamic> GetDonorINstructionsMaster()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<dynamic>("GetDonorInstructions", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<dynamic> GetDonorInstructionsbyid(int ref_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { REF_ID = ref_id };
                return connection.Query<dynamic>("GetDonorInstructionsbyid", parameters, commandType: CommandType.StoredProcedure);

            }
        }
        public IEnumerable<dynamic> GetCampaignsById(int ref_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { REF_ID = ref_id };
                return connection.Query<dynamic>("GetCampaignById", parameters, commandType: CommandType.StoredProcedure);

            }
        }
        public IEnumerable<dynamic> GetDonationReceiveMasterByReceiveID(int ref_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { ReceiveID = ref_id };
                return connection.Query<dynamic>("GetDonationReceiveDetailsById", parameters, commandType: CommandType.StoredProcedure);

            }
        }
        public dynamic GetDataByDonorID(string donorid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new { DonorID = donorid };
                return connection.QueryFirstOrDefault<dynamic>("GetDonationReceiveDataByDonorID", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public string GetDonationReceiveMasterJsonById(int ref_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { ReceiveID = ref_id };
                var result = connection.Query<dynamic>("GetDonationReceiveDetailsById", parameters, commandType: CommandType.StoredProcedure);
                return JsonConvert.SerializeObject(result);
            }
        }

        public string GetMobileListJsonById(int ref_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { REF_NO = ref_id };
                var result = connection.Query<dynamic>("GetDonationReceiveMultiMobilebyID", parameters, commandType: CommandType.StoredProcedure);
                return JsonConvert.SerializeObject(result);
            }
        }

        public string GetIdentityListJsonById(int ref_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { REF_NO = ref_id };
                var result = connection.Query<dynamic>("GetDonationReceiveMultiIdentitybyID", parameters, commandType: CommandType.StoredProcedure);
                return JsonConvert.SerializeObject(result);
            }
        }

        public string GetBankDetailsListJsonById(int ref_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { REF_NO = ref_id };
                var result = connection.Query<dynamic>("GetDonationReceiveMultiBankById", parameters, commandType: CommandType.StoredProcedure);
                return JsonConvert.SerializeObject(result);
            }
        }

        public string GetReceiptsDetailsListJsonById(int ref_id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@REF_NO", ref_id, DbType.Int32);
                    var result = connection.Query<dynamic>("GetDonationReceiveMultiHeadbyId", parameters, commandType: CommandType.StoredProcedure);

                    return JsonConvert.SerializeObject(result);
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Exception: {ex.Message}");
                throw;  // Rethrow the exception or handle it appropriately
            }
        }

        public string GetDonorInstructionsListJsonById(int ref_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { REF_NO = ref_id };
                var result = connection.Query<dynamic>("GetDonationReceiveMultiDonorInstrucByid", parameters, commandType: CommandType.StoredProcedure);
                return JsonConvert.SerializeObject(result);
            }
        }

        public string GetAnnounceListJsonById(int ref_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { REF_NO = ref_id };
                var result = connection.Query<dynamic>("GetDonationReceiveMultiAnnunceDueByid", parameters, commandType: CommandType.StoredProcedure);
                return JsonConvert.SerializeObject(result);
            }
        }

        public string GetLocationDetailsByPinCodeJson(string pinCode)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new { InputPincode = pinCode };
                var result = connection.QueryFirstOrDefault<dynamic>("GetLocationDetailsByPincode", parameters, commandType: CommandType.StoredProcedure);

                // Assuming you want to return JSON
                return JsonConvert.SerializeObject(result);
            }
        }

        public async Task<IEnumerable<dynamic>> SearchDonationReceiveDataBYPara(List<dynamic> modelItems)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();

                foreach (var model in modelItems)
                {
                    parameters.AddDynamicParams(new
                    {
                        ReceiveID = model.receiveID,
                        PaymentModeName = model.paymentModeName,
                        TotalAmount = model.totalAmount,
                        CityName = model.cityName,
                        CityID = model.cityID,
                        StateName = model.stateName,
                        StateID = model.stateID,
                        MaterialID = model.saterialID,
                        ProvNo = model.provNo,
                        ReceiveDate = model.receiveDate,
                        FullName = model.fullName,
                        PaymentModeID = model.paymentModeID,
                        IfDetailsNotComplete = model.ifDetailsNotComplete
                    });
                }

                // Execute stored procedure with parameters
                return await connection.QueryAsync<dynamic>("SearchDonationReceiveDataBYPara", parameters, commandType: CommandType.StoredProcedure);
            }
        }


    }
}