using Microsoft.AspNetCore.Mvc;
using NSSERPAPI.Db_functions_for_Gangotri;
using NSSERPAPI.Models.NationalGangotri;
using System.Dynamic;

namespace NSSERPAPI.Controllers.NationalGangotri
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DonationReceiveDetailsController : Controller
    {
        private readonly Db_functions _dbFunctions;

        public DonationReceiveDetailsController(Db_functions dbFunctions)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
        }

        [HttpGet]
        public IActionResult GetDonationReceiveDetails()
        {
            var result = _dbFunctions.GetDonationReciveDetails();
            return Ok(result);
        }

        [HttpGet("GetActiveCities")]
        public IActionResult GetActiveCities()
        {
            var result = _dbFunctions.GetActiveCities();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult SearchDonationReceiveDataBYPara([FromBody] DonationReceiveDetailsWithParaModel modelItems)
        {
            var modelreult = _dbFunctions.SearchDonationDetailsByPara(modelItems);
            dynamic firstDetail;
            firstDetail = new ExpandoObject();
            firstDetail.masterDetails = modelreult;
            firstDetail.CityMasterList = _dbFunctions.GetActiveCities();
            firstDetail.paymentModes = _dbFunctions.GetPaymentModes();
            firstDetail.statelist = _dbFunctions.GetStates();          
            return Ok(firstDetail);
        }
    }

}
