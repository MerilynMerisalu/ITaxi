using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NuGet.Protocol.Plugins;

namespace WebApp.Filters
{
    public class ProfileCompleteFilterAttribute : ActionFilterAttribute
    {
        private readonly IAppBLL _appBll;
        private readonly IHttpContextAccessor _contextAccessor;

        public ProfileCompleteFilterAttribute(IAppBLL appBll, IHttpContextAccessor contextAccessor)
        {
            _appBll = appBll;
            _contextAccessor = contextAccessor;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (_contextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false)
            {
                var userId = _contextAccessor.HttpContext?.User.GettingUserId();

                if (userId != null)
                {
                    var user = _appBll.AppUsers.GettingAppUserByAppUserIdAsync(userId.Value).Result;

                    if (user != null)
                    {
                        // check if the profile is complete
                        // if not complete

                        var profileComplete = true;

                        if (string.IsNullOrEmpty(user.PhoneNumber))
                        {
                            profileComplete = false;
                        }

                        if (!profileComplete)
                        {
                            context.Result = new RedirectResult("/Identity/Account/Manage");
                        }
                    }
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
