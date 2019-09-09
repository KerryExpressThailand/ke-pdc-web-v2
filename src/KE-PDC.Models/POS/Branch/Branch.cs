using System;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models.POS
{
    public class Branch
    {
        [Key]
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string ErpId { get; set; }
        public string ReceiptBranchName { get; set; }
        public string ContactPerson { get; set; }
        public string HomeAddress { get; set; }
        public string Road { get; set; }
        public string District { get; set; }
        public string Amphur { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string PostalCodeId { get; set; }
        public string Telephone { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string Email { get; set; }
        public string ConsignmentPrefix { get; set; }
        public string RegisterKey { get; set; }
        public string SenderName { get; set; }
        public string SenderContactPerson { get; set; }
        public string SenderHomeAddress { get; set; }
        public string SenderRoad { get; set; }
        public string SenderDistrict { get; set; }
        public string SenderAmphur { get; set; }
        public string SenderProvince { get; set; }
        public string SenderPostalCode { get; set; }
        public string SenderPostalCodeId { get; set; }
        public string SenderMobile1 { get; set; }
        public string SenderMobile2 { get; set; }
        public string SenderEmail { get; set; }
        public string CompanyName { get; set; }
        public string TaxId { get; set; }
        public string Dmsid { get; set; }
        public bool? FcVatable { get; set; }
        public string EdiUsername { get; set; }
        public string EdiPassword { get; set; }
        public string ApiKey { get; set; }
        public string OperatingDatetime { get; set; }
        public string TaxBranchName { get; set; }
        public string TaxCompanyName { get; set; }
        public string TaxAddress1 { get; set; }
        public string TaxAddress2 { get; set; }
        public string TaxPostalCode { get; set; }
        public string TaxTelephone { get; set; }
        public string TaxTaxId { get; set; }
        public string Note2 { get; set; }
        public bool? TaxInvoiceNoFromServer { get; set; }
        public DateTime? LastUpdate { get; set; }
        public bool? EnableUpdate { get; set; }
        public int? DisplaySequence { get; set; }
        public bool? TaxAbb { get; set; }
        public bool? LinepayEnable { get; set; }
        public string DatabaseVersionNo { get; set; }
        public string DatabaseVersionName { get; set; }
        public string DatabaseEdition { get; set; }
        public decimal? DatabaseDataMb { get; set; }
        public decimal? DatabaseLogMb { get; set; }
        public string DatabaseDescription { get; set; }
        public bool? DropOffEnable { get; set; }
        public string BranchType { get; set; }
        public bool? SendToKes { get; set; }
        public bool? CodEnable { get; set; }
        public bool? CreditEnable { get; set; }
        public bool? SelfcollectionEnable { get; set; }
        public bool? SclConsumerReceive { get; set; }
        public bool? SamplingEnable { get; set; }
        public int? ConnonLimit { get; set; }
        public string RegionId { get; set; }
        public int? CommissionRate { get; set; }
        public string OriginDc { get; set; }
        public DateTime? LastUpdateService { get; set; }
        public int? ProcessSequence { get; set; }
        public string CutoffTime { get; set; }
        public DateTime? FirstOpenDate { get; set; }
        public string ReceiptText_1 { get; set; }
        public string ReceiptText_2 { get; set; }
        public string ReceiptText_3 { get; set; }
        public string ReceiptText_4 { get; set; }
        public string ReceiptText_5 { get; set; }
        public DateTime? PsaSyncDate { get; set; }
        public DateTime? PdcSyncDate { get; set; }
        public DateTime? AwsSyncDate { get; set; }
        public bool? VgiEnable { get; set; }
    }
}
