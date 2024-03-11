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
    public class PurposeYojanaMasterController : Controller
    {
        private readonly Db_functions _dbFunctions;
        private readonly string _connectionString;
        public PurposeYojanaMasterController(Db_functions dbFunctions, IConfiguration configuration)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        [HttpGet]
        public IActionResult Index(string DataFlag)
        {
            var result = _dbFunctions.GetPurposeYojanaDetails(DataFlag);
            dynamic firstDetail;
            firstDetail = new ExpandoObject();
            firstDetail.masterDetails = result;
            return Ok(firstDetail);
        }

        [HttpGet]
        public IActionResult Home(string id, string DataFlag)
        {
            var result = _dbFunctions.GetPurposeYojanaById(Convert.ToInt32(id), DataFlag);
            dynamic firstDetail;

            if (result != null && result.Any())
            {
                firstDetail = result.First();
            }
            else
            {
                firstDetail = new ExpandoObject();
            }
            firstDetail.HeadList = _dbFunctions.getHeads();
            firstDetail.CurrencyList = _dbFunctions.GetCurrencyDetails();

            return Ok(firstDetail);
        }
        [HttpPost]
        public IActionResult InsertData([FromBody] CombinedPurposeYojanaandCurrencyMaster model)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();

                        var parameters = new DynamicParameters();
                        parameters.Add("@PurposeYojnaInfo", JsonConvert.SerializeObject(model.PurposeYojanaMaster));
                        parameters.Add("@YojnaCurrencyInfo",model.PurposeYojanaCurrencyMaster.CurrencyData);

                        parameters.Add("@returnResult", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

                        connection.Execute("[InsertPurposeYojnaMaster]", parameters, commandType: CommandType.StoredProcedure);

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
        public IActionResult UpdateData([FromBody] CombinedPurposeYojanaandCurrencyMaster model)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();

                        var parameters = new DynamicParameters();
                        parameters.Add("@PurposeYojnaInfo", JsonConvert.SerializeObject(model.PurposeYojanaMaster));
                        parameters.Add("@YojnaCurrencyInfo", model.PurposeYojanaCurrencyMaster.CurrencyData);
                        parameters.Add("@YojnaId", model.PurposeYojanaMaster.Yojna_ID);
                        parameters.Add("@DataFlag", model.PurposeYojanaMaster.DATA_FLAG);
                        parameters.Add("@returnResult", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

                        connection.Execute("[UpdatePurposeYojnaMaster]", parameters, commandType: CommandType.StoredProcedure);

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
