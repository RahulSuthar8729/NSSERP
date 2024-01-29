using Microsoft.EntityFrameworkCore;
using NSSERPAPI.Models;
namespace NSSERPAPI
{
    public class YourDbContext:DbContext
    {
        public YourDbContext(DbContextOptions<YourDbContext> options) : base(options)
        {
        }
       
    }
}
