using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NSSERPAPI.Models.NationalGangotri;
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
        public IEnumerable<dynamic> GetBankStatement(DateTime datefrom,DateTime dateTo)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query("[GetBankStatementOnSankalpSiddhiByDate]", new {DateFrom=datefrom,DateTo=dateTo}, commandType: CommandType.StoredProcedure);
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
        public List<dynamic> GetPaymentModes()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = connection.Query("GetPaymentModes", commandType: CommandType.StoredProcedure);
                return result.AsList();
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
        public IEnumerable<dynamic> GetBankByDataFlag(string DataFlag)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
              
                return connection.Query<dynamic>("GetBankMasterByDataFlag", new { DataFlag = DataFlag }, commandType: CommandType.StoredProcedure);
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
        public IEnumerable<dynamic> GetORderTypes()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<dynamic>("GetOrderTypes", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<dynamic> GetEmployeeDetils()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<dynamic>("GetEmployeeDetails", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<dynamic> GEtPersonDetails ()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<dynamic>("GetPersonDetails", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<dynamic> CurrencyList()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<dynamic>("[GetCurrencyForDonationReceiveMaster]", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<dynamic> GetDepositBankMaster()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<dynamic>("GetDepositeBankMaster", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<dynamic> GetProvisionalReceiptbyId(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<dynamic>("[GetProvisionalReceiptbyId]", new { ReceiveID = id }, commandType: CommandType.StoredProcedure);
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
        public IEnumerable<dynamic> GetSubHeadByHead(int HeadID, string DataFlag, int CurrencyID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { PurposeId = HeadID, DataFlag = DataFlag, CurrencyId = CurrencyID };
                return connection.Query<dynamic>("[GetSubHeadByHeadid]", parameters, commandType: CommandType.StoredProcedure);

            }
        }
        public IEnumerable<dynamic> GetQtyAmtBySubHead(int yojnaid, string DataFlag, int CurrencyID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { YojnaID = yojnaid, DataFlag = DataFlag, CurrencyID = CurrencyID };
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
        public dynamic SearchDonorDetails(SearchDonorDetails model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new { SearchType=model.SearchType,SearchData=model.searchData };
                return connection.QueryFirstOrDefault<dynamic>("[SearchDonorMasterDataByPara]", parameters, commandType: CommandType.StoredProcedure);
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
        public string GetMovementMasterListJsonById(int ref_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { ReceiveID = ref_id };
                var result = connection.Query<dynamic>("[GetDmsMovementMasterByReceiveID]", parameters, commandType: CommandType.StoredProcedure);
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

        public string GetBORTDetailsListJsonById(int ref_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { REF_NO = ref_id };
                var result = connection.Query<dynamic>("GetBORTDetsilsByid", parameters, commandType: CommandType.StoredProcedure);
                return JsonConvert.SerializeObject(result);
            }
        }

        public IEnumerable<DonationReceiveDetailsWithParaModel> SearchDonationDetailsByPara(DonationReceiveDetailsWithParaModel model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.AddDynamicParams(new
                {
                    model.ReceiveID,
                    model.PaymentModeName,
                    model.TotalAmount,
                    model.CityName,
                    model.CityID,
                    model.StateName,
                    model.StateID,
                    model.MaterialID,
                    model.ProvNo,
                    model.ReceiveDate,
                    model.FullName,
                    model.PaymentModeID,
                    model.IfDetailsNotComplete
                });

                var result = connection.Query<DonationReceiveDetailsWithParaModel>("SearchDonationReceiveDataBYPara", parameters, commandType: CommandType.StoredProcedure);

                return result;
            }
        }
        public IEnumerable<BankStatement> SearchBankStatementWithPara(BankStatement model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.AddDynamicParams(new
                {
                    Bank_Code = model.BANK_Code,
                    DR = model.DR,
                    CR = model.CR,
                    BALANCE = model.BALANCE,
                    ReceiveAmt = model.ReceiveAmt,
                    Curr_Rate = model.Curr_Rate,
                    Bank_Name = model.Bank_Name,
                    Particular = model.Particular,
                    ChqNo = model.ChqNo,
                    ReceiveId = model.ReceiveID,
                    Branch = model.Branch,
                    Data_Flag = model.DataFlag,
                    MobileNo = model.MobileNo,
                    DateFrom = model.DateFrom,
                    DateTo = model.DateTo
                });

                var result = connection.Query<BankStatement>("[GetBankStatementsWithPara]", parameters, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

    }
}