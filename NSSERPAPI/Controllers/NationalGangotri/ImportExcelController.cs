using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSSERPAPI.Db_functions_for_Gangotri;
using NSSERPAPI.Models.NationalGangotri;
using System;

namespace NSSERPAPI.Controllers.NationalGangotri
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ImportExcelController : ControllerBase
    {
        private readonly Db_functions _dbFunctions;
        private readonly string _connectionString;

        public ImportExcelController(Db_functions dbFunctions, IConfiguration configuration)
        {
            _dbFunctions = dbFunctions ?? throw new ArgumentNullException(nameof(dbFunctions));
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpPost]
        public IActionResult InsertExcelData([FromBody] ImportExcel model)
        {
            // Validate model and perform data insertion logic

            // Assuming you want to return a success message
            return Ok(new { Message = "Data inserted successfully." });
        }
    }
}
