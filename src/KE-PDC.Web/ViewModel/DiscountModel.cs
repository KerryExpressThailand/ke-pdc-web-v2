using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KE_PDC.ViewModel
{
    public class DiscountModel
    {
        public string BranchType { get; set; }
        public string ERPID { get; set; }
        public string BranchId { get; set; }
        public string ReceiptNo { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string  MemberId { get; set; }
        public string  SenderName { get; set; }
        public string  SenderMobile { get; set; }
        public string  DiscountCode { get; set; }
        public string  DiscountType { get; set; }
        public decimal?  Surcharge { get; set; }
        public decimal?  DiscountAmount { get; set; }
    }

    public class ResResultDiscount
    {
        public List<DiscountModel> Result { get; set; }
    }

    public class ReqDiscount
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public List<string> DiscountTypeList { get; set; }
        public List<string> BranchIdList { get; set; }
        public string BranchList { get; set; }
    }

    public class ReqDiscountType
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public List<DiscountTypeList> DiscountTypeList { get; set; }
        public List<BranchIdList> BranchIdList { get; set; }
        public string BranchList { get; set; }
    }
}
