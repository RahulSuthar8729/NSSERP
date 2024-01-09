using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSSERPAPI.Db_functions_for_Gangotri;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace NSSERPAPI.Controllers.NationalGangotri
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DonationReceiveMasterController : Controller
    {
        private readonly Db_functions _dbFunctions;

        public DonationReceiveMasterController(Db_functions dbFunctions)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
        }
        [HttpGet]
        public IActionResult Home(int id)
        {
            var detailsJson = HttpContext.Items["DonationDetails"] as string;
            var details = detailsJson != null ? JsonConvert.DeserializeObject<List<dynamic>>(detailsJson) : null;

            dynamic firstDetail;

            if (details != null && details.Any())
            {
                firstDetail = details.First();
            }
            else
            {
                firstDetail = new ExpandoObject();
            }

            var receiveID = ((IDictionary<string, object>)firstDetail).ContainsKey("ReceiveID")
                ? (int)((IDictionary<string, object>)firstDetail)["ReceiveID"]
                : 0;
            receiveID = id;
            // Set additional properties for firstDetail
            firstDetail.CountryList = _dbFunctions.GetCountries();
            firstDetail.paymentModeList = _dbFunctions.GetPaymentModes();
            firstDetail.currenciesList = _dbFunctions.GetCurrencyListWithCountry();
            firstDetail.bankmasterlist = _dbFunctions.GetAllBankMasters();
            firstDetail.SubHeadList = _dbFunctions.getSubHeads();
            firstDetail.ReceiveHeadList = _dbFunctions.getReceiveHeads();
            firstDetail.ReceiveInEventList = _dbFunctions.GetEvents();
            firstDetail.campaignlist = _dbFunctions.GetallCampaigns();
            firstDetail.donorInstructionList = _dbFunctions.GetDonorINstructionsMaster();

            // Set JSON properties           
            firstDetail.MobileListJson = _dbFunctions.GetMobileListJsonById(receiveID);
            firstDetail.IdentityListJson = _dbFunctions.GetIdentityListJsonById(receiveID);
            firstDetail.BankDetailsListJson = _dbFunctions.GetBankDetailsListJsonById(receiveID);
            firstDetail.ReceiptsListJson = _dbFunctions.GetReceiptsDetailsListJsonById(receiveID);
            firstDetail.DonorInstructionsListJson = _dbFunctions.GetDonorInstructionsListJsonById(receiveID);
            firstDetail.AnnounceDetailsListJson = _dbFunctions.GetAnnounceListJsonById(receiveID);

            return Ok(firstDetail);
        }
      
        [HttpGet]
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
           
            // Set additional properties for firstDetail
            firstDetail.CountryList = _dbFunctions.GetCountries();
            firstDetail.paymentModeList = _dbFunctions.GetPaymentModes();
            firstDetail.currenciesList = _dbFunctions.GetCurrencyListWithCountry();
            firstDetail.bankmasterlist = _dbFunctions.GetAllBankMasters();
            firstDetail.SubHeadList = _dbFunctions.getSubHeads();
            firstDetail.ReceiveHeadList = _dbFunctions.getReceiveHeads();
            firstDetail.ReceiveInEventList = _dbFunctions.GetEvents();
            firstDetail.campaignlist = _dbFunctions.GetallCampaigns();
            firstDetail.donorInstructionList = _dbFunctions.GetDonorINstructionsMaster();

            // Set JSON properties           
            firstDetail.MobileListJson = _dbFunctions.GetMobileListJsonById(id);
            firstDetail.IdentityListJson = _dbFunctions.GetIdentityListJsonById(id);
            firstDetail.BankDetailsListJson = _dbFunctions.GetBankDetailsListJsonById(id);
            firstDetail.ReceiptsListJson = _dbFunctions.GetReceiptsDetailsListJsonById(id);
            firstDetail.DonorInstructionsListJson = _dbFunctions.GetDonorInstructionsListJsonById(id);
            firstDetail.AnnounceDetailsListJson = _dbFunctions.GetAnnounceListJsonById(id);


            if (details == null)
            {
                return NotFound();
            }

            return Ok(firstDetail);
        }


        [HttpGet]
        public IActionResult GetStatesByCountry(int countryId)
        {
            try
            {
                var states = _dbFunctions.GetStatesByCountry(countryId);

                return Ok(states);
            }
            catch (Exception ex)
            {
                return BadRequest("Error retrieving states.");
            }
        }

        [HttpGet]
        public IActionResult GetDistrictsByState(int stateId)
        {
            try
            {
                // Use your database connection logic here to retrieve districts based on the state ID
                var districts = _dbFunctions.GetDistrictsByState(stateId); // Implement this method in your DbFunctions class

                // Return districts as JSON
                return Ok(districts);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("An error occurred while retrieving districts.");
            }
        }

        [HttpGet]
        public IActionResult GetCitiesByDistrictID(int districtId)
        {
            try
            {
                var cities = _dbFunctions.GetCitiesByDistrictID(districtId);
                return Ok(cities);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("An error occurred while retrieving cities.");
            }
        }

        [HttpGet]
        public IActionResult GetDataByDonorID(string DonorID)
        {
            try
            {
                var Data = _dbFunctions.GetDataByDonorID(DonorID);
                if (Data.PinCode != null)
                {               

                    var locationDetails = _dbFunctions.GetLocationDetailsByPinCodeJson(Convert.ToString(Data.PinCode));
                    var mobilelist = _dbFunctions.GetMobileListJsonById(Convert.ToInt32(Data.ReceiveID));
                    var identityList = _dbFunctions.GetIdentityListJsonById(Convert.ToInt32(Data.ReceiveID));

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
        public IActionResult GetLocationDetailsByPinCode(string pincode)
        {
            try
            {
                var Location = _dbFunctions.GetLocationDetailsByPinCode(pincode);
                return Ok(Location);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("An error occurred while retrieving cities.");
            }
        }


        [HttpPost]
        public IActionResult InsertData([FromBody] dynamic data)
        {
            try
            {              
                   

                    return Ok("Data inserted successfully");
                

            }
            catch (Exception ex)
            {
                // Log or handle exceptions appropriately
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
