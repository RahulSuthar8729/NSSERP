using NSSERPAPI.Areas.Masters.Models;

namespace NSSERPAPI.Areas.ReceiptBook.Models
{
    public class ReceiptBookCancellation
    {

        public int? receipt_book_no { get; set; }
        public string? PostChq { get; set; }
        public string? CancellationJson { get; set; }

        public int? PersonId { get; set; }


        public string? DataFlag { get; set; }         


        public string? TP { get; set; }

       

    }
}
