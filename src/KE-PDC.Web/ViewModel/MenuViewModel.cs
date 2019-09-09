using System;
using System.Collections.Generic;

namespace KE_PDC.ViewModel
{
    public class MenuViewModel
    {
        private Guid _id;
        public Guid? Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value.HasValue)
                {
                    this._id = value.Value;
                }
                else
                {
                    this._id = Guid.NewGuid();
                }
            }
        }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Url { get; set; }
        public string LinkText { get; set; }
        public string Icon { get; set; }
        public List<List<MenuViewModel>> Children { get; set; }
    }
}
