using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSSERPAPI.Areas.ReceiptBook.Models;
using NSSERPAPI.Db_functions_for_Gangotri;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace NSSERPAPI.Areas.ReceiptBook.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SeriesMasterController : Controller
    {
        private readonly Db_functions _dbFunctions;
        private readonly DbEngineClass _dbEngine;
        public SeriesMasterController(Db_functions dbFunctions, IConfiguration configuration)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
            _dbEngine = new DbEngineClass(configuration);
        }
        [HttpGet]
        public IActionResult Index(string DataFlag)
        {
            var result = _dbFunctions.GetReceiptBookSeriesDetails(DataFlag);
            dynamic firstDetail;
            firstDetail = new ExpandoObject();
            firstDetail.masterDetails = result;
            return Ok(firstDetail);
        }

        [HttpGet]
        public IActionResult Home(string id, string DataFlag)
        {
            var result = _dbFunctions.GetReceiptBookSeriesMasterByID(Convert.ToInt32(id), DataFlag);
            dynamic firstDetail;

            if (result != null && result.Any())
            {
                firstDetail = result.First();
            }
            else
            {
                firstDetail = new ExpandoObject();
            }
            firstDetail.ReceiptBookTypeList = _dbFunctions.GetReceiptBookType();

            return Ok(firstDetail);
        }
        [HttpPost]
        public IActionResult InsertData([FromBody] SeriesMaster model)
        {
            try
            {
                var result = _dbEngine.ExecuteInsertStoredProcedure("[InsertReceiptBookSeriesMaster]", model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }




        }

        [HttpPost]
        public IActionResult UpdateData([FromBody] SeriesMaster model)
        {

            try
            {
                var result = _dbEngine.ExecuteUpdateStoredProcedure("[UpdateReceiptBookSeriesMaster]", model.Series_Code, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }



        }
    }
}
