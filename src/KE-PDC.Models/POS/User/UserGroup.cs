using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models
{
    public class UserGroup
    {
        [Key]
        public UserGroupStatus UserGroupId { get; set; }
        public string UserGroupName { get; set; }

        //public User User { get; set; }
    }

    public enum UserGroupStatus
    {
        Unknow,
        Administrators,
        AccountingLeader,
        Normal,
        DCSPLeader
    }
}
