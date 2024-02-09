﻿using NSSERP.Areas.AdminSetup.Controllers;
using NSSERP.Areas.Masters.Models;

namespace NSSERP.Areas.NationalGangotri.Models
{
    public class SankalpSiddhiDetails
    {
        public string MapWithBankIDList { get; set; }
        public string msg { get; set; }
        public List<DonationReceiveMasterDetails> masterDetails { get; set; }
        public List<CityMaster> CityMasterList { get; set; }
        public List<StateMaster> statelist { get; set; }
        public List<PaymentModeMaster> paymentModes { get; set; }
        public IEnumerable<DepositBankmaster> DepositBankList { get; set; }
        public IEnumerable<BankStatement> BankStatementsList { get; set; }
        public class DepositBankmaster
        {
            public int? bank_code { get; set; }
            public string? Bank_Name { get; set; }
            public string? FullName { get; set; }
        }

        public class BankStatement
        {
            public int? BANK_Code { get; set; }
            public DateTime? DOE { get; set; }
            public string? Bank_Name { get; set; }
            public string? Particular { get; set; }
            public string? ChqNo { get; set; }
            public decimal? DR { get; set; }
            public decimal? CR { get; set; }
            public decimal? BALANCE { get; set; }
            public decimal? Curr_Rate { get; set; }
            public string? Branch { get; set; }
            public string? DataFlag { get; set; }
            public int? ReceiveID { get; set; }
            public decimal? ReceiveAmt { get; set; }
        }

        public string DepositeDetailsListJson { get; set; }
        public int? PaymentModeID { get; set; }
        public int? ReceiveID { get; set; }
        public int? ReceiveIDOnUpdate { get; set; }
        public string? AppStatus { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string? ProvNo { get; set; }
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

        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        public int? BANK_Code { get; set; }
        public int? BankID { get; set; }
        public DateTime? DOE { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? Bank_Name { get; set; }
        public string? Particular { get; set; }
        public string? ChqNo { get; set; }
        public decimal? DR { get; set; }
        public decimal? CR { get; set; }
        public decimal? BALANCE { get; set; }
        public decimal? Curr_Rate { get; set; }
        public string? Branch { get; set; }
        public string? MobileNo { get; set; }
        public string? DataFlag { get; set; } 
        public decimal? ReceiveAmt { get; set; }

    }
}
