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
    public class DonationReceiveMasterController : Controller
    {
        private readonly Db_functions _dbFunctions;
        private readonly string _connectionString;
        public DonationReceiveMasterController(Db_functions dbFunctions, IConfiguration configuration)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpGet]
        public IActionResult Home(int id)
        {
            var detailsJson = HttpContext.Items["DonationDetails"] as string;
            var details = detailsJson != null ? JsonConvert.DeserializeObject<List<dynamic>>(detailsJson) : null;

            dynamic firstDetail;

            if (details != null && details.Any())
            {
                firstDetail = details.First();
            }
            else
            {
                firstDetail = new ExpandoObject();
            }

            var receiveID = id;
            // Set additional properties for firstDetail
            firstDetail.CountryList = _dbFunctions.GetCountries();
            firstDetail.paymentModeList = _dbFunctions.GetPaymentModes();
            firstDetail.currenciesList = _dbFunctions.GetCurrencyListWithCountry();
            firstDetail.bankmasterlist = _dbFunctions.GetAllBankMasters();
            firstDetail.SubHeadList = _dbFunctions.getSubHeads();
            firstDetail.ReceiveHeadList = _dbFunctions.getReceiveHeads();
            firstDetail.ReceiveInEventList = _dbFunctions.GetEvents();
            firstDetail.campaignlist = _dbFunctions.GetallCampaigns();
            firstDetail.donorInstructionList = _dbFunctions.GetDonorINstructionsMaster();

            // Set JSON properties           
            firstDetail.MobileListJson = _dbFunctions.GetMobileListJsonById(receiveID);
            firstDetail.IdentityListJson = _dbFunctions.GetIdentityListJsonById(receiveID);
            firstDetail.BankDetailsListJson = _dbFunctions.GetBankDetailsListJsonById(receiveID);
            firstDetail.ReceiptsListJson = _dbFunctions.GetReceiptsDetailsListJsonById(receiveID);
            firstDetail.DonorInstructionsListJson = _dbFunctions.GetDonorInstructionsListJsonById(receiveID);
            firstDetail.AnnounceDetailsListJson = _dbFunctions.GetAnnounceListJsonById(receiveID);
            firstDetail.DepositeDetailsListJson = _dbFunctions.GetBORTDetailsListJsonById(receiveID);

            return Ok(firstDetail);
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

            // Set additional properties for firstDetail
            firstDetail.CountryList = _dbFunctions.GetCountries();
            firstDetail.paymentModeList = _dbFunctions.GetPaymentModes();
            firstDetail.currenciesList = _dbFunctions.GetCurrencyListWithCountry();
            firstDetail.bankmasterlist = _dbFunctions.GetAllBankMasters();
            firstDetail.HeadList = _dbFunctions.getHeads();
            // firstDetail.SubHeadList = _dbFunctions.getSubHeads();
            firstDetail.ReceiveHeadList = _dbFunctions.getReceiveHeads();
            firstDetail.ReceiveInEventList = _dbFunctions.GetEvents();
            firstDetail.campaignlist = _dbFunctions.GetallCampaigns();
            firstDetail.donorInstructionList = _dbFunctions.GetDonorINstructionsMaster();

            // Set JSON properties           
            firstDetail.MobileListJson = _dbFunctions.GetMobileListJsonById(id);
            firstDetail.IdentityListJson = _dbFunctions.GetIdentityListJsonById(id);
            firstDetail.BankDetailsListJson = _dbFunctions.GetBankDetailsListJsonById(id);
            firstDetail.ReceiptsListJson = _dbFunctions.GetReceiptsDetailsListJsonById(id);
            firstDetail.DonorInstructionsListJson = _dbFunctions.GetDonorInstructionsListJsonById(id);
            firstDetail.AnnounceDetailsListJson = _dbFunctions.GetAnnounceListJsonById(id);
            firstDetail.DepositeDetailsListJson = _dbFunctions.GetBORTDetailsListJsonById(id);


            if (details == null)
            {
                return NotFound();
            }

            return Ok(firstDetail);
        }

        [HttpGet]
        public IActionResult GetSubHeadByHead(string HeadID, string DataFlag)
        {
            try
            {
                var subheads = _dbFunctions.GetSubHeadByHead(Convert.ToInt32(HeadID), DataFlag);
                return Ok(subheads);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("An error occurred while retrieving location details.");
            }
        }

        [HttpGet]
        public IActionResult GetQtyAmtBySubHead(int YojnaID, string DataFlag)
        {
            try
            {
                var data = _dbFunctions.GetQtyAmtBySubHead(YojnaID, DataFlag);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest("Error retrieving states.");
            }
        }

        [HttpGet]
        public IActionResult GetPurposeHead()
        {
            try
            {
                var Head = _dbFunctions.getHeads();

                return Ok(Head);
            }
            catch (Exception ex)
            {
                return BadRequest("Error retrieving states.");
            }
        }

        [HttpGet]
        public IActionResult GetStatesByCountry(int countryId)
        {
            try
            {
                var states = _dbFunctions.GetStatesByCountry(countryId);

                return Ok(states);
            }
            catch (Exception ex)
            {
                return BadRequest("Error retrieving states.");
            }
        }

        [HttpGet]
        public IActionResult GetDistrictsByState(int stateId)
        {
            try
            {
                // Use your database connection logic here to retrieve districts based on the state ID
                var districts = _dbFunctions.GetDistrictsByState(stateId); // Implement this method in your DbFunctions class

                // Return districts as JSON
                return Ok(districts);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("An error occurred while retrieving districts.");
            }
        }

        [HttpGet]
        public IActionResult GetCitiesByDistrictID(int districtId)
        {
            try
            {
                var cities = _dbFunctions.GetCitiesByDistrictID(districtId);
                return Ok(cities);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("An error occurred while retrieving cities.");
            }
        }

        [HttpGet]
        public IActionResult GetDataByDonorID(string DonorID)
        {
            try
            {
                var Data = _dbFunctions.GetDataByDonorID(DonorID);
                if (Data.PinCode != null)
                {

                    var locationDetails = _dbFunctions.GetLocationDetailsByPinCodeJson(Convert.ToString(Data.PinCode));
                    var mobilelist = _dbFunctions.GetMobileListJsonById(Convert.ToInt32(Data.ReceiveID));
                    var identityList = _dbFunctions.GetIdentityListJsonById(Convert.ToInt32(Data.ReceiveID));

                    Data.pinCodeMasterList = locationDetails;
                    Data.MobileListJson = mobilelist;
                    Data.IdentityListJson = identityList;

                }

                return Ok(Data);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("An error occurred while retrieving location details.");
            }
        }


        [HttpGet]
        public IActionResult GetLocationDetailsByPinCode(string pincode)
        {
            try
            {
                var Location = _dbFunctions.GetLocationDetailsByPinCode(pincode);
                return Ok(Location);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("An error occurred while retrieving cities.");
            }
        }


        [HttpGet]
        public IActionResult GetProvisionalReceipt(string refid)
        {
            try
            {
                var data = _dbFunctions.GetProvisionalReceiptbyId(Convert.ToInt32(refid));
                return Ok(data);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("An error occurred while retrieving cities.");
            }
        }


        [HttpPost]
        public IActionResult InsertData([FromBody] DonationReceiveMaster model)
        {
            string mobileListJson = HttpContext.Request.Headers["MobileList"];
            string identityListJson = HttpContext.Request.Headers["IdentityList"];
            string bankDetailsListJson = HttpContext.Request.Headers["BankDetailsList"];
            string receiptDetailsListJson = HttpContext.Request.Headers["receiptdetailslist"];
            string announceDetailsListJson = HttpContext.Request.Headers["AnnounceDetsilsList"];
            string donorInstructionJsonList = HttpContext.Request.Headers["donorInstructionjsonList"];

            int maxReceiveID = 0;

            List<MobileDetails> MobileList = string.IsNullOrEmpty(mobileListJson) ? new List<MobileDetails>() : JsonConvert.DeserializeObject<List<MobileDetails>>(mobileListJson);

            List<IdentityDetails> IdentityList = string.IsNullOrEmpty(identityListJson) ? new List<IdentityDetails>() : JsonConvert.DeserializeObject<List<IdentityDetails>>(identityListJson);

            List<BankDetails> bankDetailslist = string.IsNullOrEmpty(bankDetailsListJson) ? new List<BankDetails>() : JsonConvert.DeserializeObject<List<BankDetails>>(bankDetailsListJson);

            List<ReceiptDetail> Receiptdetailslist = string.IsNullOrEmpty(receiptDetailsListJson) ? new List<ReceiptDetail>() : JsonConvert.DeserializeObject<List<ReceiptDetail>>(receiptDetailsListJson);

            List<AnnounceDetails> announcelist = string.IsNullOrEmpty(announceDetailsListJson) ? new List<AnnounceDetails>() : JsonConvert.DeserializeObject<List<AnnounceDetails>>(announceDetailsListJson);

            List<DonorInstructionList> donorInstructionLists = string.IsNullOrEmpty(donorInstructionJsonList) ? new List<DonorInstructionList>() : JsonConvert.DeserializeObject<List<DonorInstructionList>>(donorInstructionJsonList);


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
                            var parameters = new DynamicParameters();
                            parameters.Add("@AppStatus", "");
                            parameters.Add("@FinYear", model.FinYear);
                            parameters.Add("@ReceiveDate", model.ReceiveDate);
                            parameters.Add("@IsReceiveHeadDifferent", model.IsReceiveHeadDiffrent);
                            parameters.Add("@ReceiveHeadID", model.ReceiveHeadID);
                            parameters.Add("@ReceiveHeadName", model.ReceiveDepartment);
                            parameters.Add("@UserID", model.UserID);
                            parameters.Add("@UserName", model.UserName);
                            parameters.Add("@ReceiveInEventID", model.ID);
                            parameters.Add("@ReceiveInEvent", model.EventName);
                            parameters.Add("@InMemory", model.InMemory);
                            parameters.Add("@DonorID", model.DonorID);
                            parameters.Add("@NamePrefix", model.NamePrefix);
                            parameters.Add("@FullName", model.FullName);
                            parameters.Add("@PrefixToFullName", model.PrefixToFullName);
                            parameters.Add("@RelationToFullName", model.RelationToFullName);
                            parameters.Add("@DateOfBirth", model.DateOfBirth);
                            parameters.Add("@Company", model.Company);
                            parameters.Add("@FullAddress", model.FullAddress);
                            parameters.Add("@PinCode", model.PinCode);
                            parameters.Add("@CountryID", model.CountryId);
                            parameters.Add("@CountryName", model.CountryName);
                            parameters.Add("@StateID", model.StateID);
                            parameters.Add("@StateName", model.StateName);
                            parameters.Add("@DistrictID", model.DistrictID);
                            parameters.Add("@DistrictName", model.DistrictName);
                            parameters.Add("@CityID", model.CityID);
                            parameters.Add("@CityName", model.CityName);
                            parameters.Add("@IfUpdationInAddress", model.IfUpdationInAddress);
                            parameters.Add("@IsPermanentAddressDiff", model.IsPermanentAddressDiff);
                            parameters.Add("@IfDetailsNotComplete", model.IfDetailsNotComplete);
                            parameters.Add("@P_FullAddress", model.P_FullAddress);
                            parameters.Add("@P_PinCode", model.P_PinCode);
                            parameters.Add("@P_CountryID", model.P_CountryID);
                            parameters.Add("@P_CountryName", model.P_CountryName);
                            parameters.Add("@P_StateID", model.P_StateID);
                            parameters.Add("@P_StateName", model.P_StateName);
                            parameters.Add("@P_DistrictID", model.P_DistrictID);
                            parameters.Add("@P_DistrictName", model.P_DistrictName);
                            parameters.Add("@P_CityID", model.P_CityID);
                            parameters.Add("@P_CityName", model.P_CityName);
                            parameters.Add("@EmailID", model.EmailID);
                            parameters.Add("@StdCode", model.StdCode);
                            parameters.Add("@PhoneR", model.PhoneR);
                            parameters.Add("@ProvNo", model.ProvNo);
                            parameters.Add("@ProvDate", model.ProvDate);
                            parameters.Add("@DonPersonName", model.PersonName);
                            parameters.Add("@DonEventID", model.ID);
                            parameters.Add("@DonEventName", model.EventName);
                            parameters.Add("@PaymentModeID", model.PaymentModeID);
                            parameters.Add("@PaymentModeName", model.PaymentModeName);
                            parameters.Add("@CurrencyID", model.CurrencyID);
                            parameters.Add("@CurrencyCode", model.CurrencyCode);
                            parameters.Add("@Amount", model.TotalAmount);
                            parameters.Add("@MaterialDepositID", model.MaterialDepositID);
                            parameters.Add("@Material", model.Material);
                            parameters.Add("@IsManavaFormulaRequire", model.IsManavaFormulaRequire);
                            parameters.Add("@IsPatientsPhotoRequire", model.IsPatientsPhotoRequire);
                            parameters.Add("@IfDiffrentAddressForDispatch", model.IfDiffrentAddressForDispatch);
                            parameters.Add("@DifferentAddressToDispatch", model.DifferentAddressToDispatch);
                            parameters.Add("@Instructions", model.Instructions);
                            parameters.Add("@ReceiptRemarks", model.ReceiptRemarks);
                            parameters.Add("@IfAnnounceDueInFuture", model.IfAnnounceDueInFuture);
                            parameters.Add("@DocProvisonal", model.Doc1);
                            parameters.Add("@DocCheque", model.Doc2);
                            parameters.Add("@DocPayInSlip", model.Doc3);
                            parameters.Add("@CreatedBy", model.UserName);
                            parameters.Add("@TotalAmount", model.Amount);
                            parameters.Add("@CampaignID", model.CampaignID);
                            parameters.Add("@CampaignName", model.CampaignName);



                            connection.Execute("InsertDonationReceiveMaster", parameters, transaction, commandType: CommandType.StoredProcedure);

                            maxReceiveID = connection.QueryFirstOrDefault<int>("SelectMaxReceiveID", new { model.UserID, model.FinYear }, transaction, commandType: CommandType.StoredProcedure);

                            var movementParams = new DynamicParameters();
                            movementParams.Add("@ReceiveID", maxReceiveID);
                            movementParams.Add("@MovementFrom",model.ReceiveDepartment);                           
                            movementParams.Add("@UserID", model.UserID);
                            movementParams.Add("@UserName",model.UserName);
                            connection.Execute("InsertDmsMovement", movementParams, transaction, commandType: CommandType.StoredProcedure);



                            if (MobileList != null)
                            {
                                foreach (var mobileNumber in MobileList)
                                {
                                    var mobileParams = new DynamicParameters();
                                    mobileParams.Add("@REF_NO", maxReceiveID);
                                    mobileParams.Add("@CountryCode", mobileNumber.CountryCode);
                                    mobileParams.Add("@MobileNo", mobileNumber.MobileNumber);
                                    mobileParams.Add("@CreatedBy", model.UserID);
                                    connection.Execute("InsertMultiMobileInDonationReceiveMaster", mobileParams, transaction, commandType: CommandType.StoredProcedure);
                                }
                            }
                            if (IdentityList != null)
                            {
                                foreach (var identity in IdentityList)
                                {
                                    var identityParams = new DynamicParameters();
                                    identityParams.Add("@REF_NO", maxReceiveID);
                                    identityParams.Add("@IdentityType", identity.IdentityType);
                                    identityParams.Add("@IdentityNumber", identity.IdentityNumber);
                                    identityParams.Add("@CreatedBy", model.UserID);

                                    connection.Execute("InsertMultiIdentityInDonationReceiveMaster", identityParams, transaction, commandType: CommandType.StoredProcedure);
                                }
                            }
                            if (bankDetailslist != null)
                            {
                                foreach (var bankDetail in bankDetailslist)
                                {
                                    var bankParams = new DynamicParameters();
                                    bankParams.Add("@REF_NO", maxReceiveID);
                                    bankParams.Add("@BankID", bankDetail.BankID);
                                    bankParams.Add("@BankName", bankDetail.BankName);
                                    bankParams.Add("@ChequeOrDraftDate", bankDetail.ChequeDate);
                                    bankParams.Add("@ChequeOrDraftNo", bankDetail.ChequeNo);
                                    bankParams.Add("@DepositeBankID", bankDetail.DepositBankID);
                                    bankParams.Add("@DepositeBankName", bankDetail.DepositBank);
                                    bankParams.Add("@DepositeDate", bankDetail.DepositDate);
                                    bankParams.Add("@IsPdcCheque1", bankDetail.PdcCheque);
                                    bankParams.Add("@CreatedBy", model.UserID);
                                    bankParams.Add("@DonationMode", bankDetail.DonationMode);
                                    bankParams.Add("@CurrencyCode", bankDetail.Currency);
                                    bankParams.Add("@Amount", bankDetail.Amount);

                                    connection.Execute("InsertBankDetailsInDonationReceiveMultiBank", bankParams, transaction, commandType: CommandType.StoredProcedure);
                                }
                            }

                            if (Receiptdetailslist != null)
                            {
                                foreach (var receiptDetail in Receiptdetailslist)
                                {
                                    var rparameters = new DynamicParameters();
                                    rparameters.Add("@REF_NO", maxReceiveID);
                                    rparameters.Add("@HeadID", receiptDetail.HeadID);
                                    rparameters.Add("@Campaign", receiptDetail.Campaign);
                                    rparameters.Add("@HeadName", receiptDetail.HeadName);
                                    rparameters.Add("@SubHeadID", receiptDetail.SubHeadID);
                                    rparameters.Add("@SubHeadName", receiptDetail.Purpose);
                                    rparameters.Add("@Purpose", receiptDetail.Purpose);
                                    rparameters.Add("@Quantity", receiptDetail.Quantity);
                                    rparameters.Add("@Amount", receiptDetail.Amount);
                                    rparameters.Add("@CreatedOn", DateTime.Now);
                                    rparameters.Add("@CreatedBy", model.UserID);


                                    connection.Execute("InsertDonationReceiveMultiHead", rparameters, transaction, commandType: CommandType.StoredProcedure);
                                }
                            }
                            if (donorInstructionLists != null)
                            {
                                foreach (var instruction in donorInstructionLists)
                                {
                                    var instructionParams = new DynamicParameters();
                                    instructionParams.Add("@REF_NO", maxReceiveID);
                                    instructionParams.Add("@InstructionID", instruction.InstructionId);
                                    instructionParams.Add("@InstructionName", instruction.InstructionName);
                                    instructionParams.Add("@Remarks", instruction.Remarks);
                                    instructionParams.Add("@CreatedBy", model.UserID);

                                    connection.Execute("InsertDonationReceiveMultiDonorInstruc", instructionParams, transaction, commandType: CommandType.StoredProcedure);
                                }
                            }


                            if (announcelist != null)
                            {
                                foreach (var announce in announcelist)
                                {
                                    var Announcepara = new DynamicParameters();
                                    Announcepara.Add("@REF_NO", maxReceiveID);
                                    Announcepara.Add("@TotalPurposeAmount", announce.TotalPurposeAmount);
                                    Announcepara.Add("@ReceiveAmount", announce.ReceiveAmount);
                                    Announcepara.Add("@DueAmount", announce.DueAmount);
                                    Announcepara.Add("@AnnunceID", announce.AnnounceId);
                                    Announcepara.Add("@Amount", announce.Amount);
                                    Announcepara.Add("@Date", announce.Date);
                                    Announcepara.Add("@CreatedBy", model.UserID);

                                    connection.Execute("InsertDonationReceiveAnnunceDue", Announcepara, transaction, commandType: CommandType.StoredProcedure);
                                }
                            }
                            transaction.Commit();

                            //ViewBag.msg = "Receive ID:" + maxReceiveID + " is Generated Successfully";
                        }

                        catch (Exception)
                        {
                            // If an exception occurs, roll back the transaction
                            transaction.Rollback();
                            ViewBag.emsg = "An error occurred during the transaction.";

                            return View();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ViewBag.emsg = $"An error occurred: {ex.Message}";
            }
            dynamic firstDetail;
            firstDetail = new ExpandoObject();
            firstDetail.CountryList = _dbFunctions.GetCountries();
            firstDetail.paymentModeList = _dbFunctions.GetPaymentModes();
            firstDetail.currenciesList = _dbFunctions.GetCurrencyListWithCountry();
            firstDetail.bankmasterlist = _dbFunctions.GetAllBankMasters();
            firstDetail.HeadList = _dbFunctions.getHeads();
            firstDetail.SubHeadList = _dbFunctions.getSubHeads();
            firstDetail.ReceiveHeadList = _dbFunctions.getReceiveHeads();
            firstDetail.ReceiveInEventList = _dbFunctions.GetEvents();
            firstDetail.campaignlist = _dbFunctions.GetallCampaigns();
            firstDetail.donorInstructionList = _dbFunctions.GetDonorINstructionsMaster();
            firstDetail.msg = "Receive ID: " + maxReceiveID + " is Generated Successfully";
            return Ok(firstDetail);
        }

    }
}
