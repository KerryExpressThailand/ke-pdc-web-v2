using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models
{
    public class User
    {
        private string _Username;

        [Key]
        public string Username {
            get {
                return _Username.ToLower();
            }
            set {
                _Username = value.ToLower();
            }
        }
        public string Password { get; set; }
        public string Name { get; set; }
        public int? RoleId { get; set; }
        public UserGroupStatus UserGroupId { get; set; }
        public string ProfileId { get; set; }
        public string DefaultShopId { get; set; }
        public string email { get; set; }
        public DateTime? LastLogin { get; set; }
        //public DateTime? CreatedAt { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTime? UpdatedAt { get; set; }
        //public string UpdatedBy { get; set; }

        public List<UserMenu> UserMenu { get; set; }
        public UserGroup UserGroup { get; set; }
        public UserProfile UserProfile { get; set; }
    }

    public class UserMapRole
    {
        public string Username;
        public string Password { get; set; }
        public string Name { get; set; }
        public string RoleId { get; set; }
        public string email { get; set; }
        public DateTime? LastLogin { get; set; }

        public List<UserMenuRole> UserMenu { get; set; }
        public UserGroup UserGroup { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
