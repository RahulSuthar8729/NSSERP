using Dapper;
using Microsoft.AspNetCore.Mvc;
using NSSERPAPI.Areas.ReceiptBook.Models;
using NSSERPAPI.Db_functions_for_Gangotri;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace NSSERPAPI.Areas.ReceiptBook.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReceiptBookIssueController : Controller
    {
        private readonly Db_functions _dbFunctions;
        private readonly DbEngineClass _dbEngine;
        private readonly string _connectionString;
        public ReceiptBookIssueController(Db_functions dbFunctions, IConfiguration configuration)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
            _dbEngine = new DbEngineClass(configuration);
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        [HttpGet]
        public IActionResult Index(string DataFlag)
        {
            var result = _dbFunctions.GetReceiptBookIssueDetails(DataFlag);
            dynamic firstDetail;
            firstDetail = new ExpandoObject();
            firstDetail.masterDetails = result;
            return Ok(firstDetail);
        }

        [HttpGet]
        public IActionResult Home(string id, string DataFlag)
        {
            var result = _dbFunctions.GetReceiptBookIssuebyId(Convert.ToInt32(id), DataFlag);
            dynamic firstDetail;

            if (result != null && result.Any())
            {
                firstDetail = result.First();
            }
            else
            {
                firstDetail = new ExpandoObject();
            }           
            firstDetail.PersonDetails = _dbFunctions.getPersondetails(DataFlag);
            firstDetail.ReceiveInEventList = _dbFunctions.GetEProgramDetils(DataFlag);
            firstDetail.CityList = _dbFunctions.GetCity();
            return Ok(firstDetail);
        }
        [HttpPost]
        public IActionResult InsertData([FromBody] ReceiptBookIssue model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@ProvReceiptFrom", model.ProvReceiptFrom);
                    parameters.Add("@ProvReceiptTo", model.ProvReceiptTo);
                    parameters.Add("@RRSNo", model.RRSNo);
                    parameters.Add("@PersonId", model.PersonId);
                    parameters.Add("@PersonCatCode", model.PersonCatCode);
                    parameters.Add("@SubmitDate", model.SubmitDate);
                    parameters.Add("@ReceiptBookNoFrom", model.ReceiptBookNoFrom);
                    parameters.Add("@ReceiptBookNoTo", model.ReceiptBookNoTo);
                    parameters.Add("@IssueAuthority", model.IssueAuthority);
                    parameters.Add("@ReceiptUsed", model.ReceiptUsed);
                    parameters.Add("@IssueDate", model.IssueDate);
                    parameters.Add("@Submitted", model.Submitted);
                    parameters.Add("@TakenBy", model.TakenBy);
                    parameters.Add("@UserName", model.UserName);
                    parameters.Add("@Remark", model.Remark);
                    parameters.Add("@BookHolderName", model.BookHolderName);
                    parameters.Add("@Event", model.Event);
                    parameters.Add("@EventName", model.EventName);
                    parameters.Add("@EventPlace", model.EventPlace);
                    parameters.Add("@EventFDate", model.EventFDate);
                    parameters.Add("@EventTDate", model.EventTDate);
                    parameters.Add("@EventId", model.EventId);
                    parameters.Add("@RefPersonid", model.RefPersonid);
                    parameters.Add("@BookRRSNo", model.BookRRSNo);
                    parameters.Add("@DeptSubmitt", model.DeptSubmitt);
                    parameters.Add("@ReceiptBookType", model.ReceiptBookType);
                    parameters.Add("@DataFlag", model.DataFlag);
                    parameters.Add("@FYId", model.FYId);
                    parameters.Add("@returnResult", dbType: DbType.String, direction: ParameterDirection.Output, size: 50); // Output parameter

                    connection.Execute("[InsertReceiptBookIssue]", parameters, commandType: CommandType.StoredProcedure);

                   
                    string result = parameters.Get<string>("@returnResult");

                    
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


    }
}
