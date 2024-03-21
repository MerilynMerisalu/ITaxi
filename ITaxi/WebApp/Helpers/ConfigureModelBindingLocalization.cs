using Base.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace WebApp.Helpers;

/// <summary>
/// Binding localization
/// </summary>
public class ConfigureModelBindingLocalization : IConfigureOptions<MvcOptions>
{
    private readonly IServiceScopeFactory _serviceFactory;

    /// <summary>
    /// Constructor for the binding localization
    /// </summary>
    /// <param name="serviceFactory">Service factory</param>
    public ConfigureModelBindingLocalization(IServiceScopeFactory serviceFactory)
    {
        _serviceFactory = serviceFactory;
    }

    /// <summary>
    /// Binding localization configure
    /// </summary>
    /// <param name="options">Configure options</param>
    public void Configure(MvcOptions options)
    {
        using (var scope = _serviceFactory.CreateScope())
        {
            var provider = scope.ServiceProvider;
            var localizer = provider.GetRequiredService<IStringLocalizer<Common>>();

            options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) =>
                localizer[Common.ErrorMessageAttemptedValueIsInvalid, x, y]);

            options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x =>
                localizer[Common.ErrorMessageMissingBindRequiredValue, x]);

            // localizer["A value for the '{0}' parameter or property was not provided.", x]);

            options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() =>
                localizer[Common.ErrorMessageMissingKeyOrValue]);

            // localizer["A value is required."]);

            options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() =>
                localizer[Common.ErrorMessageMissingRequestBodyRequiredValue]);

            // localizer["A non-empty request body is required."]);

            options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x =>
                localizer[Common.ErrorMessageNonPropertyAttemptedValueIsInvalid, x]);
            // localizer["The value '{0}' is not valid.", x]);

            options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() =>
                localizer[Common.ErrorMessageNonPropertyUnknownValueIsInvalid]);
            // localizer["The supplied value is invalid."]);

            options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() =>
                localizer[Common.ErrorMessageNonPropertyValueMustBeANumber]);
            // localizer["The field must be a number."]);

            options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(x =>
                localizer[Common.ErrorMessageUnknownValueIsInvalid, x]);
            //  localizer["The supplied value is invalid for {0}.", x]);

            options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x =>
                localizer[Common.ErrorMessageValueIsInvalid, x]);
            //  localizer["The value '{0}' is invalid.", x]);

            options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x =>
                localizer[Common.ErrorMessageValueMustBeANumber, x]);
            //  localizer["The field {0} must be a number.", x]);

            options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x =>
                localizer[Common.ErrorMessageValueMustNotBeNull, x]);
            //  localizer["The value '{0}' is invalid.", x]);
        }
    }
}