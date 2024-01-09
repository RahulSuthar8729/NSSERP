namespace NSSERP.Areas.Masters.Models
{
    public class CampaignMaster
    {
        public int REF_ID {  get; set; }
        public string CampaignName { get; set;}
        public char IsActive { get; set; }
        public string CreatedBy { get; set;}
    }
}
