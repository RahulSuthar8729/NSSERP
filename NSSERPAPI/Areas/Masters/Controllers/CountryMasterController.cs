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
    public class CountryMasterController : Controller
    {
        private readonly Db_functions _dbFunctions;
        private readonly string _connectionString;
        public CountryMasterController(Db_functions dbFunctions, IConfiguration configuration)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpGet]
        public IActionResult Index()
        {  
            var result = _dbFunctions.GetCountryList();
            dynamic firstDetail;
            firstDetail = new ExpandoObject();
            firstDetail.masterDetails = result;         
            return Ok(firstDetail);
        }

        [HttpGet]
        public IActionResult GetCountrybyID(string CountryId)
        {
            var result = _dbFunctions.GetCountryByID(Convert.ToInt32(CountryId));            
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
        public IActionResult InsertData([FromBody] CountryMaster model)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();

                        var parameters = new DynamicParameters();
                        parameters.Add("@CountryInfo", JsonConvert.SerializeObject(model));                       

                        parameters.Add("@returnResult", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

                        connection.Execute("[InsertCountryMaster]", parameters, commandType: CommandType.StoredProcedure);

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
        public IActionResult UpdateData([FromBody] CountryMaster model)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();

                        var parameters = new DynamicParameters();
                        parameters.Add("@CountryInfo", JsonConvert.SerializeObject(model));
                        parameters.Add("@CountryCode", model.Country_Code);

                        parameters.Add("@returnResult", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

                        connection.Execute("[UpdateCountryMaster]", parameters, commandType: CommandType.StoredProcedure);

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
