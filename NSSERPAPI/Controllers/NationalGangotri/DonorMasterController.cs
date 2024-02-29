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
using System.Globalization;
using System.Security.Claims;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace NSSERPAPI.Controllers.NationalGangotri
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DonorMasterController : Controller
    {
        private readonly Db_functions _dbFunctions;
        private readonly string _connectionString;
        public DonorMasterController(Db_functions dbFunctions, IConfiguration configuration)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpGet]
        public IActionResult Home()
        {

            dynamic firstDetail;
            firstDetail = new ExpandoObject();
            firstDetail.CountryList = _dbFunctions.GetCountries();
            firstDetail.CityList = _dbFunctions.GetCity();
            firstDetail.DonorTypes = _dbFunctions.GetDonorTypes();


            return Ok(firstDetail);
        }

        [HttpPost]
        public IActionResult InsertData([FromBody] DonorMaster model)
        {
            string mobileListJson = HttpContext.Request.Headers["MobileList"];
            string identityListJson = HttpContext.Request.Headers["IdentityList"];          


            List<MobileDetails> MobileList = string.IsNullOrEmpty(mobileListJson) ? new List<MobileDetails>() : JsonConvert.DeserializeObject<List<MobileDetails>>(mobileListJson);

            List<IdentityDetails> IdentityList = string.IsNullOrEmpty(identityListJson) ? new List<IdentityDetails>() : JsonConvert.DeserializeObject<List<IdentityDetails>>(identityListJson);



            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    try
                    {
                        var parameters = new DynamicParameters();

                        // Basic Information
                        parameters.Add("@FinYear", model.FinYear);
                        parameters.Add("@UserID", model.UserID);
                        parameters.Add("@UserName", model.UserName);
                        parameters.Add("@DataFlag", model.DataFlag);
                        parameters.Add("@IsAnonymous", model.IsAnonymous);
                        parameters.Add("@GroupNGCode", model.GroupNGCode);
                        parameters.Add("@onlineCustId", model.onlineCustId);
                        parameters.Add("@upiId", model.upiId);
                        parameters.Add("@mailingNo", model.mailingNo);
                        parameters.Add("@referenceNo", model.referenceNo);
                        parameters.Add("@dateOfEntry", model.dateOfEntry);

                        // Donor Information
                        parameters.Add("@DonorCat", model.DonorCat);
                        parameters.Add("@IsAppShreekaPurnVivranReceived", model.IsAppShreekaPurnVivranReceived);
                        parameters.Add("@NamePrefix", model.NamePrefix);
                        parameters.Add("@FirstName", model.FirstName);
                        parameters.Add("@MiddleName", model.MiddleName);
                        parameters.Add("@LastName", model.LastName);
                        parameters.Add("@DateOfBirth", model.DateOfBirth);
                        parameters.Add("@RelationToFullName", model.PrefixToRelation + " " + model.RelationToFullName);
                        parameters.Add("@DateOfAnniversary", model.DateOfAnniversary);
                        parameters.Add("@Company", model.Company);
                        parameters.Add("@YourCompany", model.YourCompany);
                        parameters.Add("@BussinessOrJobType", model.BussinessOrJobType);
                        parameters.Add("@Profession", model.Profession);
                        parameters.Add("@WorkingIn", model.WorkingIn);
                        parameters.Add("@Designation", model.Designation);
                        parameters.Add("@CareOf", model.CareOf);
                        parameters.Add("@ToatlDonation", model.ToatlDonation);

                        // Address Information
                        parameters.Add("@Address1", model.Address1);
                        parameters.Add("@Address2", model.Address2);
                        parameters.Add("@Address3", model.Address3);
                        parameters.Add("@PinCode", model.PinCode);
                        parameters.Add("@CountryID", model.CountryId);
                        parameters.Add("@CountryName", model.CountryName);
                        parameters.Add("@StateID", model.StateID);
                        parameters.Add("@StateName", model.StateName);
                        parameters.Add("@DistrictID", model.DistrictID);
                        parameters.Add("@DistrictName", model.DistrictName);
                        parameters.Add("@CityID", model.CityID);
                        parameters.Add("@CityName", model.CityName);

                        // Permanent Address Information
                        parameters.Add("@P_Address1", model.P_Address1);
                        parameters.Add("@P_Address2", model.P_Address2);
                        parameters.Add("@P_Address3", model.P_Address3);
                        parameters.Add("@P_PinCode", model.P_PinCode);
                        parameters.Add("@P_CountryID", model.P_CountryID);
                        parameters.Add("@P_CountryName", model.P_CountryName);
                        parameters.Add("@P_StateID", model.P_StateID);
                        parameters.Add("@P_StateName", model.P_StateName);
                        parameters.Add("@P_DistrictID", model.P_DistrictID);
                        parameters.Add("@P_DistrictName", model.P_DistrictName);
                        parameters.Add("@P_CityID", model.P_CityID);
                        parameters.Add("@P_CityName", model.P_CityName);

                        // Communication Information
                        parameters.Add("@IsCallActive", model.IsCallActive);
                        parameters.Add("@IsMsgActive", model.IsMsgActive);
                        parameters.Add("@IsWhatsAppActive", model.IsWhatsAppActive);
                        parameters.Add("@IsEmailActive", model.IsEmailActive);
                        parameters.Add("@IsLetterCommunicationActive", model.IsLetterCommunicationActive);
                        parameters.Add("@IsSendoperationPhotoActive", model.IsSendoperationPhotoActive);

                        // List Information
                        parameters.Add("@MobileList", model.MobileList);
                        parameters.Add("@IdentityList", model.IdentityList);

                        // Other Information
                        parameters.Add("@Sandipan", model.Sandipan);
                        parameters.Add("@language", model.language);
                        parameters.Add("@SandipanRemarksReason", model.SandipanRemarksReason);
                        parameters.Add("@SandipanRemarks", model.SandipanRemarks);
                        parameters.Add("@ReceiptCopyRequireOptions", model.ReceiptCopyRequireOptions);
                        parameters.Add("@IsVisit", model.IsVisit);
                        parameters.Add("@VisitYear", model.VisitYear);
                        parameters.Add("@ForginNgCodeRefrence", model.ForginNgCodeRefrence);
                        parameters.Add("@ChangesRemarks", model.ChangesRemarks);
                        parameters.Add("@Remarks", model.Remarks);

                        // Document Information
                        parameters.Add("@Doc1", model.Doc1);
                        parameters.Add("@Doc2", model.Doc2);
                        parameters.Add("@Doc3", model.Doc3);

                        
                        connection.Execute("InsertDonorMaster", parameters, commandType: CommandType.StoredProcedure);
                      
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions as needed
                        ViewBag.emsg = "An error occurred during the transaction.";
                        return View();
                    }
                }

            }
            catch (Exception ex)
            {
                dynamic msg;
                msg = new ExpandoObject();
                msg.emsg = ex.Message;
                return Ok(msg);               
            }
            model.msg = "";
            return Ok(model);
        }


    }
}

