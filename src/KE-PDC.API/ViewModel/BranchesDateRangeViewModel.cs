using System;
using System.Collections.Generic;

namespace KE_PDC.API.ViewModel
{
    public class BranchesDateRangeViewModel
    {
        public int Page { get; set; }
        
        public int PerPage { get; set; }

        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }

        public List<string> Branches { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
