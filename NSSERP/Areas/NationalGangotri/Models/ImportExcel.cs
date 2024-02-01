using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;

namespace NSSERP.Areas.NationalGangotri.Models
{
    public class ImportExcel
    {
        public string msg { get; set; }
        public string ExcelData {  get; set; }
        public string Bank_Name { get; set; }
        public DateTime TRDATE { get; set; }
        public string DESCRIPTION { get; set; }= string.Empty;
        public string CHEQUENO { get; set; }
        public decimal DR { get; set; }
        public decimal CR { get; set; }
        public decimal RATE { get; set; }
        public decimal BAL { get; set; }
        public string BRANCH { get; set; }            
        public IFormFile excelFile { get; set; }
    }
}
