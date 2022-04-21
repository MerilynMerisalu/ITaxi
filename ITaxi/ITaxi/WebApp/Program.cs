using System.Collections.Immutable;
using System.Globalization;
using App.DAL.EF;
using App.Domain.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using WebApp.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
/*builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>();*/
builder.Services.AddIdentity<AppUser, AppRole>(
        options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddControllersWithViews();

/* Setting up the language support system */
var culture = new CultureInfo("et-EE");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{

    // TODO: Should be in appsettings json
    var appSupportedCultures = new[]
    {
        new CultureInfo("et-EE"),
        new CultureInfo("en-GB")
        
    };
   
    options.SupportedCultures = appSupportedCultures;
    options.SupportedUICultures = appSupportedCultures;
    
    options.DefaultRequestCulture = new RequestCulture("en-GB", "en-GB") ;
    options.SetDefaultCulture("en-GB");
    options.RequestCultureProviders = new List<IRequestCultureProvider>()
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    };
    builder.Services.AddSingleton<IConfigureOptions<MvcOptions>,
        ConfigureModelBindingLocalization>();

});
var app = builder.Build();




await DataHelper.SetupAppData(app, app.Environment, app.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
RotativaConfiguration.Setup(builder.Environment.WebRootPath);

app.UseRouting();


var requestLocalizationOptions = ((IApplicationBuilder) app).ApplicationServices
    .GetService<IOptions<RequestLocalizationOptions>>()?.Value;
if (requestLocalizationOptions != null)
    app.UseRequestLocalization(requestLocalizationOptions);

app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
