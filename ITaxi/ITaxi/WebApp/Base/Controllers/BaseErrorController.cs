using App.Enum.Enum;
using Base.Resources;
using Base.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Base.Controllers;

public abstract class BaseErrorController : Controller
{
    protected IActionResult GetErrorView(int statusCode)
    { 
        Response.StatusCode = statusCode;

        var errorStatusCode = Enum.IsDefined(typeof(ErrorStatusCode), statusCode) ?
            (ErrorStatusCode)statusCode : ErrorStatusCode.ServerError; 
            
        var vm = new ErrorPageViewModel() { 
            StatusCode = errorStatusCode,
        };

    }

    private static string GetTitle(ErrorStatusCode statusCode) 
    {
        return statusCode switch
        {
            ErrorStatusCode.Forbidden => Common.ForbiddenTitle,
            ErrorStatusCode.NotFound => Common.NotFoundTitle,
            ErrorStatusCode.ServerError => Common.ServerErrorTitle,
            _ => Common.GeneralErrorMessage
        };
        
    }

    private static string GetMessage(ErrorStatusCode statusCode)
    {
        return statusCode switch
        {
            ErrorStatusCode.Forbidden => "",
            ErrorStatusCode.NotFound => Common.NotFoundText,
            ErrorStatusCode.ServerError => "",
            _ => Common.GeneralErrorMessage
        };
    }