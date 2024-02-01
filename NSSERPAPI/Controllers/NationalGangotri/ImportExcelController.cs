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
using System.Globalization;
using System.Security.Claims;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            List<ImportExcel> ExceData = string.IsNullOrEmpty(model.ExcelData) ? new List<ImportExcel>() : JsonConvert.DeserializeObject<List<ImportExcel>>(model.ExcelData);
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Start a transaction
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            if (ExceData != null)
                            {
                                foreach (var Excel in ExceData)
                                {
                                    var Excellist = new DynamicParameters();
                                    Excellist.Add("@Bank_Name", Excel.Bank_Name);
                                    Excellist.Add("@DOE", Excel.TRDATE);
                                    Excellist.Add("@Particular", Excel.DESCRIPTION);
                                    Excellist.Add("@ChqNo", Excel.CHEQUENO);
                                    Excellist.Add("@DR", Excel.DR);
                                    Excellist.Add("@CR", Excel.CR);
                                    Excellist.Add("@BALANCE", Excel.BAL);
                                    Excellist.Add("@Curr_Rate", Excel.RATE);
                                    Excellist.Add("@Branch", Excel.BRANCH);

                                    connection.Execute("InsertBankStatementeFromExcel", Excellist, transaction, commandType: CommandType.StoredProcedure);
                                }
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                           
                            transaction.Rollback();
                            throw ex;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                
            }                   
            string msg = "Bank Statement Inserted Successfully";
            return Ok(msg);
          
        }
    }
}
