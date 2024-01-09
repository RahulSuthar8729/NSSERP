using Microsoft.EntityFrameworkCore;
using NSSERP.Areas.Masters.Models;

namespace NSSERP
{
    public class YourDbContext : DbContext
    {
        public YourDbContext(DbContextOptions<YourDbContext> options) : base(options)
        {
        }
        public DbSet<Countrys>CountryMaster { get; set; }
      public DbSet<StateMaster>StateMaster { get; set; }
    }
}
