using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.ViewModel
{
    public class MonthlyExpenseUpdateViewModel
    {
        [Required]
        public string BranchID { get; set; }
        [Required]
        public string MonthYear { get; set; }

        public string ManagementFee { get; set; }
        public string[] ManagementFeeID { get; set; }
        public string[] ManagementFeeItem { get; set; }
        public string[] ManagementFeeExpense { get; set; }
        public string[] ManagementFeeRemark { get; set; }
        public IFormFile[] ManagementFeeAttach { get; set; }
        public bool[] ManagementFeeTrash { get; set; }

        public string ServiceFeeIT { get; set; }
        public string[] ServiceFeeITID { get; set; }
        public string[] ServiceFeeITItem { get; set; }
        public string[] ServiceFeeITExpense { get; set; }
        public string[] ServiceFeeITRemark { get; set; }
        public IFormFile[] ServiceFeeITAttach { get; set; }
        public bool[] ServiceFeeITTrash { get; set; }

        public string ServiceFeeSupply { get; set; }
        public string[] ServiceFeeSupplyID { get; set; }
        public string[] ServiceFeeSupplyItem { get; set; }
        public string[] ServiceFeeSupplyExpense { get; set; }
        public string[] ServiceFeeSupplyRemark { get; set; }
        public IFormFile[] ServiceFeeSupplyAttach { get; set; }
        public bool[] ServiceFeeSupplyTrash { get; set; }

        public string ServiceFeeFacility { get; set; }
        public string ServiceFeeFacilityVat { get; set; }
        public string[] ServiceFeeFacilityID { get; set; }
        public string[] ServiceFeeFacilityItem { get; set; }
        public string[] ServiceFeeFacilityExpense { get; set; }
        public string[] ServiceFeeFacilityRemark { get; set; }
        public IFormFile[] ServiceFeeFacilityAttach { get; set; }
        public bool[] ServiceFeeFacilityTrash { get; set; }

        public string SalesPackage { get; set; }
        public string[] SalesPackageID { get; set; }
        public string[] SalesPackageItem { get; set; }
        public string[] SalesPackageAmount { get; set; }
        public string[] SalesPackageExpense { get; set; }
        public string[] SalesPackageRemark { get; set; }
        public string[] SalesPackageAttach { get; set; }
        public bool[] SalesPackageTrash { get; set; }

        public string Adjustment { get; set; }
        public string[] AdjustmentID { get; set; }
        public string[] AdjustmentItem { get; set; }
        public string[] AdjustmentExpense { get; set; }
        public string[] AdjustmentRemark { get; set; }
        public string[] AdjustmentAttach { get; set; }
        public bool[] AdjustmentTrash { get; set; }
    }
}
