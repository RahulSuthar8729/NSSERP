using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSSERPAPI.Db_functions_for_Gangotri;
using System.Dynamic;

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
        public IActionResult Home()
        {

            dynamic firstDetail;
            firstDetail = new ExpandoObject();
            firstDetail.CountryList = _dbFunctions.GetCountries();
            firstDetail.CityList = _dbFunctions.GetCity();
            firstDetail.DonorTypes = _dbFunctions.GetDonorTypes();


            return Ok(firstDetail);
        }

    }
}
