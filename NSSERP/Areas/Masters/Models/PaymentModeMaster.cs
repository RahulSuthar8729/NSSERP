namespace NSSERP.Areas.Masters.Models
{
    public class PaymentModeMaster
    {
        public int PaymentModeID { get; set; }
        public string PaymentModeName { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }      
        public string CreatedBy { get; set; }
    }
}
