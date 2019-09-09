using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KE_PDC.Models
{
    public class AuthUser
    {
        [Key]
        public string user_id { get; set; }

        public byte[] password { get; set; }

        public string user_name { get; set; }

        public DateTime? created_date { get; set; }

        public string created_by { get; set; }

        public DateTime? updated_date { get; set; }

        public string updated_by { get; set; }

        public DateTime? last_login { get; set; }

        public string email { get; set; }
    }
}
