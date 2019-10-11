using KE_PDC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KE_PDC.Areas.Api.ExternalServices.Utils
{
    public class StaticName
    {
        public static string apiUrl = "EDIServicesLoginAD";
        public static string SSOMethod = "SSOMethod";
    }
    public static class GlobalVal
    {
        public static List<DiscountModel> _listDiscount { get; set; }
    }
}
