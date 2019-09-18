using KE_PDC.Models.POS;
using KE_PDC.Models.POS.SevicePoint;
using KE_PDC.Models.POS.Stock;
using Microsoft.EntityFrameworkCore;

namespace KE_PDC.Models
{
    public class KE_POSContext : DbContext
    {
        public KE_POSContext()
        {
            Database.SetCommandTimeout(150000);
        }

        public KE_POSContext(DbContextOptions<KE_POSContext> options)
            : base(options)
        { }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserGroup> UserGroup { get; set; }
        public virtual DbSet<UserMenu> UserMenu { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }
        public virtual DbSet<UserShop> UserShop { get; set; }
        public virtual DbSet<AuthUser> AuthUser { get; set; }
        public virtual DbSet<UserMenuRole> UserMenuRole { get; set; }
        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<BranchType> BranchType { get; set; }
        public virtual DbSet<BranchTypes> BranchTypes { get; set; }
        public virtual DbSet<BranchList> BranchList { get; set; }
        public virtual DbSet<CloseShop> CloseShop { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }


        public virtual DbSet<Region> Region { get; set; }

        public virtual DbSet<DailyRevenue> DailyRevenue { get; set; }
        public virtual DbSet<DailyRevenueVerify> DailyRevenueVerify { get; set; }
        public virtual DbSet<DailyRevenueConfirm> DailyRevenueConfirm { get; set; }
        public virtual DbSet<DailyRevenueConfirmFC> DailyRevenueConfirmFC { get; set; }
        public virtual DbSet<DailyRevenuePivot> DailyRevenuePivot { get; set; }

        public virtual DbSet<SaveDBResponse> SaveDBResponse { get; set; }
        public virtual DbSet<DailyRevenueDetail> DailyRevenueDetail { get; set; }
        public virtual DbSet<DailyRevenueDetailCash> DailyRevenueDetailCash { get; set; }
        public virtual DbSet<ShopDailyRevenue> ShopDailyRevenue { get; set; }
        public virtual DbSet<Receipt> Receipt { get; set; }
        public virtual DbSet<TaxInvoice> TaxInvoice { get; set; }
        public virtual DbSet<MonthlyCommission> MonthlyCommission { get; set; }
        public virtual DbSet<MonthlySummaryCommission> MonthlySummaryCommission { get; set; }
        public virtual DbSet<DailyCommission> DailyCommission { get; set; }
        public virtual DbSet<LINEPayRemittance> LINEPayRemittance { get; set; }
        public virtual DbSet<MonthlyExpense> MonthlyExpense { get; set; }
        public virtual DbSet<MonthlyExpenseDetail> MonthlyExpenseDetail { get; set; }
        public virtual DbSet<Package> Package { get; set; }
        public virtual DbSet<StockOrder> StockOrder { get; set; }
        public virtual DbSet<StockStatus> StockStatus { get; set; }
        public virtual DbSet<EOD> EOD { get; set; }
        public virtual DbSet<DHLVerify> DHLVerify { get; set; }
        public virtual DbSet<NoneShipmentDHL> NoneShipmentDHL { get; set; }
        public virtual DbSet<DailyRevenueDHL> DailyRevenueDHL { get; set; }
        public virtual DbSet<ReviewBalanceReport> ReviewBalanceReport { get; set; }
        public virtual DbSet<DailyCOD> DailyCOD { get; set; }
        public virtual DbSet<DailyCODDetail> DailyCODDetail { get; set; }
        public virtual DbSet<DashboardShopDaily> DashboardShopDaily { get; set; }
        public virtual DbSet<CashReport> CashReport { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<LinePayInvoice> LinePayInvoice { get; set; }
        public virtual DbSet<LinePayTopup> LinePayTopup { get; set; }
        public virtual DbSet<BoxRevenue> BoxRevenue { get; set; }
        public virtual DbSet<MDEConsignments> MDEConsignments { get; set; }
        public virtual DbSet<MDEPackages> MDEPackages { get; set; }
        public virtual DbSet<BranchList_r> BranchList_r { get; set; }
        public virtual DbSet<Discount> Discount { get; set; }
        public virtual DbSet<SevicePointDailyRevenue> SevicePointDailyRevenue { get; set; }
        public virtual DbSet<Insurance> Insurance { get; set; }

        public virtual DbSet<ReconcileSummaryMaster> ReconcileSummaryMaster { get; set; }
        public virtual DbSet<DailyRevenueReconcileBillPayment> DailyRevenueReconcileBillPayment { get; set; }
        public virtual DbSet<DailyRevenueReconcileCards> DailyRevenueReconcileCards { get; set; }
        public virtual DbSet<DailyRevenueReconcileLinePay> DailyRevenueReconcileLinePay { get; set; }
        public virtual DbSet<DailyRevenueReconcileQrPayment> DailyRevenueReconcileQrPayment { get; set; }

        public virtual DbSet<GetBatch> GetBatch { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("tb_master_user");

                entity.Property(e => e.Username)
                    .HasColumnName("userid")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.Password)
                    .HasColumnName("pwd")
                    .HasColumnType("varchar(50)");

                //entity.Property(e => e.CreatedAt)
                //    .HasColumnName("created_date")
                //    .HasColumnType("datetime");

                //entity.Property(e => e.CreatedBy)
                //    .HasColumnName("created_by")
                //    .HasColumnType("varchar(20)");

                //entity.Property(e => e.UpdatedAt)
                //    .HasColumnName("updated_date")
                //    .HasColumnType("datetime");

                //entity.Property(e => e.UpdatedBy)
                //    .HasColumnName("updated_by")
                //    .HasColumnType("varchar(20)");

                entity.Property(e => e.DefaultShopId)
                    .HasColumnName("default_shop_id")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Name)
                    .HasColumnName("username")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .HasColumnType("int");

