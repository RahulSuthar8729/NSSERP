using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using NSSERP.Areas.Masters.Models;
using System.Collections.Generic;
using NSSERP.Areas.NationalGangotri.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;



namespace NSSERP.DbFunctions
{
    public class DbClass
    {
        private readonly string _connectionString;

        // Constructor to set the connection string
        public DbClass(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public List<Countrys> GetCountries()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Execute stored procedure to get countries
                return connection.Query<Countrys>("GetCountries", commandType: CommandType.StoredProcedure).AsList();
            }
        }

        public List<StateMaster> GetStates()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Execute stored procedure to get states
                return connection.Query<StateMaster>("GetStates", commandType: CommandType.StoredProcedure).AsList();
            }
        }

        public List<CityMaster> GetActiveCities()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Execute stored procedure to get active cities
                return connection.Query<CityMaster>("GetActiveCities", commandType: CommandType.StoredProcedure).AsList();
            }
        }

        public IEnumerable<StateMaster> GetStatesByCountry(int countryId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Use Dapper to call the stored procedure
                return connection.Query<StateMaster>("GetStatesByCountry", new { CountryID = countryId }, commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<DistrictMaster> GetDistrictsByState(int stateId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<DistrictMaster>("GetDistrictsByState", new { StateID = stateId }, commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<CityMaster> GetCitiesByDistrictID(int districtId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<CityMaster>("GetCitiesByDistrictID", new { DistrictID = districtId }, commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<PaymentModeMaster> GetPaymentModes()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<PaymentModeMaster>("GetPaymentModes", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<CurrencyMaster> GetCurrencyListWithCountry()
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                var currencies = dbConnection.Query<CurrencyMaster, Countrys, CurrencyMaster>(
                    "GetCurrencyListWithCountry",
                    (currency, country) =>
                    {
                        currency.CountryMaster = country;
                        return currency;
                    },
                    splitOn: "CountryID",
                    commandType: CommandType.StoredProcedure
                ).ToList();

                return currencies;
            }
        }
        public PinCodeMaster GetLocationDetailsByPinCode(string pinCode)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new { InputPincode = pinCode };
                return connection.QueryFirstOrDefault<PinCodeMaster>("GetLocationDetailsByPincode", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<BankMaster> GetAllBankMasters()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<BankMaster>("GetAllBankMasters", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<DonationReceiveMaster> getHeads()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<DonationReceiveMaster>("GetHeads", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<DonationReceiveMaster> getReceiveHeads()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<DonationReceiveMaster>("GetReceiveHeads", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<DonationReceiveMaster> GetEvents()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<DonationReceiveMaster>("GetEvents", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<DonationReceiveMaster> GetHeadsbySubheadID(int subheadid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { SubHeadID = subheadid };
                return connection.Query<DonationReceiveMaster>("GetHeadsbySubHeadid", parameters, commandType: CommandType.StoredProcedure);

            }
        }
        public IEnumerable<DonationReceiveMaster> getSubHeads()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<DonationReceiveMaster>("GetSubHeads", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<CampaignMaster> GetallCampaigns()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<CampaignMaster>("GetAllCampaigns", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<DonorInstructionMaster> GetDonorINstructionsMaster()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<DonorInstructionMaster>("GetDonorInstructions", commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<DonorInstructionMaster> GetDonorInstructionsbyid(int ref_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { REF_ID = ref_id };
                return connection.Query<DonorInstructionMaster>("GetDonorInstructionsbyid", parameters, commandType: CommandType.StoredProcedure);

            }
        }
        public IEnumerable<CampaignMaster> GetCampaignsById(int ref_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { REF_ID = ref_id };
                return connection.Query<CampaignMaster>("GetCampaignById", parameters, commandType: CommandType.StoredProcedure);

            }
        }

        public IEnumerable<DonationReceiveMasterDetails> GetDonationReciveDetails()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<DonationReceiveMasterDetails>("GetDonationReceiveDetails", commandType: CommandType.StoredProcedure);
            }
        }
        
        public IEnumerable<DonationReceiveMaster> GetDonationReceiveMasterByReceiveID(int ref_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { ReceiveID = ref_id };
                return connection.Query<DonationReceiveMaster>("GetDonationReceiveDetailsById", parameters, commandType: CommandType.StoredProcedure);

            }
        }


        public DonationReceiveMaster GetDataByDonorID(string donorid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new { DonorID = donorid };
                return connection.QueryFirstOrDefault<DonationReceiveMaster>("GetDonationReceiveDataByDonorID", parameters, commandType: CommandType.StoredProcedure);
            }
        }


        public string GetDonationReceiveMasterJsonById(int ref_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { ReceiveID = ref_id };
                var result = connection.Query<DonationReceiveMaster>("GetDonationReceiveDetailsById", parameters, commandType: CommandType.StoredProcedure);
                return JsonConvert.SerializeObject(result);
            }
        }



        public string GetMobileListJsonById(int ref_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { REF_NO = ref_id };
                var result = connection.Query<MobileDetails>("GetDonationReceiveMultiMobilebyID", parameters, commandType: CommandType.StoredProcedure);
                return JsonConvert.SerializeObject(result);
            }
        }

        public string GetIdentityListJsonById(int ref_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { REF_NO = ref_id };
                var result = connection.Query<IdentityDetails>("GetDonationReceiveMultiIdentitybyID", parameters, commandType: CommandType.StoredProcedure);
                return JsonConvert.SerializeObject(result);
            }
        }

        public string GetBankDetailsListJsonById(int ref_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { REF_NO = ref_id };
                var result = connection.Query<BankDetails>("GetDonationReceiveMultiBankById", parameters, commandType: CommandType.StoredProcedure);
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
                    var result = connection.Query<ReceiptList>("GetDonationReceiveMultiHeadbyId", parameters, commandType: CommandType.StoredProcedure);

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
                var result = connection.Query<DonorInstructionList>("GetDonationReceiveMultiDonorInstrucByid", parameters, commandType: CommandType.StoredProcedure);
                return JsonConvert.SerializeObject(result);
            }
        }

        public string GetAnnounceListJsonById(int ref_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { REF_NO = ref_id };
                var result = connection.Query<AnnounceDetails>("GetDonationReceiveMultiAnnunceDueByid", parameters, commandType: CommandType.StoredProcedure);
                return JsonConvert.SerializeObject(result);
            }
        }

        public string GetLocationDetailsByPinCodeJson(string pinCode)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new { InputPincode = pinCode };
                var result = connection.QueryFirstOrDefault<PinCodeMaster>("GetLocationDetailsByPincode", parameters, commandType: CommandType.StoredProcedure);

                // Assuming you want to return JSON
                return JsonConvert.SerializeObject(result);
            }
        }


    }
}
