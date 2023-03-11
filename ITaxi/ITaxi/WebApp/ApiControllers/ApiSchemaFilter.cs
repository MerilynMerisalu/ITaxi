using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApp.ApiControllers;

public class ApiSchemaFilter : ISchemaFilter
{
    /// <summary>
    /// Prefixes of Type FullNames that should NOT be exposed in the swagger Schema documentation
    /// </summary>
    private readonly string[] _blackList = 
    {
        "App.DAL",
        "App.Domain",
        "App.BLL",
        "WebApp.DTO",
        "Base.Domain",
        "Microsoft.AspNetCore"
    };
    private readonly string[] _whiteList = 
    {
        //"App.Domain.Enum"
    };
    
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var keys = context.SchemaRepository.Schemas.Keys
            .Where(key => _blackList.Any(bl => key.StartsWith(bl)))
            .Where(key => !_whiteList.Any(wl => key.StartsWith(wl)))
            .ToList();
        
        foreach(var key in keys)
        {
            context.SchemaRepository.Schemas.Remove(key);   
        }
    }
}