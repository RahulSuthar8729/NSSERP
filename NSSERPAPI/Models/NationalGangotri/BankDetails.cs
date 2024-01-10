namespace NSSERPAPI.Models.NationalGangotri
{
    public class BankDetails
    {
        public int BankID { get; set; }

        public string DonationMode { get; set; }

        public string BankName { get; set; }
        public string ChequeDate { get; set; }
        public string ChequeNo { get; set; }
        public int DepositBankID { get; set; }
        public string DepositBank { get; set; }
        public string DepositDate { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string PdcCheque { get; set; }


        public string? ChequeOrDraftDate { get; set; }

        public string ChequeOrDraftNo { get; set; }

        public int DepositeBankID { get; set; }

        public string DepositeBankName { get; set; }


        public string? DepositeDate { get; set; }

        public string CurrencyCode { get; set; }

        public string IsPdcCheque1 { get; set; }

    }
}
