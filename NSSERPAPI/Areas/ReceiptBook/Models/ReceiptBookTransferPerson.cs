namespace NSSERPAPI.Areas.ReceiptBook.Models
{
    public class ReceiptBookTransferPerson
    {
        public int? ISNO { get; set; }
        public int? PID { get; set; }
        public int? OldPId { get; set; }
        public int? UserId { get; set; }
        public string? DataFlag { get; set; }
        public int? BookId { get; set; }
        public int? PNoFrom { get; set; }
        public int? PNoTo { get; set; }
        public string? TP { get; set; }
    }
}
