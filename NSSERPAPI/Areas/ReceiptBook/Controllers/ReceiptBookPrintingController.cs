using Microsoft.AspNetCore.Mvc;
using NSSERPAPI.Areas.ReceiptBook.Models;
using NSSERPAPI.Db_functions_for_Gangotri;
using System.Dynamic;

namespace NSSERPAPI.Areas.ReceiptBook.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReceiptBookPrintingController : Controller
    {
        private readonly Db_functions _dbFunctions;
        private readonly DbEngineClass _dbEngine;
        public ReceiptBookPrintingController(Db_functions dbFunctions, IConfiguration configuration)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
            _dbEngine = new DbEngineClass(configuration);
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
        public IActionResult Home(string id, string DataFlag)
        {
            var result = _dbFunctions.GetReceiptBookPrintingBYId(Convert.ToInt32(id), DataFlag);
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
        public IActionResult InsertData([FromBody] ReceiptBookPrinting model)
        {
            try
            {
                var result = _dbEngine.ExecuteInsertStoredProcedure("[InsertReceiptBookPrinting]", model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UpdateData([FromBody] ReceiptBookPrinting model)
        {

            try
            {
                var result = _dbEngine.ExecuteUpdateStoredProcedure("[updatepersoncategorymaster]", model.receipt_book_no, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }
    }
}
