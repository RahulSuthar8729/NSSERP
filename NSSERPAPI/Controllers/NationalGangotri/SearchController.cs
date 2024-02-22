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

namespace NSSERPAPI.Controllers.NationalGangotri
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SearchController : Controller
    {
        private readonly Db_functions _dbFunctions;
        private readonly string _connectionString;
        public SearchController(Db_functions dbFunctions, IConfiguration configuration)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        [HttpGet]
        public IActionResult SearchDonorDataByPara([FromQuery] string searchType, [FromQuery] string searchData)
        {
            try
            {                
                var Data = _dbFunctions.SearchDonorDetails(new SearchDonorDetails { SearchType = searchType, searchData = searchData });
                if (Data.Pincode != null)
                {
                    var locationDetails = _dbFunctions.GetLocationDetailsByPinCodeJson(Convert.ToString(Data.Pincode));
                    var mobilelist = _dbFunctions.GetMobileListJsonByDonorID(Convert.ToInt32(Data.DonorID));
                    var identityList = _dbFunctions.GetIdentityListJsonByDonorID(Convert.ToInt32(Data.DonorID));

                    Data.pinCodeMasterList = locationDetails;
                    Data.MobileListJson = mobilelist;
                    Data.IdentityListJson = identityList;

                }

                return Ok(Data);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("An error occurred while retrieving location details.");
            }
        }


        [HttpGet]
        public IActionResult GetPersonbyProvisonal(string ReceiptNo, string TP, string DataFlag)
        {
            try
            {
                var data = _dbFunctions.GetPersonNameByProvisonal(Convert.ToInt32(ReceiptNo), TP, DataFlag);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest("Error retrieving states.");
            }
        }

    }
}
