using App.Domain;
using Base.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApp.Helpers;

/*public class CustomLangStrBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Metadata.ModelType == typeof(LangStr))
        {
            var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();
            return new LangStrBinderProvider(loggerFactory, context.Metadata.ModelType);
        }

        return null;
    }
}*/