using App.Enum.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.ViewModels
{
    public class ErrorPageViewModel
    {
        public ErrorStatusCode StatusCode { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
    }
}
