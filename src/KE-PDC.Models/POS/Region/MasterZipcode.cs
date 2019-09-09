using System;
using System.Collections.Generic;
using System.Text;

namespace KE_PDC.Models
{
    public class MasterZipcode
    {
        public string PostalcodeId { get; set; }  
        public string Postalcode { get; set; }
        public string Province { get; set; }
        public string Amphur { get; set; }
        public string District { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string Remark { get; set; }
        public string RegionId { get; set; }

    }

    public class Region
    {
        public string RegionId { get; set; }
        public string RegionName  { get; set; }
    }

}
