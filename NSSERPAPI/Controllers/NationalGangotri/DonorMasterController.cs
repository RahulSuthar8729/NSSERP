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
            List<MobileDetails> MobileList = string.IsNullOrEmpty(model.MobileList) ? new List<MobileDetails>() : JsonConvert.DeserializeObject<List<MobileDetails>>(model.MobileList);

            List<IdentityDetails> IdentityList = string.IsNullOrEmpty(model.IdentityList) ? new List<IdentityDetails>() : JsonConvert.DeserializeObject<List<IdentityDetails>>(model.IdentityList);


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


                            parameters.Add("@DonorPersonalInfo", JsonConvert.SerializeObject(model));
                            parameters.Add("@MobileList",model.MobileList);
                            parameters.Add("@IdentityList", model.IdentityList);



                            //parameters.Add("@Anonymous", (model.IsAnonymous ?? false) ? "Y" : "N");
                            //parameters.Add("@Online_CustomerId", model.onlineCustId);
                            //parameters.Add("@NGCode", model.DonorID);
                            //parameters.Add("@Don_Id", model.DonorID);
                            //parameters.Add("@GType", model.GroupNGCode);

                            //parameters.Add("@Donor_Category", model.DonorCat);
                            //parameters.Add("@DONOR_TYPE", model.DonorType);
                            //parameters.Add("@Ref_No", model.referenceNo);
                            //parameters.Add("@DONOR_POST_TYPE", model.DonorType);

                            //parameters.Add("@B_Type", model.BussinessOrJobType);
                            //parameters.Add("@UPI_ID", model.upiId);
                            //parameters.Add("@Receive_PostType", model.BussinessOrJobType);
                            //parameters.Add("@DShri", model.NamePrefix);
                            //parameters.Add("@DName", model.FirstName);
                            //parameters.Add("@MiddleName", model.MiddleName);
                            //parameters.Add("@DLName", model.LastName);
                            //parameters.Add("@DOB", model.DateOfBirth);

                            //parameters.Add("@CO_Title", model.PrefixToRelation);
                            //parameters.Add("@DFatherName", model.RelationToFullName);
                            //parameters.Add("@DOA", model.DateOfAnniversary);

                            //parameters.Add("@ncity_id", model.CityID);
                            //parameters.Add("@IAddress", model.Address1);
                            //parameters.Add("@DOE", model.dateOfEntry);
                            //parameters.Add("@Category", model.DonorCat);
                            //parameters.Add("@Reference", model.referenceNo);
                            //parameters.Add("@Designation", model.Designation);
                            //parameters.Add("@Company", model.Company);
                            //parameters.Add("@Add1", model.Address1);
                            //parameters.Add("@Add2", model.Address2);
                            //parameters.Add("@Add3", model.Address3);
                            //parameters.Add("@COUNTRY_CODE", model.CountryId);
                            //parameters.Add("@country", model.CountryName);
                            //parameters.Add("@State_code", model.StateID);
                            //parameters.Add("@State", model.StateName);
                            //parameters.Add("@District_CODE", model.DistrictID);
                            //parameters.Add("@District", model.DistrictName);
                            //parameters.Add("@CITY_CODE", model.CityID);
                            //parameters.Add("@Place", model.CityName);
                            //parameters.Add("@Pincode", model.PinCode);

                            //parameters.Add("@Same_Add", (model.IsPermanentAddressDiff ?? false) ? "Y" : "N");

                            //parameters.Add("@OffAdd1", model.P_Address1);
                            //parameters.Add("@OffAdd2", model.P_Address2);
                            //parameters.Add("@OffAdd3", model.P_Address3);
                            //parameters.Add("@PerCountry_Code", model.P_CountryID);
                            //parameters.Add("@PerState_Code", model.P_StateID);
                            //parameters.Add("@PerDistrict_Code", model.P_DistrictID);
                            //parameters.Add("@PerCity_Code", model.P_CityID);
                            //parameters.Add("@PerPinCode", model.P_PinCode);

                            //parameters.Add("@PerPincode_Code", model.P_PinCode);

                            //parameters.Add("@IsSandipan", (model.IsSandipanSend ?? false) ? "Y" : "N");

                            //parameters.Add("@TotalAmount", model.ToatlDonation);
                            //parameters.Add("@user_name", model.UserName);

                            //parameters.Add("@IsSandipan_original", (model.IsSandipanSend ?? false) ? "Y" : "N");

                            //parameters.Add("@TotalAmount_original", model.ToatlDonation);
                            //parameters.Add("@Receipt_Copy", model.ReceiptCopyRequireOptions);
                            //parameters.Add("@care_of", model.CareOf);

                            //parameters.Add("@Remark", model.Remarks);

                            //parameters.Add("@SMS", model.IsMsgActive);
                            //parameters.Add("@Visit", (model.IsVisit ?? false) ? "Y" : "N");

                            //parameters.Add("@Visit_Year", model.VisitYear);
                            //parameters.Add("@Website", model.Website);
                            //parameters.Add("@Sandipan_Remark", model.SandipanRemarks);

                            //parameters.Add("@Entry_By", model.UserID);
                            //parameters.Add("@Lang", model.language);
                            //parameters.Add("@Sandipan_NoReason", model.SandipanRemarksReason);

                            //parameters.Add("@DATA_FLAG", model.DataFlag);

                            //parameters.Add("@Donor_Remark", model.Remarks);
                            //parameters.Add("@Cont_whatsApp", (model.IsWhatsAppActive ?? false) ? "Y" : "N");
                            //parameters.Add("@Cont_Email", (model.IsEmailActive ?? false) ? "Y" : "N");

                            //parameters.Add("@Cont_Letter", (model.IsLetterCommunicationActive ?? false) ? "Y" : "N");



                            //parameters.Add("@CreatedBy", model.UserName);
                            //parameters.Add("@MobileList", model.MobileList);
                            //parameters.Add("@IdentityList", model.IdentityList);

                            connection.Execute("InsertDonorMaster", parameters, transaction, commandType: CommandType.StoredProcedure);

                           // connection.Execute("InsertDonorMaster", parameters, commandType: CommandType.StoredProcedure);




                            if (MobileList != null)
                            {
                                foreach (var mobileNumber in MobileList)
                                {
                                    var mobileParams = new DynamicParameters();
                                    mobileParams.Add("@REF_NO",null);
                                    mobileParams.Add("@ReceiveHeadName", null);
                                    mobileParams.Add("@DonorID", model.DonorID);
                                    mobileParams.Add("@ContactType", mobileNumber.ContactType);
                                    mobileParams.Add("@CountryCode", mobileNumber.CountryCode);
                                    mobileParams.Add("@MobileNo", mobileNumber.ContactDetail);
                                    mobileParams.Add("@DataFlag", model.DataFlag);
                                    mobileParams.Add("@CreatedBy", model.UserID);
                                    connection.Execute("InsertMultiMobileInDonationReceiveMaster", mobileParams, transaction, commandType: CommandType.StoredProcedure);
                                }
                            }
                            if (IdentityList != null)
                            {
                                foreach (var identity in IdentityList)
                                {
                                    var identityParams = new DynamicParameters();
                                    identityParams.Add("@REF_NO", null);
                                    identityParams.Add("@ReceiveHeadName",null);
                                    identityParams.Add("@DonorID", model.DonorID);
                                    identityParams.Add("@IdentityType", identity.IdentityType);
                                    identityParams.Add("@IdentityNumber", identity.IdentityNumber);
                                    identityParams.Add("@DataFlag", model.DataFlag);
                                    identityParams.Add("@CreatedBy", model.UserID);

                                    connection.Execute("InsertMultiIdentityInDonationReceiveMaster", identityParams, transaction, commandType: CommandType.StoredProcedure);
                                }
                            }




                            transaction.Commit();

                           
                        }

                        catch (Exception ex)
                        {
                            
                            transaction.Rollback();
                            

                            return View();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dynamic msg;
                msg = new ExpandoObject();
                msg.emsg = ex.Message;
                return Ok(msg);
                ViewBag.emsg = $"An error occurred: {ex.Message}";
            }

         
            model.msg = "Donor Created Successfully";
            return Ok(model);
        }


    }
}

