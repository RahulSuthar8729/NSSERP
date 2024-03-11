using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSSERPAPI.Areas.Masters.Models;
using NSSERPAPI.Db_functions_for_Gangotri;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace NSSERPAPI.Areas.Masters.Controllers
{
    [ApiController]
    [Area("Masters")]
    [Route("api/[controller]/[action]")]
    public class PurposeMasterController : Controller
    {
        private readonly Db_functions _dbFunctions;
        private readonly string _connectionString;
        public PurposeMasterController(Db_functions dbFunctions, IConfiguration configuration)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        [HttpGet]
        public IActionResult Index(string DataFlag)
        {
            var result = _dbFunctions.GetPurposeDetails(DataFlag);
            dynamic firstDetail;
            firstDetail = new ExpandoObject();
            firstDetail.masterDetails = result;        
            return Ok(firstDetail);
        }

        [HttpGet]
        public IActionResult Home(string id, string DataFlag)
        {
            var result = _dbFunctions.GetPurposeById(Convert.ToInt32(id),DataFlag);
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
        public IActionResult InsertData([FromBody] PurposeMaster model)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();

                        var parameters = new DynamicParameters();
                        parameters.Add("@PurposeInfo", JsonConvert.SerializeObject(model));

                        parameters.Add("@returnResult", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

                        connection.Execute("[InsertPurposeMaster]", parameters, commandType: CommandType.StoredProcedure);

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
        public IActionResult UpdateData([FromBody] PurposeMaster model)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();

                        var parameters = new DynamicParameters();
                        parameters.Add("@PurposeInfo", JsonConvert.SerializeObject(model));
                        parameters.Add("@PurposeId", model.Purpose_id);
                        parameters.Add("@returnResult", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

                        connection.Execute("[UpdatePurposeMaster]", parameters, commandType: CommandType.StoredProcedure);

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
