using Microsoft.AspNetCore.Mvc;
using NSSERPAPI.Areas.ReceiptBook.Models;
using NSSERPAPI.Db_functions_for_Gangotri;
using System.Dynamic;
using System.Text.Json.Serialization;
using System.Text.Json;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace NSSERPAPI.Areas.ReceiptBook.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReceiptBookCancellationController : Controller
    {
        private readonly Db_functions _dbFunctions;
        private readonly DbEngineClass _dbEngine;
        private readonly string _connectionString;
        public ReceiptBookCancellationController(Db_functions dbFunctions, IConfiguration configuration)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
            _dbEngine = new DbEngineClass(configuration);
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        [HttpGet]
        public IActionResult Index(string DataFlag)
        {
            var result = _dbFunctions.GetReceiptBookPrintingDetais(DataFlag);
            dynamic firstDetail;
            firstDetail = new ExpandoObject();
            firstDetail.masterDetails = result;
            return Ok(firstDetail);
        }


        [HttpGet]
        public IActionResult Home(string id, string DataFlag, string TP)
        {
            var result = _dbFunctions.GetReceiptCancellationmasterById(Convert.ToInt32(id), DataFlag, TP);
            dynamic firstDetail;
            if (result != null && result.Any())
            {
                firstDetail = result.First();
            }
            else
            {
                firstDetail = new ExpandoObject();
            }

            firstDetail.receiptBookCancellationListMasterByIds = _dbFunctions.GetReceiptCancellationById(Convert.ToInt32(id), DataFlag, TP);
            firstDetail.PersonDetails = _dbFunctions.GEtPersonDetails();
            firstDetail.CancellationReasonsList = _dbFunctions.GetReceiptCancellation_master_Reasons();
            firstDetail.EmployeeDetils = _dbFunctions.GetEmployeeDetils();



            return Ok(firstDetail);
        }



        [HttpPost]
        public IActionResult UpdateData([FromBody] ReceiptBookCancellation model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@ReceiptCancelInfo", model.CancellationJson);
                    parameters.Add("@ReceiptBookNo", model.receipt_book_no);
                    parameters.Add("@PostChq", model.PostChq);
                    parameters.Add("@PersonId", model.PersonId);
                    parameters.Add("@DataFlag", model.DataFlag);
                    parameters.Add("@TP", model.TP);
                    parameters.Add("@returnResult", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

                    connection.Execute("[UpdateReceiptCancellation]", parameters, commandType: CommandType.StoredProcedure);


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
