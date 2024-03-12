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
    public class PostTypeMasterController : Controller
    {
        private readonly Db_functions _dbFunctions;
        private readonly DbEngineClass _dbEngine;
        public PostTypeMasterController(Db_functions dbFunctions, IConfiguration configuration)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
            _dbEngine = new DbEngineClass(configuration);
        }
        [HttpGet]
        public IActionResult Index(string DataFlag)
        {
            var result = _dbFunctions.GetPostTypeDetails(DataFlag);
            dynamic firstDetail;
            firstDetail = new ExpandoObject();
            firstDetail.masterDetails = result;
            return Ok(firstDetail);
        }

        [HttpGet]
        public IActionResult Home(string id, string DataFlag)
        {
            var result = _dbFunctions.GetPostTypeById(Convert.ToInt32(id), DataFlag);
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
        public IActionResult InsertData([FromBody] PostTypeMaster model)
        {

            try
            {
                var result = _dbEngine.ExecuteInsertStoredProcedure("[InsertPostTypeMaster]",model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UpdateData([FromBody] PostTypeMaster model)
        {

            try
            {
                var result = _dbEngine.ExecuteUpdateStoredProcedure("[UpdatePostTypeMaster]",model.Post_Type_ID, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }



        }
    }
}
