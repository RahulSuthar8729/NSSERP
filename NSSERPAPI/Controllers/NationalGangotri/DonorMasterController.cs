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
        public IActionResult Home(string DonorID, string DataFlag)
        {
            var details = _dbFunctions.GetDonorDataByID(Convert.ToInt32(DonorID), DataFlag);
            dynamic firstDetail;

            if (details != null && details.Any())
            {
                firstDetail = details.First();
            }
            else
            {
                firstDetail = new ExpandoObject();
            }           
            firstDetail.CountryList = _dbFunctions.GetCountries();
            firstDetail.CityList = _dbFunctions.GetCity();
            firstDetail.DonorTypes = _dbFunctions.GetDonorTypes();

            return Ok(firstDetail);

           
        }      

        [HttpPost]
        public IActionResult InsertData([FromBody] DonorMaster model)
        {           

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();

                        var parameters = new DynamicParameters();
                        parameters.Add("@DonorPersonalInfo", JsonConvert.SerializeObject(model));
                        parameters.Add("@MobileList", model.MobileList);
                        parameters.Add("@IdentityList", model.IdentityList);

                        parameters.Add("@returnResult", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

                        connection.Execute("InsertDonorMaster", parameters, commandType: CommandType.StoredProcedure);

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

