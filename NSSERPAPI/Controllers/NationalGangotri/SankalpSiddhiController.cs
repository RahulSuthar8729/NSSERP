using Microsoft.AspNetCore.Mvc;
using NSSERPAPI.Db_functions_for_Gangotri;
using System.Dynamic;

namespace NSSERPAPI.Controllers.NationalGangotri
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SankalpSiddhiController : Controller
    {
        private readonly Db_functions _dbFunctions;

        public SankalpSiddhiController(Db_functions dbFunctions)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
        }
        [HttpGet]
        public IActionResult GetDataOnPageLoad()
        {
            var result = _dbFunctions.GetDonationReciveDetails();
            dynamic firstDetail;
            firstDetail = new ExpandoObject();
            firstDetail.masterDetails = result;
            firstDetail.CityMasterList = _dbFunctions.GetActiveCities();
            firstDetail.paymentModes = _dbFunctions.GetPaymentModes();
            firstDetail.statelist = _dbFunctions.GetStates();
            firstDetail.DepositBankList = _dbFunctions.GetDepositBankMaster();
            return Ok(firstDetail);
        }
        [HttpGet]
        public IActionResult GetDepositDetails(int refno)
        {
            var result = _dbFunctions.GetBORTDetailsListJsonById(refno);
            return Ok(result);
        }

    }
}
