using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KE_PDC.Models
{
    public class SharedModel
    {
        public class StatusModel
        {
            public int code { get; set; }
            public string desc { get; set; }
        }

        public class ResponseStatusModel
        {
            public StatusModel status { get; set; }
        }

        public class ResponseByInfoModel
        {
            public StatusModel status { get; set; }
            public object result { get; set; }
        }

        public class ResponseSequenceNumberModel
        {
            [Key]
            public string next_number { get; set; }
        }

        public class ResponseConsignmentNoModel
        {
            [Key]
            public string consignment_No { get; set; }
            public string package_id { get; set; }
            public decimal selling_price { get; set; }
        }


    }
}
