
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.ViewModels
{
    public class ShowHideButtonVM
    {
        public Guid Id { get; set; }
        public bool IsIgnored { get; set; }
        public bool CanBeShown { get; set; } = true;
    }
}
