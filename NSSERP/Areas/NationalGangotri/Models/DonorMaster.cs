using NSSERP.Areas.Masters.Models;
using System.Threading.Channels;

namespace NSSERP.Areas.NationalGangotri.Models
{
    public class DonorMaster
    {
        public string DataFlag { get; set; }
        public bool IsAnonymous { get; set; }
        public int DonorID { get; set; }
        public string CustId { get; set; }
        public string UpiID { get; set; }
        public string MailingNo { get; set; }

        public string ReferenceNo { get; set; }
        public DateTime DOE { get; set; }
        public string DonorCat { get; set; }
        public bool IsAppShreekaPurnVivranReceived { get; }

        public string NamePrefix { get; set; } = "";
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string PrefixToRelation { get; set; }
        public string RelationToFullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfAnniversary { get; set; }
        public string Company { get; set; } = "";
        public string YourCompany { get; set; } = "";
        public string BussinessOrJobType { get; set; } = "";
        public string Profession { get; set; } = "";
        public string WorkingIn { get; set; } = "";
        public string Designation { get; set; } = "";
        public string CareOf { get; set; } = "";
        public decimal ToatlDonation { get; set; }
        public string Address1 { get; set; } = "";
        public string Address2 { get; set; } = "";
        public string Address3 { get; set; } = "";
        public string PinCode { get; set; } = "";
        public int? CountryId { get; set; }
        public string CountryName { get; set; } = "";
        public List<Countrys> CountryList { get; set; }
        public int? StateID { get; set; }
        public string StateName { get; set; } = "";
        public int? DistrictID { get; set; }
        public string DistrictName { get; set; } = "";
        public int? CityID { get; set; }
        public string CityName { get; set; } = "";
        public IEnumerable<Citys> CityList { get; set; }

        public class Citys()
        {
            public int CityID { get; set; }
            public string CityName { get; set; }
        }
        public bool? IsPermanentAddressDiff { get; set; }
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

        public bool IsCallActive { get; set; } = true;

        public bool IsMsgActive { get; set; } = true;
        public bool IsWhatsAppActive { get; set; } = true;
        public bool IsEmailActive { get; set; }=true;
        public bool IsLetterCommunicationActive { get; set; }=true;
        public bool IsSendoperationPhotoActive { get; set; } = true;
        public string MobileList { get; set; } = "";
        public string IdentityList { get; set; } = "";

        public string MobileListJson { get; set; } = "";

        public string IdentityListJson { get; set; } = "";

        public bool IsSandipanSend { get; set; }
        public string Languages { get; set; } = "";
        public string Sandipan { get; set; } = "";
        public string SandipanRemarks { get; set; } = "";
        public string ReceiptCopyRequireOptions { get; set; } = "";

        public bool IsVisit { get; set; }
        public string VisitYear { get; set; } = "";
        public string ForginNgCodeRefrence { get; set; } = "";
        public string ChangesRemarks { get; set; } = "";
        public string Remarks { get; set; } = "";
    }
}
