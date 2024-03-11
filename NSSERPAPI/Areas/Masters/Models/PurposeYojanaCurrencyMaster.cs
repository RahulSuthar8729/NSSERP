namespace NSSERPAPI.Areas.Masters.Models
{
    public class PurposeYojanaCurrencyMaster
    {
        public int? Purpose_id { get; set; }
        public int? Currency_id { get; set; }
        public int? Yojna_ID { get; set; }
        public decimal? Amount { get; set; }
        public string? DATA_FLAG { get; set; }
        public int? FY_ID { get; set; } = 0;
        public string? CurrencyData { get; set; }
    }
}
