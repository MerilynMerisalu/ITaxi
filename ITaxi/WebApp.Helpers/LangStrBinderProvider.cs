namespace WebApp.Helpers;

/*public class LangStrBinderProvider : IModelBinder
{
    private readonly ILogger<LangStrBinderProvider>? _logger;
    private readonly Type _type;

    public LangStrBinderProvider(ILoggerFactory? loggerFactory, Type type)
    {
        _logger = loggerFactory?.CreateLogger<LangStrBinderProvider>();
        _type = type;
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        var value = valueProviderResult.FirstValue;

        if (value == null)
        {
            return Task.CompletedTask;
        }

        _logger?.LogDebug("LangStrBinder: {}", value);

        bindingContext.Result = ModelBindingResult.Success(new LangStr(value));

        return Task.CompletedTask;
    }
}*/