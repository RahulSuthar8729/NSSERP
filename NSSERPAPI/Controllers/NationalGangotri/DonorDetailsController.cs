using Microsoft.AspNetCore.Mvc;
using NSSERPAPI.Db_functions_for_Gangotri;
using System.Dynamic;

namespace NSSERPAPI.Controllers.NationalGangotri
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DonorDetailsController : Controller
    {
        private readonly Db_functions _dbFunctions;

        public DonorDetailsController(Db_functions dbFunctions)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
        }

        [HttpGet]
        public IActionResult GetDonorDetailsList()
        {
            var result = _dbFunctions.GetDonormasterDetailsList();
            dynamic firstDetail;
            firstDetail = new ExpandoObject();
            firstDetail.masterDetails = result;
            firstDetail.CountryList = _dbFunctions.GetCountries();
            firstDetail.CityList = _dbFunctions.GetCity();
            firstDetail.DonorTypes = _dbFunctions.GetDonorTypes();
            return Ok(firstDetail);
        }
    }
}
