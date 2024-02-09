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
    public class SankalpSiddhiController : Controller
    {
        private readonly Db_functions _dbFunctions;
        private readonly string _connectionString;
        public SankalpSiddhiController(Db_functions dbFunctions, IConfiguration configuration)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpGet]
        public IActionResult GetDataOnPageLoad()
        {
            var result = _dbFunctions.GetDonationReciveDetails();
            dynamic firstDetail;
            firstDetail = new ExpandoObject();
            firstDetail.masterDetails = result;            
            firstDetail.CityMasterList = _dbFunctions.GetActiveCities();
            firstDetail.paymentModes = _dbFunctions.GetPaymentModes();
            firstDetail.statelist = _dbFunctions.GetStates();
            firstDetail.DepositBankList = _dbFunctions.GetDepositBankMaster();
            firstDetail.BankStatementsList = _dbFunctions.GetBankStatement();
            return Ok(firstDetail);
        }

        [HttpGet]
        public IActionResult GetDepositDetails(int refno)
        {
            var result = _dbFunctions.GetBORTDetailsListJsonById(refno);         
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetBankStatement()
        {
            var result = _dbFunctions.GetBankStatement();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult InsertData([FromBody] SankalpShidhiDepositeDetails model)
        {
            int ReceiveID=0;
            int bankrefno = 0;
             string DepositeList = HttpContext.Request.Headers["DepositeDetailsListJson"];
            List<SankalpShidhiDepositeDetails> DepostiteDetailsList = string.IsNullOrEmpty(DepositeList) ? new List<SankalpShidhiDepositeDetails>() : JsonConvert.DeserializeObject<List<SankalpShidhiDepositeDetails>>(DepositeList);
            if (DepostiteDetailsList.Count > 0)
            {                
                ReceiveID =Convert.ToInt32(DepostiteDetailsList[0].receiveid);                
            }

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
                                connection.Execute("[DeleteDonationReceiveBORTMultiDeposite]", new { REF_NO=ReceiveID }, transaction, commandType: CommandType.StoredProcedure);


                                foreach (var depositedetils in DepostiteDetailsList)
                                {
                                    var DepositesParams = new DynamicParameters();
                                    DepositesParams.Add("@REF_NO", ReceiveID);
                                    
                                    if (depositedetils.refNo == "null")
                                    {
                                        DepositesParams.Add("@REF_NO_BANK", bankrefno);
                                    }
                                    else
                                    {
                                        DepositesParams.Add("@REF_NO_BANK", depositedetils.refNo);
                                    }
                                    DepositesParams.Add("@DepositeMode", depositedetils.mode);
                                    DepositesParams.Add("@DepositBank", depositedetils.DepositeBank);
                                    DepositesParams.Add("@DepositeDate", depositedetils.date);
                                    DepositesParams.Add("@CurrencyCode", depositedetils.currencyCode);
                                    DepositesParams.Add("@DepositeAmount", depositedetils.amount);
                                    DepositesParams.Add("@BankID", depositedetils.bankID);
                                    DepositesParams.Add("@TransactionID", depositedetils.TransactionID);
                                    DepositesParams.Add("@DocPayInSlip", depositedetils.TempDoc);
                                    DepositesParams.Add("@CreatedBy",model.UserName);

                                    connection.Execute("InsertDonationReceiveBORTMultiDeposite", DepositesParams, transaction, commandType: CommandType.StoredProcedure);
                                }
                            }


                            var updateParams = new DynamicParameters();
                            updateParams.Add("@ReceiveID", ReceiveID);
                            updateParams.Add("@Status", "SankalpShidhi");                         
                            updateParams.Add("@ModifiedBy", model.UserName);
                            connection.Execute("[UpdateDonationReceiveMasterAtSankalpShiddhi]", updateParams, transaction, commandType: CommandType.StoredProcedure);


                            var movementParams = new DynamicParameters();
                            movementParams.Add("@ReceiveID",ReceiveID);
                            movementParams.Add("@MovementFrom", "Sankalp");
                            movementParams.Add("@UserID", model.UserID);
                            movementParams.Add("@UserName", model.UserName);
                            connection.Execute("InsertDmsMovement", movementParams, transaction, commandType: CommandType.StoredProcedure);

                            transaction.Commit();

                        }

                        catch (Exception)
                        {
                            // If an exception occurs, roll back the transaction
                            transaction.Rollback();

                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }           
            string msg = "Receive ID: " + ReceiveID + " has been verified successfully at the Sankalp Siddhi.";
            return Ok(msg);
        }

        [HttpPost]
        public IActionResult SearchBankStatementByPara([FromBody] BankStatement model)        {

            var result = _dbFunctions.SearchBankStatementWithPara(model);        
            return Ok(result);

        }

    }
}
