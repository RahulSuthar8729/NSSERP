using NSSERP.Areas.ReceiptBook.Models;
using static NSSERP.Areas.NationalGangotri.Models.BackOfficeModel;
using System.Text.Json.Serialization;

public class ReceiptBookCancellation
{

    public List<ReceiptBookCancellation> ReceiptBookCancellationList { get; set; }

   
    public List<ReceiptBookCancellationListMasterById> ReceiptBookCancellationListMasterByIds { get; set; }

 
    public List<NSSERP.Areas.ReceiptBook.Models.PersonMaster> PersonDetails { get; set; }

    public List<CancellationReasons> CancellationReasonsList { get; set; }

 
    public List<EmployeMaster> EmployeeDetils { get; set; }

   
    public int? receipt_book_no { get; set; }

    public string? PostChq { get; set; }
    public string? CancellationJson { get; set; }


    public int? PersonId { get; set; }


    public string? DataFlag { get; set; } = "GANGOTRI";


    public string? msg { get; set; }


    public string? TP { get; set; }


    public string? receiptbook_type { get; set; }


    public int? receipt_from { get; set; }


    public int? receipt_to { get; set; }


    public DateTime? date_print { get; set; }

    public class CancellationReasons
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
    }
}