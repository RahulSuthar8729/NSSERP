namespace NSSERPAPI.Models.NationalGangotri
{
    public class ImportExcel
    {
        public string ExcelData { get; set; }
        public string? Bank_Name { get; set; }
        public DateTime? TRDATE { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? CHEQUENO { get; set; }
        public decimal? DR { get; set; }
        public decimal? CR { get; set; }
        public decimal? RATE { get; set; }
        public decimal? BAL { get; set; }
        public string? BRANCH { get; set; }
    }
}