                entity.Property(e => e.UserGroupId)
                    .HasColumnName("user_group_id")
                    .HasColumnType("int");

                entity.Property(e => e.ProfileId)
                    .HasColumnName("profile_id")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.email)
                   .HasColumnName("email")
                   .HasColumnType("varchar(200)");

                entity.Property(e => e.LastLogin)
                    .HasColumnName("last_login")
                    .HasColumnType("datetime");

                //entity.HasOne(p => p.UserGroup)
                //    .WithOne(i => i.User)
                //    .HasForeignKey<User>(b => b.UserGroupId);

                //entity.HasOne(p => p.UserMenu)
                //    .WithMany()
                //    .HasForeignKey<UserMenu>(b => b.Username);

                //entity.HasOne(p => p.UserProfile)
                //    .WithOne(i => i.User)
                //    .HasForeignKey<User>(b => b.ProfileId);
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("tb_master_region");

                entity.Property(e => e.RegionId)
                    .HasColumnName("region_id")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.RegionName)
                    .HasColumnName("region_name")
                    .HasColumnType("varchar(50)");
            });
            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.ToTable("tb_master_user_group");

                entity.Property(e => e.UserGroupId)
                    .HasColumnName("user_group_id")
                    .HasColumnType("int");

                entity.Property(e => e.UserGroupName)
                    .HasColumnName("user_group_name")
                    .HasColumnType("varchar(250)");
            });

            modelBuilder.Entity<UserMenu>(entity =>
            {
                entity.ToTable("tb_pdc_mapping_user_menu");

                entity.HasKey(e => new { e.Username, e.MenuId })
                    .HasName("PK_tb_pdc_mapping_user_menu");

                entity.Property(e => e.Username)
                    .HasColumnName("userid")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.MenuId)
                    .HasColumnName("menu_id")
                    .HasColumnType("uniqueidentifier");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasColumnType("varchar(20)");
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.ToTable("tb_master_user_profile");

                entity.Property(e => e.ProfileId)
                    .HasColumnName("profile_id")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.ProfileName)
                    .HasColumnName("profile_name")
                    .HasColumnType("nvarchar(150)");

                entity.Property(e => e.ProfileOwner)
                    .HasColumnName("profile_owner")
                    .HasColumnType("nvarchar(100)");

                entity.Property(e => e.ProfileCompany)
                    .HasColumnName("profile_company")
                    .HasColumnType("nvarchar(150)");

                entity.Property(e => e.ProfileTaxId)
                    .HasColumnName("profile_tax_id")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.ProfileIncludeVat)
                    .HasColumnName("profile_include_vat")
                    .HasColumnType("bit");

                entity.Property(e => e.ProfileAccountNo)
                    .HasColumnName("profile_account_no")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.ProfileBillAddress)
                    .HasColumnName("profile_bill_address")
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.ProfileEmail1)
                    .HasColumnName("profile_email1")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.ProfileEmail2)
                    .HasColumnName("profile_email2")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.ProfileEmail3)
                    .HasColumnName("profile_email3")
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<UserShop>(entity =>
            {
                entity.ToTable("tb_master_user_shop");

                entity.HasKey(e => new { e.Username, e.ShopId })
                    .HasName("PK_tb_user_shop");

                entity.Property(e => e.Username)
                    .HasColumnName("userid")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.ShopId)
                    .HasColumnName("shop_id")
                    .HasColumnType("varchar(10)");
            });
            #endregion

            #region Branch
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.ToTable("tb_Branch");
                entity.Property(e => e.BranchId).HasColumnName("BranchID");
                entity.Property(e => e.BranchName).HasColumnName("BranchName");
                entity.Property(e => e.ErpId).HasColumnName("ERP_ID");
                entity.Property(e => e.ReceiptBranchName).HasColumnName("receipt_branchName");
                entity.Property(e => e.ContactPerson).HasColumnName("ContactPerson");
                entity.Property(e => e.HomeAddress).HasColumnName("HomeAddress");
                entity.Property(e => e.Road).HasColumnName("Road");
                entity.Property(e => e.District).HasColumnName("District");
                entity.Property(e => e.Amphur).HasColumnName("Amphur");
                entity.Property(e => e.Province).HasColumnName("Province");
                entity.Property(e => e.PostalCode).HasColumnName("PostalCode");
                entity.Property(e => e.PostalCodeId).HasColumnName("PostalCodeID");
                entity.Property(e => e.Telephone).HasColumnName("Telephone");
                entity.Property(e => e.Mobile1).HasColumnName("Mobile1");
                entity.Property(e => e.Mobile2).HasColumnName("Mobile2");
                entity.Property(e => e.Email).HasColumnName("Email");
                entity.Property(e => e.ConsignmentPrefix).HasColumnName("ConsignmentPrefix");
                entity.Property(e => e.RegisterKey).HasColumnName("RegisterKey");
                entity.Property(e => e.SenderName).HasColumnName("Sender_Name");
                entity.Property(e => e.SenderContactPerson).HasColumnName("Sender_ContactPerson");
                entity.Property(e => e.SenderHomeAddress).HasColumnName("Sender_HomeAddress");
                entity.Property(e => e.SenderRoad).HasColumnName("Sender_Road");
                entity.Property(e => e.SenderDistrict).HasColumnName("Sender_District");
                entity.Property(e => e.SenderAmphur).HasColumnName("Sender_Amphur");
                entity.Property(e => e.SenderProvince).HasColumnName("Sender_Province");
                entity.Property(e => e.SenderPostalCode).HasColumnName("Sender_PostalCode");
                entity.Property(e => e.SenderPostalCodeId).HasColumnName("Sender_PostalCodeID");
                entity.Property(e => e.SenderMobile1).HasColumnName("Sender_Mobile1");
                entity.Property(e => e.SenderMobile2).HasColumnName("Sender_Mobile2");
                entity.Property(e => e.SenderEmail).HasColumnName("Sender_Email");
                entity.Property(e => e.CompanyName).HasColumnName("CompanyName");
                entity.Property(e => e.TaxId).HasColumnName("TaxID");
                entity.Property(e => e.Dmsid).HasColumnName("DMSID");
                entity.Property(e => e.FcVatable).HasColumnName("fc_vatable");
                entity.Property(e => e.EdiUsername).HasColumnName("edi_username");
                entity.Property(e => e.EdiPassword).HasColumnName("edi_password");
                entity.Property(e => e.ApiKey).HasColumnName("api_key");
                entity.Property(e => e.OperatingDatetime).HasColumnName("operating_datetime");
                entity.Property(e => e.TaxBranchName).HasColumnName("tax_branch_name");
                entity.Property(e => e.TaxCompanyName).HasColumnName("tax_CompanyName");
                entity.Property(e => e.TaxAddress1).HasColumnName("tax_Address1");
                entity.Property(e => e.TaxAddress2).HasColumnName("tax_Address2");
                entity.Property(e => e.TaxPostalCode).HasColumnName("tax_PostalCode");
                entity.Property(e => e.TaxTelephone).HasColumnName("tax_Telephone");
                entity.Property(e => e.TaxTaxId).HasColumnName("tax_TaxID");
                entity.Property(e => e.Note2).HasColumnName("note2");
                entity.Property(e => e.TaxInvoiceNoFromServer).HasColumnName("taxInvoiceNo_From_Server");
                entity.Property(e => e.LastUpdate).HasColumnName("last_update");
                entity.Property(e => e.EnableUpdate).HasColumnName("enable_update");
                entity.Property(e => e.DisplaySequence).HasColumnName("display_sequence");
                entity.Property(e => e.TaxAbb).HasColumnName("tax_abb");
                entity.Property(e => e.LinepayEnable).HasColumnName("Linepay_enable");
                entity.Property(e => e.DatabaseVersionNo).HasColumnName("database_version_no");
                entity.Property(e => e.DatabaseVersionName).HasColumnName("database_version_name");
                entity.Property(e => e.DatabaseEdition).HasColumnName("database_edition");
                entity.Property(e => e.DatabaseDataMb).HasColumnName("database_data_mb");
                entity.Property(e => e.DatabaseLogMb).HasColumnName("database_log_mb");
                entity.Property(e => e.DatabaseDescription).HasColumnName("database_description");
                entity.Property(e => e.DropOffEnable).HasColumnName("DropOff_enable");
                entity.Property(e => e.BranchType).HasColumnName("branch_type");
                entity.Property(e => e.SendToKes).HasColumnName("send_to_kes");
                entity.Property(e => e.CodEnable).HasColumnName("cod_enable");
                entity.Property(e => e.CreditEnable).HasColumnName("credit_enable");
                entity.Property(e => e.SelfcollectionEnable).HasColumnName("selfcollection_enable");
                entity.Property(e => e.SclConsumerReceive).HasColumnName("scl_consumer_receive");
                entity.Property(e => e.SamplingEnable).HasColumnName("sampling_enable");
                entity.Property(e => e.ConnonLimit).HasColumnName("connon_limit");
                entity.Property(e => e.RegionId).HasColumnName("region_id");
                entity.Property(e => e.CommissionRate).HasColumnName("commission_rate");
                entity.Property(e => e.OriginDc).HasColumnName("origin_dc");
                entity.Property(e => e.LastUpdateService).HasColumnName("last_update_service");
                entity.Property(e => e.ProcessSequence).HasColumnName("process_sequence");
                entity.Property(e => e.CutoffTime).HasColumnName("cutoff_time");
                entity.Property(e => e.FirstOpenDate).HasColumnName("first_open_date");
                entity.Property(e => e.ReceiptText_1).HasColumnName("receipt_text_1");
                entity.Property(e => e.ReceiptText_2).HasColumnName("receipt_text_2");
                entity.Property(e => e.ReceiptText_3).HasColumnName("receipt_text_3");
                entity.Property(e => e.ReceiptText_4).HasColumnName("receipt_text_4");
                entity.Property(e => e.ReceiptText_5).HasColumnName("receipt_text_5");
                entity.Property(e => e.PsaSyncDate).HasColumnName("PSA_Sync_Date");
                entity.Property(e => e.PdcSyncDate).HasColumnName("PDC_Sync_Date");
                entity.Property(e => e.AwsSyncDate).HasColumnName("AWS_Sync_Date");
                entity.Property(e => e.VgiEnable).HasColumnName("vgi_enable");
            });
            #endregion


            modelBuilder.Entity<AuthUser>(entity =>
            {
                entity.ToTable("tb_auth_user");

                entity.Property(e => e.user_id)
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.password)
                    .HasColumnName("pass")
                    .HasColumnType("varbinary(MAX)");

                entity.Property(e => e.user_name)
                   .HasColumnName("user_name")
                   .HasColumnType("varchar(100)");

                entity.Property(e => e.created_date)
                   .HasColumnName("created_date")
                   .HasColumnType("datetime");

                entity.Property(e => e.updated_date)
                    .HasColumnName("updated_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.updated_by)
                    .HasColumnName("updated_by")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.last_login)
                    .HasColumnName("last_login")
                    .HasColumnType("datetime");

                entity.Property(e => e.email)
                    .HasColumnName("email")
                    .HasColumnType("varchar(100)");

                //entity.Property(e => e.status)
                //.HasColumnName("status")
                //.HasColumnType("bit");

            });

            modelBuilder.Entity<DailyRevenue>(entity =>
            {
                entity.ToTable("tb_daily_revenue");
                entity.HasKey(e => new { e.BranchId, e.ReportDate })
                    .HasName("PK_tb_daily_revenue");
                entity.Property(e => e.BranchId).HasColumnName("BranchID");
                entity.Property(e => e.ReportDate).HasColumnName("ReportDate");
                entity.Property(e => e.Consignment).HasColumnName("Con");
                entity.Property(e => e.Boxes).HasColumnName("Boxes");
                entity.Property(e => e.Sender).HasColumnName("Sender");
                entity.Property(e => e.MonthlySender).HasColumnName("MonthlySender");
                entity.Property(e => e.AmCon).HasColumnName("AMCon");
                entity.Property(e => e.AmBoxes).HasColumnName("AMBoxes");
                entity.Property(e => e.Cash).HasColumnName("Cash");
                entity.Property(e => e.Rabbit).HasColumnName("Rabbit");
                entity.Property(e => e.Credit).HasColumnName("Credit");
                entity.Property(e => e.CreditBbl).HasColumnName("CreditBBL");
                entity.Property(e => e.CreditScb).HasColumnName("CreditSCB");
                entity.Property(e => e.QrPayment).HasColumnName("QRPay");
                entity.Property(e => e.LinePay).HasColumnName("LinePay");
                entity.Property(e => e.FreightSurcharge).HasColumnName("FreightSurcharge");
                entity.Property(e => e.AmSurcharge).HasColumnName("AMSurcharge");
                entity.Property(e => e.PupSurcharge).HasColumnName("PUPSurcharge");
                entity.Property(e => e.SatDelSurcharge).HasColumnName("SatDelSurcharge");
                entity.Property(e => e.RemoteAreaSurcharge).HasColumnName("RemoteAreaSurcharge");
                entity.Property(e => e.CodSurcharge).HasColumnName("CODSurcharge");
                entity.Property(e => e.CodAmount).HasColumnName("CODAmount");
                entity.Property(e => e.CodTotalCon).HasColumnName("CODTotalCon");
                entity.Property(e => e.InsurSurcharge).HasColumnName("InsurSurcharge");
                entity.Property(e => e.PkgSurcharge).HasColumnName("PkgSurcharge");
                entity.Property(e => e.VatSurcharge).HasColumnName("VatSurcharge");
                entity.Property(e => e.VatInsurSurcharge).HasColumnName("VatInsurSurcharge");
                entity.Property(e => e.VatCodSurcharge).HasColumnName("VatCODSurcharge");
                entity.Property(e => e.VatPkgSurcharge).HasColumnName("VatPkgSurcharge");
                entity.Property(e => e.DiscountSurcharge).HasColumnName("DiscountSurcharge");
                entity.Property(e => e.PkgService).HasColumnName("PkgService");
                entity.Property(e => e.DhlService).HasColumnName("DHLService");
                entity.Property(e => e.LineTopUpService).HasColumnName("LineTopUpService");
                entity.Property(e => e.VatService).HasColumnName("VatService");
                entity.Property(e => e.VatPkgService).HasColumnName("VatPkgService");
                entity.Property(e => e.CashForService).HasColumnName("CashForService");
                entity.Property(e => e.RabbitForService).HasColumnName("RabbitForService");
                entity.Property(e => e.CreditForService).HasColumnName("CreditForService");
                entity.Property(e => e.LinePayForService).HasColumnName("LinePayForService");
                entity.Property(e => e.BsdForService).HasColumnName("BSDForService");
                entity.Property(e => e.BsdSurcharge).HasColumnName("BSDSurcharge");
                entity.Property(e => e.CitySurcharge).HasColumnName("CITYSurcharge");
                entity.Property(e => e.CitynSurcharge).HasColumnName("CITYNSurcharge");
                entity.Property(e => e.CitysSurcharge).HasColumnName("CITYSSurcharge");
                entity.Property(e => e.CityvSurcharge).HasColumnName("CITYVSurcharge");
                entity.Property(e => e.GrabexSurcharge).HasColumnName("GRABEXSurcharge");
                entity.Property(e => e.BsdDiscount).HasColumnName("BSDDiscount");
                entity.Property(e => e.CityDiscount).HasColumnName("CITYDiscount");
                entity.Property(e => e.CitynDiscount).HasColumnName("CITYNDiscount");
                entity.Property(e => e.CitysDiscount).HasColumnName("CITYSDiscount");
                entity.Property(e => e.CityvDiscount).HasColumnName("CITYVDiscount");
                entity.Property(e => e.GrabexDiscount).HasColumnName("GRABEXDiscount");
                entity.Property(e => e.BsdLinePay).HasColumnName("BSDLinePay");
                entity.Property(e => e.BsdCash).HasColumnName("BSDCash");
                entity.Property(e => e.BsdLineTopUp).HasColumnName("BSDLineTopUp");
                entity.Property(e => e.BsdCon).HasColumnName("BSDCon");
                entity.Property(e => e.BsdBoxes).HasColumnName("BSDBoxes");
                entity.Property(e => e.BsdcodSurcharge).HasColumnName("BSDCODSurcharge");
                entity.Property(e => e.BsdcodAmount).HasColumnName("BSDCODAmount");
                entity.Property(e => e.BsdcodTotalCon).HasColumnName("BSDCODTotalCon");
                entity.Property(e => e.DropOffBox).HasColumnName("DropOffBox");
                entity.Property(e => e.TudForService).HasColumnName("TUDForService");
                entity.Property(e => e.TudVerifyBy).HasColumnName("TUDVerifyBy");
                entity.Property(e => e.TudVerifyDate).HasColumnName("TUDVerifyDate");
                entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
                entity.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
                entity.Property(e => e.Captured).HasColumnName("Captured");
                entity.Property(e => e.CapturedDate).HasColumnName("CapturedDate");
                entity.Property(e => e.CapturedBy).HasColumnName("CapturedBy");
                entity.Property(e => e.BonusCommission).HasColumnName("BonusCommission");
                entity.Property(e => e.BonusCommissionRemark).HasColumnName("BonusCommissionRemark");
                entity.Property(e => e.AdjOther).HasColumnName("AdjOther");
                entity.Property(e => e.AdjOtherRemark).HasColumnName("AdjOtherRemark");
                entity.Property(e => e.ReturnCharge).HasColumnName("ReturnCharge");
                entity.Property(e => e.ReturnChargeRemark).HasColumnName("ReturnChargeRemark");
                entity.Property(e => e.Suspense).HasColumnName("Suspense");
                entity.Property(e => e.SuspenseRemark).HasColumnName("SuspenseRemark");
                entity.Property(e => e.WithHoldingTax).HasColumnName("WithHoldingTax");
                entity.Property(e => e.WithHoldingTaxRemark).HasColumnName("WithHoldingTaxRemark");
                entity.Property(e => e.Promotion).HasColumnName("Promotion");
                entity.Property(e => e.PromotionRemark).HasColumnName("PromotionRemark");
                entity.Property(e => e.BankCharge).HasColumnName("BankCharge");
                entity.Property(e => e.BankChargeRemark).HasColumnName("BankChargeRemark");
                entity.Property(e => e.AdjCreditCardCharge).HasColumnName("AdjCreditCardCharge");
                entity.Property(e => e.AdjCreditCardRemark).HasColumnName("AdjCreditCardRemark");
                entity.Property(e => e.AdjLinePayCharge).HasColumnName("AdjLinePayCharge");
                entity.Property(e => e.AdjLinePayRemark).HasColumnName("AdjLinePayRemark");
                entity.Property(e => e.VerifyDate).HasColumnName("VerifyDate");
                entity.Property(e => e.RemittanceDate).HasColumnName("RemittanceDate");
                entity.Property(e => e.SendToKes).HasColumnName("SendToKES");
                entity.Property(e => e.SendToKesDate).HasColumnName("SendToKESDate");
                entity.Property(e => e.SendToErp).HasColumnName("SendToERP");
                entity.Property(e => e.SendToErpDate).HasColumnName("SendToERPDate");
                entity.Property(e => e.BsdProcessed).HasColumnName("BSDProcessed");
                entity.Property(e => e.BsdProcessedDate).HasColumnName("BSDProcessedDate");
                entity.Property(e => e.Approved).HasColumnName("Approved");
                entity.Property(e => e.ApprovedDate).HasColumnName("ApprovedDate");
                entity.Property(e => e.ApprovedBy).HasColumnName("ApprovedBy");
                entity.Property(e => e.FcConfirmed).HasColumnName("FCConfirmed");
                entity.Property(e => e.FcConfirmedDate).HasColumnName("FCConfirmedDate");
                entity.Property(e => e.FcConfirmedBy).HasColumnName("FCConfirmedBy");
                entity.Property(e => e.PsaSyncDate).HasColumnName("PSA_Sync_Date");
                entity.Property(e => e.PdcSyncDate).HasColumnName("PDC_Sync_Date");
                entity.Property(e => e.ErpSyncDate).HasColumnName("ERP_Sync_Date");
                entity.Property(e => e.RejectDate).HasColumnName("Reject_Date");
                //add by kathawutpa 17/7/2019 for project sales x
                entity.Property(e => e.Project).HasColumnName("Project");
                entity.Property(e => e.ProjectRemark).HasColumnName("ProjectRemark");
            });

            modelBuilder.Entity<ReviewBalanceReport>(entity =>
            {
                entity.ToTable("tb_tmp_review_balance");
            });


            modelBuilder.Entity<MonthlyExpenseDetail>(entity =>
            {
                entity.HasKey(e => new { e.BranchID, e.Month, e.Year, e.ItemID })
                    .HasName("PK_tb_fee_monthly_transaction_1");

                entity.ToTable("tb_fee_monthly_transaction");
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.ToTable("tb_Package");
                entity.Property(e => e.UnitPrice).HasColumnName("unit_price");
            });

            modelBuilder.Entity<StockOrder>(entity =>
            {
                entity.HasKey(e => new { e.OrderID, e.PackageID });
            });

            modelBuilder.Entity<StockStatus>(entity =>
            {
                entity.ToTable("tb_stock_status");
                entity.Property(e => e.ID).HasColumnName("status_id");
                entity.Property(e => e.Description).HasColumnName("status_desc");
            });

            modelBuilder.Entity<EOD>(entity =>
            {
                entity.HasKey(e => new { e.BranchID, e.Report_Date })
                    .HasName("PK_tb_EOD_1");

                entity.ToTable("tb_EOD");
            });

            modelBuilder.Entity<NoneShipmentDHL>(entity =>
            {
                entity.HasKey(e => new { e.RecordID, e.BranchID })
                    .HasName("PK_tb_NoneShipment");
            });

            modelBuilder.Entity<DailyRevenueDHL>(entity =>
            {
                entity.HasKey(e => new { e.BranchID, e.ReportDate })
                    .HasName("PK_tb_daily_revenue");
            });

            modelBuilder.Entity<GetBatch>(entity =>
            {
                entity.HasKey(e => new { e.Batch_Id })
                    .HasName("PK_sp_PDC_Reconcile_Get_Batch_Master");
            });

            modelBuilder.Entity<DailyRevenueDetailCash>(entity =>
            {
                entity.HasKey(e => new { e.ID })
                    .HasName("PK_sp_PDC_Report_DailyRevenueVerifyMatchCash_Get");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.user_id })
                    .HasName("PK_sp_PDC_Login_Get");
            });

            modelBuilder.Entity<UserMenuRole>(entity =>
            {
                entity.HasKey(e => new { e.MenuId })
                    .HasName("PK_sp_PDC_Get_Menu");
            });

            modelBuilder.Entity<DailyRevenuePivot>(entity =>
            {
                entity.HasKey(e => new { e.BranchID, e.ReportDate })
                    .HasName("PK_tb_daily_revenue_pivot");
            });

            modelBuilder.Entity<Insurance>(entity =>
            {
                entity.Property(e => e.ConsignmentNo).HasColumnName("consignment_no");
                entity.Property(e => e.SenderName).HasColumnName("sender_name");
                entity.Property(e => e.SenderMobile).HasColumnName("sender_mobile");
                entity.Property(e => e.IDCard).HasColumnName("id_card");
                entity.Property(e => e.RecipientName).HasColumnName("recipient_name");
                entity.Property(e => e.RecipientMobile).HasColumnName("recipient_mobile");
                entity.Property(e => e.RecipientAddress).HasColumnName("recipient_address");
                entity.Property(e => e.RecipientSoi).HasColumnName("recipient_soi");
                entity.Property(e => e.RecipientRoad).HasColumnName("recipient_road");
                entity.Property(e => e.RecipientDistrict).HasColumnName("recipient_district");
                entity.Property(e => e.RecipientAmphur).HasColumnName("recipient_amphur");
                entity.Property(e => e.RecipientProvince).HasColumnName("recipient_province");
                entity.Property(e => e.RecipientZipcode).HasColumnName("recipient_zipcode");
                entity.Property(e => e.ParcelWeight).HasColumnName("parcel_weight");
                entity.Property(e => e.TotalAmount).HasColumnName("total_amount");
                entity.Property(e => e.BranchId).HasColumnName("branch_id");
                entity.Property(e => e.BranchName).HasColumnName("branch_name");
                entity.Property(e => e.ReceiptNo).HasColumnName("receipt_no");
                entity.Property(e => e.ReceiptDate).HasColumnName("receipt_date");
                entity.Property(e => e.Qty).HasColumnName("qty");
                entity.Property(e => e.InsuranceAmount).HasColumnName("insurance_amount");
                entity.Property(e => e.InsuranceRemark).HasColumnName("insurance_remark");
                entity.Property(e => e.CODAmount).HasColumnName("cod_amount");
                entity.Property(e => e.AccountId).HasColumnName("account_id");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            });

            #region Service Point
            modelBuilder.Entity<SevicePointDailyRevenue>(entity =>
            {
                entity.HasKey(e => new { e.BranchID, e.ProfileID, e.ReportDate });

                entity.ToTable("tb_daily_revenue_rtsp");

                entity.Property(e => e.Consignment)
                    .HasColumnName("Con");
            });
            #endregion

            #region Reconcile
            modelBuilder.Entity<ReconcileAdjustMaster>(entity =>
            {
                entity.ToTable("tb_reconcile_adjust_master");

                entity.Property(e => e.AdjustId).HasColumnName("adjust_id");
                entity.Property(e => e.AdjustDescription).HasColumnName("adjust_desc");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");
            });

            modelBuilder.Entity<ReconcileAdjustTransaction>(entity =>
            {
                entity.ToTable("tb_reconcile_adjust_transaction");

                entity.HasKey(e => new { e.BranchId, e.ReportDate, e.TypeId, e.AdjustId })
                    .HasName("PK_tb_reconcile_adjust_transaction");

                entity.Property(e => e.BranchId).HasColumnName("branch_id");
                entity.Property(e => e.ReportDate).HasColumnName("report_date");
                entity.Property(e => e.TypeId).HasColumnName("type_id");
                entity.Property(e => e.AdjustId).HasColumnName("adjust_id");
                entity.Property(e => e.Amount).HasColumnName("amount");
                entity.Property(e => e.Remark).HasColumnName("remark");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");
            });

            modelBuilder.Entity<ReconcileSummaryMaster>(entity =>
            {
                entity.ToTable("tb_reconcile_summary_master");

                entity.HasKey(e => new { e.BranchId, e.ReportDate, e.TypeId })
                    .HasName("PK_tb_reconcile_summary_master");

                entity.Property(e => e.BranchId).HasColumnName("branch_id");
                entity.Property(e => e.ReportDate).HasColumnName("report_date");
                entity.Property(e => e.TypeId).HasColumnName("type_id");
                entity.Property(e => e.Amount).HasColumnName("amount");
                entity.Property(e => e.Commission).HasColumnName("commission");
                entity.Property(e => e.Tax).HasColumnName("tax");
                entity.Property(e => e.VerifiedFlag).HasColumnName("verified_flag");
                entity.Property(e => e.VerifiedDate).HasColumnName("verified_date");
                entity.Property(e => e.VerifiedBy).HasColumnName("verified_by");
                entity.Property(e => e.ConfirmFlag).HasColumnName("confirm_flag");
                entity.Property(e => e.ConfirmDate).HasColumnName("confirm_date");
                entity.Property(e => e.ConfirmBy).HasColumnName("confirm_by");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");
                entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
                entity.Property(e => e.DeletedDate).HasColumnName("deleted_date");
            });

            modelBuilder.Entity<DailyRevenueReconcileBillPayment>(entity =>
            {
                entity.HasKey(e => new { e.BranchID, e.ReportDate })
                    .HasName("PK_tb_daily_revenue_reconcile");
            });

            modelBuilder.Entity<DailyRevenueReconcileCards>(entity =>
            {
                entity.HasKey(e => new { e.BranchID, e.ReportDate })
                    .HasName("PK_tb_daily_revenue_reconcile");
            });

            modelBuilder.Entity<DailyRevenueReconcileLinePay>(entity =>
            {
                entity.HasKey(e => new { e.BranchID, e.ReportDate })
                    .HasName("PK_tb_daily_revenue_reconcile");
            });

            modelBuilder.Entity<DailyRevenueReconcileQrPayment>(entity =>
            {
                entity.HasKey(e => new { e.BranchID, e.ReportDate })
                    .HasName("PK_tb_daily_revenue_reconcile");
            });
            #endregion
        }
    }
}
