using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FPTV.Data
{
    public class FPTVContext : IdentityDbContext
    {
        public FPTVContext(DbContextOptions<FPTVContext> options)
            : base(options)
        {
        }
    }
}