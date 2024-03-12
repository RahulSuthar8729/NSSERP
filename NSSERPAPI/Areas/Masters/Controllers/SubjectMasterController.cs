using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSSERPAPI.Db_functions_for_Gangotri;
using NSSERPAPI.Models.ReceiptDispatch;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace NSSERPAPI.Areas.Masters.Controllers
{
    [ApiController]
    [Area("Masters")]
    [Route("api/[controller]/[action]")]
    public class SubjectMasterController : Controller
    {
        private readonly Db_functions _dbFunctions;
        private readonly string _connectionString;
        public SubjectMasterController(Db_functions dbFunctions, IConfiguration configuration)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        [HttpGet]
        public IActionResult Index(string DataFlag)
        {
            var result = _dbFunctions.GetSubjectDetails(DataFlag);
            dynamic firstDetail;
            firstDetail = new ExpandoObject();
            firstDetail.masterDetails = result;
            return Ok(firstDetail);
        }

        [HttpGet]
        public IActionResult Home(string id, string DataFlag)
        {
            var result = _dbFunctions.GetSubjectById(Convert.ToInt32(id), DataFlag);
            dynamic firstDetail;

            if (result != null && result.Any())
            {
                firstDetail = result.First();
            }
            else
            {
                firstDetail = new ExpandoObject();
            }
            return Ok(firstDetail);
        }

        [HttpPost]
        public IActionResult InsertData([FromBody] SubjectMaster model)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();

                        var parameters = new DynamicParameters();
                        parameters.Add("@subjectinfo", JsonConvert.SerializeObject(model));
                        parameters.Add("@returnresult", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

                        connection.Execute("[InsertSubjectMaster]", parameters, commandType: CommandType.StoredProcedure);

                        var result = parameters.Get<string>("@returnResult");

                        string msg = result;
                        return Ok(msg);
                    }
                    catch (Exception ex)
                    {

                        return View();
                    }
                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }



        }

        [HttpPost]
        public IActionResult UpdateData([FromBody] SubjectMaster model)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();

                        var parameters = new DynamicParameters();
                        parameters.Add("@subjectinfo", JsonConvert.SerializeObject(model));
                        parameters.Add("@subjectid", model.subject_id);
                        parameters.Add("@returnResult", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

                        connection.Execute("[UpdateSubjectMaster]", parameters, commandType: CommandType.StoredProcedure);

                        var result = parameters.Get<string>("@returnResult");

                        string msg = result;
                        return Ok(msg);
                    }
                    catch (Exception ex)
                    {

                        return View();
                    }
                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }



        }
    }
}
