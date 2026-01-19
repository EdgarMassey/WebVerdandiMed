using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebVerdandiMedReg.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<WebVerdandiMedReg.Data.Membership> Memberships => Set<WebVerdandiMedReg.Data.Membership>();

    }
}
