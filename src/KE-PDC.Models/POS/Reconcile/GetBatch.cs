using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KE_PDC.Models
{
    public class GetBatch
    {
        [Key]
        public int? Batch_Id { get; set; }
    }
}
