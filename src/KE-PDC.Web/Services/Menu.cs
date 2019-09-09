using System;
using System.Collections.Generic;

namespace KE_PDC
{
    public class Menu
    {
        public string Id { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Url { get; set; }
        public string LinkText { get; set; }
        public string Icon { get; set; }
        public List<List<Menu>> Children { get; set; }
    }
}
