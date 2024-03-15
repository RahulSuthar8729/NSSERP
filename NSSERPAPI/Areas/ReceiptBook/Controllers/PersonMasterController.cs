﻿using Microsoft.AspNetCore.Mvc;
using NSSERPAPI.Areas.ReceiptBook.Models;
using NSSERPAPI.Db_functions_for_Gangotri;
using System.Dynamic;

namespace NSSERPAPI.Areas.ReceiptBook.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PersonMasterController : Controller
    {
        private readonly Db_functions _dbFunctions;
        private readonly DbEngineClass _dbEngine;
        public PersonMasterController(Db_functions dbFunctions, IConfiguration configuration)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
            _dbEngine = new DbEngineClass(configuration);
        }
        [HttpGet]
        public IActionResult Index(string DataFlag)
        {
            var result = _dbFunctions.getPersondetails(DataFlag);
            dynamic firstDetail;
            firstDetail = new ExpandoObject();
            firstDetail.masterDetails = result;
            return Ok(firstDetail);
        }

        [HttpGet]
        public IActionResult Home(string id, string DataFlag)
        {
            var result = _dbFunctions.getPersonMasterbyId(Convert.ToInt32(id), DataFlag);
            dynamic firstDetail;

            if (result != null && result.Any())
            {
                firstDetail = result.First();
            }
            else
            {
                firstDetail = new ExpandoObject();
            }
            firstDetail.CountryList = _dbFunctions.GetCountries();
            firstDetail.CityList = _dbFunctions.GetCity();           
            firstDetail.bussinessTypesList = _dbFunctions.GetDonorBussinussType(DataFlag);
            firstDetail.EmployeeDetils = _dbFunctions.GetEmployeeDetils();
            firstDetail.PersonCatDetails=_dbFunctions.getPersoncatdetails(DataFlag);
            return Ok(firstDetail);
        }
        [HttpPost]
        public IActionResult InsertData([FromBody] PersonMaster model)
        {
            try
            {
                var result = _dbEngine.ExecuteInsertStoredProcedure("[InsertPersonMaster]", model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UpdateData([FromBody] PersonMaster model)
        {

            try
            {
                var result = _dbEngine.ExecuteUpdateStoredProcedure("[updatepersonmaster]", model.person_id, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }
    }
}
