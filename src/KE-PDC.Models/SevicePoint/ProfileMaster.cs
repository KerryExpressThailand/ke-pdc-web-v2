using System;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models.SevicePoint
{
    public class ProfileMaster
    {
        [Key]
        public string ProfileId { get; set; }
        public string MasterId { get; set; }
        public string ProjectId { get; set; }
        public string KESAccountId { get; set; }
        public string POSAccountId { get; set; }
        public string ReferenceCode { get; set; }
        public string OriginDC { get; set; }
        public string ZoneId { get; set; }
        public string OwnerId { get; set; }
        public string ProfileName { get; set; }
        public string ProfileAddress1 { get; set; }
        public string ProfileAddress2 { get; set; }
        public string ProfileZipcode { get; set; }
        public string ProfileMobile1 { get; set; }
        public string ProfileMobile2 { get; set; }
        public string ProfilePerson { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public decimal? CommissionRate { get; set; }
        public string CutoffTime { get; set; }
        public bool MobilePrinter { get; set; }
        public bool SynchronizeKES { get; set; }
        public bool? EnableScheduler { get; set; }
        public bool ProfileArchetype { get; set; }
        public string TaxIdentification { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedDesc { get; set; }
    }
}
