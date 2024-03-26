namespace NSSERPAPI.Areas.ReceiptBook.Models
{
    public class InsertDocFileInfo
    {
        public int? Id { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public string? DataFlag { get; set; }
        public string? FileType { get; set; }
        public int? FYID { get; set; }       
    }
}
