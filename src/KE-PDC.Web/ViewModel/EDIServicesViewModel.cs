using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KE_PDC.ViewModel
{
    public class EDIServicesViewModel
    {
        public string Account_Name { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string First_Name { get; set; }
        public object GivenName { get; set; }
        public object Mobile { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string SurName { get; set; }
        public object TelephoneNumber { get; set; }
        public string UserPrincipalName { get; set; }
        public string ResultCode { get; set; }
        public string ResultDescription { get; set; }
    }

    public class ReqAd
    {
        public string Password { get; set; }
        public string User { get; set; }
    }


    public class SSOService
    {
        public string Account_Name { get; set; }
        public object Department { get; set; }
        public string Email { get; set; }
        public string First_Name { get; set; }
        public object GivenName { get; set; }
        public object Mobile { get; set; }
        public string Name { get; set; }
        public object Position { get; set; }
        public object SurName { get; set; }
        public object TelephoneNumber { get; set; }
        public string UserPrincipalName { get; set; }
        public object LandingPage { get; set; }
        public string ResultCode { get; set; }
        public string ResultDescription { get; set; }
    }

}
