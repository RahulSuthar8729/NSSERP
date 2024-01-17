namespace NSSERPAPI.Models.NationalGangotri
{
    public class DepositeDetailsModel
    {
        public string mode { get; set; } 
        public DateTime date { get; set; } 
        public string currencyCode { get; set; } 
        public decimal amount { get; set; } 
        public string bankID { get; set; } 
    }
}
