using Microsoft.EntityFrameworkCore;

namespace KE_PDC.Models
{
    public class KE_PMGWContext : DbContext
    {
        public KE_PMGWContext(DbContextOptions<KE_PMGWContext> options)
            : base(options)
        { }

        public virtual DbSet<LINEPay> LINEPay { get; set; }
        public virtual DbSet<LINETopUp> LINETopUp { get; set; }
    }
}
