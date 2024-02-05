namespace NSSERPAPI.Models.NationalGangotri
{
    public class DepositeDetailsModel
    {
        public int? refNo {  get; set; }
   
        public string? DepositeBank { get; set; }
        public string TempDoc { get; set; }
        public string mode { get; set; } 
        public DateTime date { get; set; } 
        public string currencyCode { get; set; } 
        public decimal amount { get; set; } 
        public string bankID { get; set; }
        public string TrasactionID { get; set; }

    }
}
