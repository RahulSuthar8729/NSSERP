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
        private readonly DbEngineClass _dbEngine;

        public Db_functions(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _dbEngine = new DbEngineClass(configuration);
        }     

        public List<dynamic> GetCountries()
        {
            return _dbEngine.ExecuteStoredProcedure("GetCountries");
        }        
        public List<dynamic> GetDonationReciveDetails()
        {
            return _dbEngine.ExecuteStoredProcedure("GetDonationReceiveDetails");
        }

        public IEnumerable<dynamic> GetDonormasterDetailsList()
        {
            return _dbEngine.ExecuteStoredProcedure("[GetDonorDetails]");
        }

        public IEnumerable<dynamic> GetBankStatement(DateTime dateFrom, DateTime dateTo)
        {
            var parameters = new { DateFrom = dateFrom, DateTo = dateTo };
            return _dbEngine.ExecuteStoredProcedure("[GetBankStatementOnSankalpSiddhiByDate]", parameters);
        }
        public List<dynamic> GetActiveCities()
        {
            return _dbEngine.ExecuteStoredProcedure("GetActiveCities");
        }

        public List<dynamic> GetReceiptBookType()
        {
            return _dbEngine.ExecuteStoredProcedure("GetReceiptBookType");
        }

        public List<dynamic> GetPaymentModes()
        {
            return _dbEngine.ExecuteStoredProcedure("GetPaymentModes");
        }
        public List<dynamic> GetStates()
        {
            return _dbEngine.ExecuteStoredProcedure("GetStates");
        }
        public IEnumerable<dynamic> GetStatesByCountry(int countryId)
        {
            var parameters = new { CountryID = countryId, DataFlag = "" };
            return _dbEngine.ExecuteStoredProcedure("GetStatesByCountry", parameters);
        }

        public IEnumerable<dynamic> GetCity()
        {
            return _dbEngine.ExecuteStoredProcedure("[GetCity]");
        }

        public IEnumerable<dynamic> GetBankByDataFlag(string dataFlag)
        {
            var parameters = new { DataFlag = dataFlag };
            return _dbEngine.ExecuteStoredProcedure("GetBankMasterByDataFlag", parameters);
        }

        public IEnumerable<dynamic> GetDistrictsByState(int stateId)
        {
            var parameters = new { StateID = stateId };
            return _dbEngine.ExecuteStoredProcedure("GetDistrictsByState", parameters);
        }
        public IEnumerable<dynamic> GetCitiesByDistrictID(int districtId)
        {
            var parameters = new { DistrictID = districtId };
            return _dbEngine.ExecuteStoredProcedure("GetCitiesByDistrictID", parameters);
        }

        public IEnumerable<dynamic> GetCurrencyListWithCountry()
        {
            return _dbEngine.ExecuteStoredProcedure("GetCurrencyListWithCountry");
        }

        public dynamic GetLocationDetailsByPinCode(string pinCode)
        {
            var parameters = new { PinCode = pinCode };
            return _dbEngine.ExecuteStoredProcedure("GetLocationDetailsByPincode", parameters);
        }

        public IEnumerable<dynamic> GetAllBankMasters()
        {
            return _dbEngine.ExecuteStoredProcedure("GetAllBankMasters");
        }

        public IEnumerable<dynamic> GetOrderTypes()
        {
            return _dbEngine.ExecuteStoredProcedure("GetOrderTypes");
        }

        public IEnumerable<dynamic> GetEmployeeDetils()
        {
            return _dbEngine.ExecuteStoredProcedure("GetEmployeeDetails");
        }

        public IEnumerable<dynamic> GEtPersonDetails()
        {
            return _dbEngine.ExecuteStoredProcedure("GetPersonDetails");
        }

        public IEnumerable<dynamic> CurrencyList()
        {
            return _dbEngine.ExecuteStoredProcedure("[GetCurrencyForDonationReceiveMaster]");
        }

        public IEnumerable<dynamic> GetDepositBankMaster()
        {
            return _dbEngine.ExecuteStoredProcedure("GetDepositeBankMaster");
        }

        public IEnumerable<dynamic> GetProvisionalReceiptbyId(int id)
        {
            var parameters = new { ReceiveID = id };
            return _dbEngine.ExecuteStoredProcedure("[GetProvisionalReceiptbyId]", parameters);
        }

        public IEnumerable<dynamic> getHeads()
        {
            return _dbEngine.ExecuteStoredProcedure("GetHeads");
        }

        public IEnumerable<dynamic> getReceiveHeads()
        {
            return _dbEngine.ExecuteStoredProcedure("GetReceiveHeads");
        }

        public IEnumerable<dynamic> GetEvents()
        {
            return _dbEngine.ExecuteStoredProcedure("GetEvents");
        }

        public IEnumerable<dynamic> GetSubHeadByHead(int HeadID, string DataFlag, int CurrencyID)
        {
            var parameters = new { PurposeId = HeadID, DataFlag = DataFlag, CurrencyId = CurrencyID };
            return _dbEngine.ExecuteStoredProcedure("[GetSubHeadByHeadid]", parameters);
        }

        public IEnumerable<dynamic> GetOperationAmountByQty(int Qty, string DataFlag, int CurrencyID)
        {
            var parameters = new { Qty = Qty, DataFlag = DataFlag, CurrencyID = CurrencyID };
            return _dbEngine.ExecuteStoredProcedure("[GetOperationAmountBYQty]", parameters);
        }

        public IEnumerable<dynamic> GetQtyAmtBySubHead(int yojnaid, string DataFlag, int CurrencyID)
        {
            var parameters = new { YojnaID = yojnaid, DataFlag = DataFlag, CurrencyID = CurrencyID };
            return _dbEngine.ExecuteStoredProcedure("GetQtyAmtBySubHead", parameters);
        }

        public IEnumerable<dynamic> GetPersonNameByProvisonal(int ReceiptNo, string TP, string DataFlag)
        {
            var parameters = new { ReceiptNo = ReceiptNo, TP = TP, DataFlag = DataFlag };
            return _dbEngine.ExecuteStoredProcedure("[GetPersonDetailsByProvisioanlNo]", parameters);
        }

        public IEnumerable<dynamic> getSubHeads()
        {
            return _dbEngine.ExecuteStoredProcedure("GetSubHeads");
        }

        public IEnumerable<dynamic> GetCriticalPurpose()
        {
            return _dbEngine.ExecuteStoredProcedure("[GetCriticalPupose]");
        }

        public IEnumerable<dynamic> GetallCampaigns()
        {
            return _dbEngine.ExecuteStoredProcedure("GetAllCampaigns");
        }

        public IEnumerable<dynamic> GetDonorINstructionsMaster()
        {
            return _dbEngine.ExecuteStoredProcedure("GetDonorInstructions");
        }

        public IEnumerable<dynamic> GetDonorInstructionsbyid(int ref_id)
        {
            var parameters = new { REF_ID = ref_id };
            return _dbEngine.ExecuteStoredProcedure("GetDonorInstructionsbyid", parameters);
        }

        public IEnumerable<dynamic> GetCampaignsById(int ref_id)
        {
            var parameters = new { REF_ID = ref_id };
            return _dbEngine.ExecuteStoredProcedure("GetCampaignById", parameters);
        }

        public IEnumerable<dynamic> GetDonationReceiveMasterByReceiveID(int ref_id)
        {
            var parameters = new { ReceiveID = ref_id };
            return _dbEngine.ExecuteStoredProcedure("GetDonationReceiveDetailsById", parameters);
        }

        public dynamic GetDataByDonorID(string donorid)
        {
            var parameters = new { DonorID = donorid };
            return _dbEngine.ExecuteStoredProcedure("GetDonationReceiveDataByDonorID", parameters);
        }

        public dynamic SearchDonorDetails(SearchDonorDetails model)
        {
            var parameters = new { SearchType = model.SearchType, SearchData = model.searchData };
            return _dbEngine.ExecuteStoredProcedure("[SearchDonorMasterDataByPara]", parameters);
        }

        public dynamic SearchDonorIdentityDetails(SearchDonorDetails model)
        {
            var parameters = new { SearchType = model.SearchType, SearchData = model.searchData };
            return _dbEngine.ExecuteStoredProcedure("[SearchDonorIdentityDetailsBYPara]", parameters);
        }

        public dynamic SearchDonorContactDetailsDetails(SearchDonorDetails model)
        {
            var parameters = new { SearchType = model.SearchType, SearchData = model.searchData };
            return _dbEngine.ExecuteStoredProcedure("[SearchDonorContactDetailsBYPara]", parameters);
        }

        public string GetDonationReceiveMasterJsonById(int ref_id)
        {
            var parameters = new { ReceiveID = ref_id };
            var result = _dbEngine.ExecuteStoredProcedure("GetDonationReceiveDetailsById", parameters);
            return JsonConvert.SerializeObject(result);
        }

        public string GetMovementMasterListJsonById(int ref_id)
        {
            var parameters = new { ReceiveID = ref_id };
            var result = _dbEngine.ExecuteStoredProcedure("[GetDmsMovementMasterByReceiveID]", parameters);
            return JsonConvert.SerializeObject(result);
        }

        public string GetMobileListJsonById(int ref_id)
        {
            var parameters = new { REF_NO = ref_id };
            var result = _dbEngine.ExecuteStoredProcedure("GetDonationReceiveMultiMobilebyID", parameters);
            return JsonConvert.SerializeObject(result);
        }

        public string GetIdentityListJsonById(int ref_id)
        {
            var parameters = new { REF_NO = ref_id };
            var result = _dbEngine.ExecuteStoredProcedure("GetDonationReceiveMultiIdentitybyID", parameters);
            return JsonConvert.SerializeObject(result);
        }
        public string GetMobileListJsonByDonorID(int ref_id)
        {
            var parameters = new { DonorID = ref_id };
            var result = _dbEngine.ExecuteStoredProcedure("GetDonationReceiveMultiMobilebyID", parameters);
            return JsonConvert.SerializeObject(result);
        }

        public string GetIdentityListJsonByDonorID(int ref_id)
        {
            var parameters = new { DonorID = ref_id };
            var result = _dbEngine.ExecuteStoredProcedure("GetDonationReceiveMultiIdentitybyID", parameters);
            return JsonConvert.SerializeObject(result);
        }

        public string GetBankDetailsListJsonById(int ref_id)
        {
            var parameters = new { REF_NO = ref_id };
            var result = _dbEngine.ExecuteStoredProcedure("GetDonationReceiveMultiBankById", parameters);
            return JsonConvert.SerializeObject(result);
        }

        public string GetReceiptsDetailsListJsonById(int ref_id)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@REF_NO", ref_id, DbType.Int32);
                var result = _dbEngine.ExecuteStoredProcedure("GetDonationReceiveMultiHeadbyId", parameters);
                return JsonConvert.SerializeObject(result);
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
            var parameters = new { REF_NO = ref_id };
            var result = _dbEngine.ExecuteStoredProcedure("GetDonationReceiveMultiDonorInstrucByid", parameters);
            return JsonConvert.SerializeObject(result);
        }

        public string GetAnnounceListJsonById(int ref_id)
        {
            var parameters = new { REF_NO = ref_id };
            var result = _dbEngine.ExecuteStoredProcedure("GetDonationReceiveMultiAnnunceDueByid", parameters);
            return JsonConvert.SerializeObject(result);
        }

        public string GetLocationDetailsByPinCodeJson(string pinCode)
        {
            var parameters = new { PinCode = pinCode };
            var result = _dbEngine.ExecuteStoredProcedure("GetLocationDetailsByPincode", parameters);
            return JsonConvert.SerializeObject(result);
        }

        public string GetBORTDetailsListJsonById(int ref_id)
        {
            var parameters = new { REF_NO = ref_id };
            var result = _dbEngine.ExecuteStoredProcedure("GetBORTDetsilsByid", parameters);
            return JsonConvert.SerializeObject(result);
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


        #region DonorMaster
        public IEnumerable<dynamic> GetDonorTypes()
        {           
            return _dbEngine.ExecuteStoredProcedure("GetDonorTypes");          
            
        }

        public IEnumerable<dynamic> GetDonorBussinussType(string DataFlag)
        {
            return _dbEngine.ExecuteStoredProcedure("GetDonorBusinessDetails", new { DataFlag=DataFlag});
        }

        public IEnumerable<dynamic>GetDonorDataByID(int DonorID, string DataFlag)
        {
            var parameters = new { Ngcode = DonorID, DataFlag = DataFlag };
            return _dbEngine.ExecuteStoredProcedure("[GetDonorDataById]", parameters);
        }

        #endregion


        #region Country Master
        public IEnumerable<dynamic> GetCountryList()
        {            
            return _dbEngine.ExecuteStoredProcedure("GetCountryDetails");
        }
        public IEnumerable<dynamic> GetCountryByID(int id)
        {
            var parameters = new { CountryId = id};
            return _dbEngine.ExecuteStoredProcedure("[GetCountryById]", parameters);
        }
        #endregion


        #region StateMaster
        public IEnumerable<dynamic> GetStateDetailList(string DataFlag)
        {
            return _dbEngine.ExecuteStoredProcedure("GetStateDetails", new {DataFlag=DataFlag });
        }

        public IEnumerable<dynamic> GetStateById(int id,string DataFlag)
        {
            var parameters = new { StateId = id ,DataFlag=DataFlag};
            return _dbEngine.ExecuteStoredProcedure("[GetStateById]", parameters);
        }
        #endregion


        #region District Master
        public IEnumerable<dynamic> GetDistrictDetails(string DataFlag)
        {
            return _dbEngine.ExecuteStoredProcedure("[GetDistrictDetails]", new { DataFlag = DataFlag });
        }
        public IEnumerable<dynamic> GetDistrictById(int id, string DataFlag)
        {
            var parameters = new { DistrictId = id, DataFlag = DataFlag };
            return _dbEngine.ExecuteStoredProcedure("[GetDistrictById]", parameters);
        }

        #endregion

        #region City Master
        public IEnumerable<dynamic> GetCityDetsils(string DataFlag)
        {
            return _dbEngine.ExecuteStoredProcedure("[GetCityDetails]", new { DataFlag=DataFlag});
        }
        public IEnumerable<dynamic> GetCityById(int id, string DataFlag)
        {
            var parameters = new { CityId = id, DataFlag = DataFlag };
            return _dbEngine.ExecuteStoredProcedure("[GetCityById]", parameters);
        }

        #endregion



        #region Purpose Master
        public IEnumerable<dynamic> GetPurposeDetails (string DataFlag)
        {
            return _dbEngine.ExecuteStoredProcedure("[GetPurposeDetails]", new { DataFlag = DataFlag });
        }    
        public IEnumerable<dynamic> GetPurposeById(int id,string DataFlag)
        {
            return _dbEngine.ExecuteStoredProcedure("[GetPurposeById]", new { PurposeId=id, DataFlag = DataFlag });
        }

        #endregion

        #region Purpose Yojana Master

        public IEnumerable<dynamic> GetPurposeYojanaDetails(string DataFlag)
        {
            return _dbEngine.ExecuteStoredProcedure("[GetPurposeYojnaDetails]", new { DataFlag = DataFlag });
        }
        public IEnumerable<dynamic> GetPurposeYojanaById(int id, string DataFlag)
        {
            return _dbEngine.ExecuteStoredProcedure("[GetPurposeYojnaById]", new { YojnaId = id, DataFlag = DataFlag });
        }  
        public IEnumerable<dynamic> GetCurrencyDetails()
        {
            return _dbEngine.ExecuteStoredProcedure("GetCurrencyDetails");
        }

        #endregion
    }
}