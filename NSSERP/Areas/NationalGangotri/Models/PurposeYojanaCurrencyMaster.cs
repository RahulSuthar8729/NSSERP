using Newtonsoft.Json;

namespace NSSERP.Areas.NationalGangotri.Models
{
    public class PurposeYojanaCurrencyMaster
    {
  
        public IEnumerable<PurposeYojanaCurrencyMaster> masterDetails { get; set; }
        public IEnumerable<PurposeYojanaCurrencyMaster> CurrencyList { get; set; }
        public string? msg { get; set; }
        public  int? Purpose_id { get; set; }
        public  int? Currency_id { get; set; }
        public  int? Yojna_ID { get; set; }
        public decimal? Amount { get; set; }
        public string? DATA_FLAG { get; set; } = "GANGOTRI";
        public int? FY_ID { get; set; }    
        public string? CURRENCY { get; set; }
        public string? CURRENCY_SYMBOL { get; set; }       
        public string? CurrencyData { get; set; }
        public List<CurrencyAmount> CurrencyAmounts { get; set; }
        public List<currencyID> CurrencyIDs { get; set; }

        public class CurrencyAmount
        {
           
            public decimal? Amount { get; set; }
        }
public class currencyID
        {

            public int Currency_id { get; set; }
        }
    }
}
