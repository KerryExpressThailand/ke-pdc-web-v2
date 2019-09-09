using System;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models
{
    public class UserProfile
    {
        [Key]
        public string ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string ProfileOwner { get; set; }
        public string ProfileCompany { get; set; }
        public string ProfileTaxId { get; set; }
        public bool ProfileIncludeVat { get; set; }
        public string ProfileAccountNo { get; set; }
        public string ProfileBillAddress { get; set; }
        public string ProfileEmail1 { get; set; }
        public string ProfileEmail2 { get; set; }
        public string ProfileEmail3 { get; set; }

        //public User User { get; set; }
    }
}
