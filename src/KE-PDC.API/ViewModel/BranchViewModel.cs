using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace KE_PDC.API.ViewModel
{
    public class BranchViewModel
    {
        [DefaultValue(1)]
        public int Page { get; set; }

        [DefaultValue(15)]
        public int PerPage { get; set; }

        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }

        public List<string> BranchTypes { get; set; }

        public string SearchBy { get; set; }
        public string SearchKeyword { get; set; }
    }
}
