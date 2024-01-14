using NSSERP.Areas.Masters.Models;

namespace NSSERP.Areas.NationalGangotri.Models
{
    public class DonationReceiveMasterDetails
    {
        public List<DonationReceiveMasterDetails> masterDetails { get; set; }
        public List<CityMaster> CityMasterList { get; set; }
        public List<StateMaster> statelist { get; set; }
        public List<PaymentModeMaster> paymentModes { get; set; }
        public int? PaymentModeID { get; set; }
        public int? ReceiveID { get; set; }
        public string? AppStatus { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string?  ProvNo { get; set; }
        public bool? IsReceiveHeadDiffrent { get; set; }
        public string? PaymentModeName { get; set; }
        public string? CurrencyCode { get; set; }
        public string? FullName { get; set; }
        public string? FullAddress { get; set; }
        public string? PinCode { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public int? StateID { get; set; }
        public string? StateName { get; set; }
        public int? DistrictID { get; set; }
        public string? DistrictName { get; set; }
        public int? CityID { get; set; }
        public string? CityName { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool? IfUpdationInAddress { get; set; }
        public bool? IfDetailsNotComplete { get; set; }
        public bool? IfAnnounceDueInFuture { get; set; }
        public int? ReceiveHeadID { get; set; }
        public string? ReceiveHeadName { get; set; }
        public string? CreatedBy { get; set; }
        public int? MaterialID { get; set; }
    }
}
