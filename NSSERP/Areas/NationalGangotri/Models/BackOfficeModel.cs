using Microsoft.Extensions.Primitives;
using NSSERP.Areas.Masters.Models;

namespace NSSERP.Areas.NationalGangotri.Models
{
    public class BackOfficeModel
    {
   
        public string? msg { get; set; }
        public int? ReceiveID { get; set; }
        public DateTime? ReceiveDate { get; set; }

        public int? ReceiveHeadID { get; set; }
        public string? ReceiveHeadName { get; set; } = "";
        public string? NamePrefix { get; set; } = "";
        public string? FullName { get; set; }
        public int? ProvNo { get; set; }
        public DateTime? ProvDate { get; set; }
        public string? PersonName { get; set; } = "";
        public int? PaymentModeID { get; set; }
        public string? PaymentModeName { get; set; } = "";
        public int? CurrencyID { get; set; }
        public string? CurrencyCode { get; set; } = "";
        public decimal? TotalAmount { get; set; }
        public IFormFile DocPayInSlip { get; set; }
        public string Doc3 { get; set; }
        public string? TotalAmountInWords { get; set; }
        public List<PaymentModeMaster> paymentModeList { get; set; }
        public IEnumerable<CurrencyMaster> currenciesList { get; set; }
        public List<DMSMovementListItems> MovementDetils { get; set; }
        public IEnumerable<BankMaster> bankmasterlist { get; set; }
        public IEnumerable<DepositBankmaster> depositBankmaster {  get; set; }
        public class DepositBankmaster() {
            public int bank_code { get; set; }
            public string Bank_Name { get; set; }
            public string FullName { get; set; }            

        }

        public int BankID { get; set; }
        public string BankName { get; set; }
        public class DMSMovementListItems()
        {
            public int MovementID { get; set; }
            public string MovementFrom { get; set;}
            public string MovementTo { get; set;}
            public int UserID { get; set; }
            public string UserName { get; set; }
            public string Date {  get; set; }
        }
      
        public string BankDetailsList { get; set; }
        public string BankDetailsListJson { get; set; }
       public string DonationDetails { get; set; }
       public int? OrderTypeID { get; set; }
       public string OrderTypeName { get; set; }
       public string OrderNumber { get; set; }

        public List<OrderTypes> ordertypelist { get; set; } 
      
        public class OrderTypes()
        {
            public int OrderTypeID { get; set; }
            public string OrderTypeName { get; set; }
        }

        public List<PersonMaster> PersonDetails { get; set; }
        public List<EmployeMaster> EmployeeDetils { get; set; }
        public class PersonMaster()
        {
            public int PersonID { get; set; }
            public string PersonName { get; set; }
        }
        public class EmployeMaster()
        {
            public int EmpID { get; set; }
            public string EmpName { get; set; }
        }
    }
}
