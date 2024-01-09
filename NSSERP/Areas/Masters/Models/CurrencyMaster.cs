namespace NSSERP.Areas.Masters.Models
{
    public class CurrencyMaster
    {
        public int CurrencyID { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string IsActive { get; set; }
        public string Symbol { get; set; }
        public string CreatedBy { get; set; }
        public int CountryID { get; set;}
        public Countrys CountryMaster { get; set; }

    }
}
