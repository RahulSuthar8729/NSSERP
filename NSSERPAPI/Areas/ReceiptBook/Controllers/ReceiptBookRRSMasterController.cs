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
    public class ReceiptBookRRSMasterController : Controller
    {
        private readonly Db_functions _dbFunctions;
        private readonly DbEngineClass _dbEngine;
        private readonly string _connectionString;
        public ReceiptBookRRSMasterController(Db_functions dbFunctions, IConfiguration configuration)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
            _dbEngine = new DbEngineClass(configuration);
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        [HttpGet]
        public IActionResult Index(string DataFlag)
        {
            var result = _dbFunctions.getReceiptBookRRSdetails(DataFlag);
            dynamic firstDetail;
            firstDetail = new ExpandoObject();
            firstDetail.masterDetails = result;
            return Ok(firstDetail);
        }

        [HttpGet]
        public IActionResult Home(string id, string DataFlag)
        {
            var result = _dbFunctions.getReceiptBookRRSMasterbyId(Convert.ToInt32(id), DataFlag);
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
            return Ok(firstDetail);
        }
        [HttpPost]
        public IActionResult InsertData([FromBody] ReceiptBookRRSMaster model)
        {
            try
            {
                var result = _dbEngine.ExecuteInsertStoredProcedure("[InsertReceiptBookRRS]", model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UpdateData([FromBody] ReceiptBookRRSMaster model)
        {

            try
            {
                var result = _dbEngine.ExecuteUpdateStoredProcedure("[UpdateReceiptBookRRS]", model.book_rrs_no, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [HttpGet]
        public IActionResult Discard(string id, string DataFlag)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@RRSNO", Convert.ToInt32(id));
                    parameters.Add("@DataFlag", DataFlag);
                    parameters.Add("@returnResult", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                    connection.Execute("[ReceiptBookRRSDiscard]", parameters, commandType: CommandType.StoredProcedure);

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
