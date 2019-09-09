using System;

namespace KE_PDC.Models
{
    public class UserMenu
    {
        public string Username { get; set; }
        public Guid MenuId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        //public User User { get; set; }
    }

    public class UserMenuRole
    {
        public string Username { get; set; }
        public string MenuId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        //public User User { get; set; }


    }
}
