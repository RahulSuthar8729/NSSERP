using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSSERPAPI.Db_functions_for_Gangotri;
using System.Data.SqlClient;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using Newtonsoft.Json.Linq;
using System.Data;
using NSSERPAPI.Models.NationalGangotri;

namespace NSSERPAPI.Controllers.NationalGangotri
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BackOfficeWorkController : ControllerBase
    {
        private readonly Db_functions _dbFunctions;
        private readonly string _connectionString;
        public BackOfficeWorkController(Db_functions dbFunctions, IConfiguration configuration)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
            _connectionString = configuration.GetConnectionString("ConStr");
        }


        [HttpGet]
        public IActionResult ViewDetails(int id)
        {
            var details = _dbFunctions.GetDonationReceiveMasterByReceiveID(id);

            dynamic firstDetail;

            if (details != null && details.Any())
            {
                firstDetail = details.First();
            }
            else
            {
                firstDetail = new ExpandoObject();
            }
            firstDetail.PersonDetails = _dbFunctions.GEtPersonDetails();
            firstDetail.EmployeeDetils = _dbFunctions.GetEmployeeDetils();
            firstDetail.ordertypelist = _dbFunctions.GetOrderTypes();
            firstDetail.bankmasterlist = _dbFunctions.GetAllBankMasters();
            firstDetail.depositBankmaster = _dbFunctions.GetDepositBankMaster();
            firstDetail.paymentModeList = _dbFunctions.GetPaymentModes();
            firstDetail.currenciesList = _dbFunctions.GetCurrencyListWithCountry();
            firstDetail.bankmasterlist = _dbFunctions.GetAllBankMasters();
            firstDetail.BankDetailsListJson = _dbFunctions.GetBankDetailsListJsonById(id);
            if (details == null)
            {
                return NotFound();
            }

            return Ok(firstDetail);
        }

        [HttpPost]
        public IActionResult InsertData([FromBody] BackOfficeModel model)
        {
            string DepositeList = HttpContext.Request.Headers["DepositeList"];
            List<DepositeDetailsModel> DepostiteDetailsList = string.IsNullOrEmpty(DepositeList) ? new List<DepositeDetailsModel>() : JsonConvert.DeserializeObject<List<DepositeDetailsModel>>(DepositeList);

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Start a transaction
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            if (DepostiteDetailsList != null)
                            {
                                foreach (var depositedetils in DepostiteDetailsList)
                                {
                                    var DepositesParams = new DynamicParameters();
                                    DepositesParams.Add("@REF_NO", model.ReceiveID);
                                    DepositesParams.Add("@REF_NO_BANK", depositedetils.refNo);
                                    DepositesParams.Add("@DepositeMode", depositedetils.mode);
                                    DepositesParams.Add("@DepositBank", depositedetils.DepositeBank);
                                    DepositesParams.Add("@DepositeDate", depositedetils.date);
                                    DepositesParams.Add("@CurrencyCode", depositedetils.currencyCode);
                                    DepositesParams.Add("@DepositeAmount", depositedetils.amount);
                                    DepositesParams.Add("@BankID", depositedetils.bankID);
                                    DepositesParams.Add("@TransactionID", depositedetils.TrasactionID);
                                    DepositesParams.Add("@DocPayInSlip", depositedetils.TempDoc);
                                    DepositesParams.Add("@CreatedBy", model.UserName);

                                    connection.Execute("InsertDonationReceiveBORTMultiDeposite", DepositesParams, transaction, commandType: CommandType.StoredProcedure);
                                }
                            }


                            var updateParams = new DynamicParameters();
                            updateParams.Add("@ReceiveID", model.ReceiveID);
                            updateParams.Add("@Status", "Addresing");
                            updateParams.Add("@DocPayInSlip", model.Doc3);
                            updateParams.Add("@ModifiedBy", model.UserName);
                            connection.Execute("UpdateDonationReceiveMasterAtBORT", updateParams, transaction, commandType: CommandType.StoredProcedure);


                            var movementParams = new DynamicParameters();
                            movementParams.Add("@ReceiveID", model.ReceiveID);
                            movementParams.Add("@MovementFrom", "2");
                            movementParams.Add("@UserID", model.UserID);
                            movementParams.Add("@UserName", model.UserName);
                            connection.Execute("InsertDmsMovement", movementParams, transaction, commandType: CommandType.StoredProcedure);

                            transaction.Commit();

                        }

                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
            dynamic firstDetail;
            firstDetail = new ExpandoObject();


            firstDetail.paymentModeList = _dbFunctions.GetPaymentModes();
            firstDetail.currenciesList = _dbFunctions.GetCurrencyListWithCountry();
            firstDetail.masterDetails = _dbFunctions.GetDonationReciveDetails();
            firstDetail.CityMasterList = _dbFunctions.GetActiveCities();
            firstDetail.paymentModes = _dbFunctions.GetPaymentModes();
            firstDetail.statelist = _dbFunctions.GetStates();
            firstDetail.msg = "Receive ID: " + model.ReceiveID + " has been verified successfully at the back office.";
            return Ok(firstDetail);
        }


    }
}
