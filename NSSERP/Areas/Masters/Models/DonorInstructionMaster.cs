namespace NSSERP.Areas.Masters.Models
{
    public class DonorInstructionMaster
    {
        public int REF_ID { get; set; }
        public string InstructionName { get; set; }
        public string CreatedBy { get; set; }
        public char IsActive { get; set; }
    }
}
