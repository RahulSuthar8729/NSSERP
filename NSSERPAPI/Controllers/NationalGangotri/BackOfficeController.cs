using Microsoft.AspNetCore.Mvc;
using NSSERPAPI.Db_functions_for_Gangotri;
using NSSERPAPI.Models.NationalGangotri;
using System.Dynamic;
namespace NSSERPAPI.Controllers.NationalGangotri
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BackOfficeController : Controller
    {
        private readonly Db_functions _dbFunctions;

        public BackOfficeController(Db_functions dbFunctions)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
        }
        public IActionResult ViewDetails(int id)
        {
            var details = _dbFunctions.GetDonationReceiveMasterByReceiveID(id);

            dynamic firstDetail;

            if (details != null && details.Any())
            {
                firstDetail = details.First();
            }
            else
            {
                firstDetail = new ExpandoObject();
            }

            
            firstDetail.paymentModes = _dbFunctions.GetPaymentModes();
            firstDetail.currenciesList = _dbFunctions.GetCurrencyListWithCountry();
            firstDetail.bankmasterlist = _dbFunctions.GetAllBankMasters();           
            if (details == null)
            {
                return NotFound();
            }

            return Ok(firstDetail);
        }
    }
}
