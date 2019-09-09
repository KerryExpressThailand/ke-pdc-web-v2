using KE_PDC.Models.SevicePoint;
using Microsoft.EntityFrameworkCore;

namespace KE_PDC.Models
{
    public class KE_RTSPContext : DbContext
    {
        public KE_RTSPContext(DbContextOptions<KE_RTSPContext> options)
            : base(options)
        { }

        public virtual DbSet<DailyCommission> DailyCommission { get; set; }
        public virtual DbSet<ProfileMaster> ProfileMaster { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DailyCommission>(entity =>
            {
                entity.HasKey(e => new { e.ProfileId, e.ReportDate });

                entity.Property(e => e.ProfileId)
                    .HasColumnName("profile_id");

                entity.Property(e => e.ReportDate)
                    .HasColumnName("report_date");

                entity.Property(e => e.ProfileName)
                    .HasColumnName("profile_name");

                entity.Property(e => e.BranchId)
                    .HasColumnName("BranchID");

                entity.Property(e => e.Consignment)
                    .HasColumnName("Con");

                entity.Property(e => e.Boxes)
                    .HasColumnName("Boxes");

                entity.Property(e => e.Cash)
                    .HasColumnName("Cash");

                entity.Property(e => e.Commission)
                    .HasColumnName("commission");

                entity.Property(e => e.Verified)
                    .HasColumnName("verify_status");
            });

            modelBuilder.Entity<ProfileMaster>(entity =>
            {
                entity.ToTable("tb_profile_master");

                entity.Property(e => e.ProfileId)
                    .HasColumnName("profile_id");

                entity.Property(e => e.MasterId)
                    .HasColumnName("master_id");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("project_id");

                entity.Property(e => e.KESAccountId)
                    .HasColumnName("kes_account_id");

                entity.Property(e => e.POSAccountId)
                    .HasColumnName("pos_account_id");

                entity.Property(e => e.ReferenceCode)
                    .HasColumnName("reference_code");

                entity.Property(e => e.OriginDC)
                    .HasColumnName("origin_dc");

                entity.Property(e => e.ZoneId)
                    .HasColumnName("zone_id");

                entity.Property(e => e.OwnerId)
                    .HasColumnName("owner_id");

                entity.Property(e => e.ProfileName)
                    .HasColumnName("profile_name");

                entity.Property(e => e.ProfileAddress1)
                    .HasColumnName("profile_address1");

                entity.Property(e => e.ProfileAddress2)
                    .HasColumnName("profile_address2");

                entity.Property(e => e.ProfileZipcode)
                    .HasColumnName("profile_zipcode");

                entity.Property(e => e.ProfileMobile1)
                    .HasColumnName("profile_mobile1");

                entity.Property(e => e.ProfileMobile2)
                    .HasColumnName("profile_mobile2");

                entity.Property(e => e.ProfilePerson)
                    .HasColumnName("profile_person");

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude");

                entity.Property(e => e.CommissionRate)
                    .HasColumnName("commission_rate");

                entity.Property(e => e.CutoffTime)
                    .HasColumnName("cutoff_time");

                entity.Property(e => e.MobilePrinter)
                    .HasColumnName("mobile_printer");

                entity.Property(e => e.SynchronizeKES)
                    .HasColumnName("synchronize_kes");

                entity.Property(e => e.EnableScheduler)
                    .HasColumnName("enable_scheduler");

                entity.Property(e => e.ProfileArchetype)
                    .HasColumnName("profile_archetype");

                entity.Property(e => e.TaxIdentification)
                    .HasColumnName("tax_identification");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("updated_date");

                entity.Property(e => e.DeletedBy)
                    .HasColumnName("deleted_by");

                entity.Property(e => e.DeletedDate)
                    .HasColumnName("deleted_date");

                entity.Property(e => e.DeletedDesc)
                    .HasColumnName("deleted_desc");

            });
        }
    }
}
