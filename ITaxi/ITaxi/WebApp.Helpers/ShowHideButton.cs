using Base.Contracts.ViewModels;
using Base.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Helpers
{
    public static class ShowHideButtonExtensions
    {
        public static ShowHideButtonVM ToShowHideVM(this IShowHideItem item, bool parentIsIgnored = false )
        {
            return new ShowHideButtonVM
            {
                Id = item.Id,
                IsIgnored = item.IsIgnored,
                CanBeShown = !parentIsIgnored,
            };
        }
    }
}
