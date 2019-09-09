using Microsoft.EntityFrameworkCore;

namespace KE_PDC.Models
{
    public class KE_CMSContext : DbContext
    {
        public KE_CMSContext(DbContextOptions<KE_CMSContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
