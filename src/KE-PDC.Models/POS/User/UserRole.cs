using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KE_PDC.Models
{
    public class UserRole
    {
        [Key]
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string user_role { get; set; }
        public bool status { get; set; }
        public string email { get; set; }
        public DateTime? last_login { get; set; }

    }




}
