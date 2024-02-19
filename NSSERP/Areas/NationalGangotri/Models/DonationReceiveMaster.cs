using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NSSERP.Areas.Masters.Models;
using System.ComponentModel.DataAnnotations;
using static NSSERP.Areas.NationalGangotri.Models.BackOfficeModel;

namespace NSSERP.Areas.NationalGangotri.Models
{
    public class DonationReceiveMaster
    {
        public string TempFilename { get; set; }
        public string msg { get; set; }
        public bool ischekecked { get; set; }
        public int? PaymentModeID { get; set; }
        public string PaymentModeName { get; set; } = "";
        public int? CurrencyID { get; set; }
        public string? CurrencyCode { get; set; } 

        public int? HeadID { get; set; }
        public string HeadName { get; set; } = "";
         public int? Purpose_ID { get; set; }
        public string Purpose { get; set; } = "";
        public int? ID { get; set; }
        public bool? IsReceiveHeadDiffrent { get; set; }
        
        public int? ReceiveID { get; set; }
        public int? ReceiveHeadID { get; set; }
        public string ReceiveHeadName { get; set; } = "";

        public string DataFlag { get; set; }

        public DateTime ReceiveDate { get; set; }

        public string RBType { get; set; }
        public List<ReceiptBookType> ReceiptBookTypeList { get; set; }
        public class ReceiptBookType()
        {
            public string RB_Type_Code { get; set; }
            public string RB_Type { get; set; }
        }

        public DonationReceiveMaster()
        {
            IsReceiveHeadDiffrent = false;
            ReceiveDate = DateTime.Now;           
            IsManavaFormulaRequire = false;
            IsPatientsPhotoRequire=false;
        }
        public int? CampaignID { get; set; }
        public string CampaignName { get; set; }

        public string InMemory { get; set; } = "";
        public int? DonorID { get; set; } 
        public string NamePrefix { get; set; } = "";
        public string FullName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string PrefixToFullName { get; set; }
        public string RelationToFullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Company { get; set; } = "";
        public string FullAddress { get; set; } = "";
        public string Address1 { get; set; } = "";
        public string Address2 { get; set; } = "";
        public string Address3 { get; set; } = "";
        public string PinCode { get; set; } = "";
        public int? CountryId { get; set; }
        public string CountryName { get; set; } = "";
        public int? StateID { get; set; }
        public string StateName { get; set; } = "";
        public int? DistrictID { get; set; }
        public string DistrictName { get; set; } = "";
        public int? CityID { get; set; }
        public string CityName { get; set; } = "";

        public bool? IfUpdationInAddress { get; set; }
        public bool? IsPermanentAddressDiff { get; set; }
        public bool?  IfDetailsNotComplete { get; set; }

        public string P_FullAddress { get; set; } = "";
        public string P_Address1 { get; set; } = "";
        public string P_Address2 { get; set; } = "";
        public string P_Address3 { get; set; } = "";
        public string P_PinCode { get; set; } = "";

        public int? P_CountryID { get; set; }
        public string P_CountryName { get; set; } = "";
        public int? P_StateID { get; set; }
        public string P_StateName { get; set; } = "";
        public int? P_DistrictID { get; set; }
        public string P_DistrictName { get; set; } = "";
        public int? P_CityID { get; set; }
        public string P_CityName { get; set; } = "";

        public string MobileList { get; set; } = "";

        public string EmailID { get; set; } = "";
        public string StdCode { get; set; } = "";
        public string PhoneR { get; set; } = "";

        public string IdentityList { get; set; } = "";

        public string BankDetailsList { get; set; } = "";
        public int? ProvNo { get; set; }
        public DateTime? ProvDate { get; set; }

        public string PersonName { get; set; } = "";
        public string EventName { get; set; } = "";
        public decimal Amount { get; set; }
        public decimal? TotalAmount { get; set; }
        public string MaterialDepositID { get; set; } = "";
        public  int? OrderTypeID { get; set; }
        public string? OrderTypeName { get; set; }
        public string OrderNumber { get; set; }
        public List<OrderTypes> ordertypelist { get; set; }
        public class OrderTypes()
        {
           public int OrderTypeID { get; set; }
            public string OrderTypeName { get; set; }
        }
        public string Material { get; set; } = "";
        public bool? IsManavaFormulaRequire { get; set; }
        public bool? IsPatientsPhotoRequire { get; set; }
        public bool? IfDiffrentAddressForDispatch { get; set; }
        public string DifferentAddressToDispatch { get; set; } = "";

        public int? SubHeadID { get; set; }
        public string SubHeadName { get; set; } = "";
        public string Instructions { get; set; } = "";
        public string ReceiptRemarks { get; set; } = "";
        public bool IfAnnounceDueInFuture { get; set; }
        public List<IFormFile> DocProvisonal { get; set; }        
        public IFormFile DocCheque { get; set; }
        public IFormFile DocPayInSlip { get; set; }
        public string Doc1 { get; set; } = "";
        public string Doc2 { get; set; } = "";
        public string Doc3 { get; set; } = "";
        public string receiptdetailslist { get; set; } = "";
        public string AnnounceDetsilsList { get; set; } = "";
        public string donorInstructionjsonList { get; set; } = "";      
        public List<Countrys> CountryList { get; set; }
        public IEnumerable<CampaignMaster> campaignlist { get; set; }
        public IEnumerable<DonorInstructionMaster> donorInstructionList { get; set; }
        public IEnumerable<PaymentModeMaster> paymentModeList { get; set; }
        public IEnumerable<CurrencyMaster> currenciesList { get; set; }
        public IEnumerable<BankMaster> bankmasterlist { get; set; }
        public IEnumerable<DonationReceiveMaster> ReceiveHeadList { get; set; }
        public IEnumerable<DonationReceiveMaster> HeadList { get; set; }
        public IEnumerable<DonationReceiveMaster> SubHeadList { get; set; }
        public IEnumerable<DonationReceiveMaster> ReceiveInEventList { get; set; }
        public IEnumerable<DepositBankmaster> depositBankmaster { get; set; }

        public int? ReceiveInEventID { get; set; }
        public string? ReceiveInEventName { get; set; }
       public string pinCodeMasterList { get; set; } = "";

        public string DonationDetails { get; set; } = "";
            
        public string MobileListJson { get; set; } = "";

        public string IdentityListJson { get; set; } = "";

        public string BankDetailsListJson { get; set; } = "";
            
        public string ReceiptsListJson { get; set; } = "";

        public string DonorInstructionsListJson { get; set; } = "";

        public string AnnounceDetailsListJson { get; set; } = "";

        public string DepositeDetailsListJson { get; set; }

        public string MovementMasterListJson { get; set; }
    }
}
