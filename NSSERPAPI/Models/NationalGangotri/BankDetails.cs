using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;
using System.Runtime.Serialization;

namespace NSSERPAPI.Models.NationalGangotri
{
    public class BankDetails
    {
        public int BankID { get; set; }
        public string DonationMode { get; set; }
        public string BankName { get; set; }       
        public DateTime ChequeDate { get; set; }
        public string ChequeNo { get; set; }
        public int DepositBankID { get; set; }
        public string DepositBank { get; set; } 
        public DateTime DepositDate { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string PdcCheque { get; set; }

    }
}
