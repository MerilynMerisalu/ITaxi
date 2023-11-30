using System.Globalization;
using System.Text;
using App.BLL;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.DAL.EF;
using App.DAL.EF.Repositories;
using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rotativa.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApp;
using WebApp.ApiControllers;
using WebApp.Helpers;
using AutoMapperConfig = WebApp.ApiControllers.v1.AutoMapperConfig;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IAppUnitOfWork, AppUOW>();
builder.Services.AddScoped<IAppBLL, AppBLL>();
builder.Services.AddIdentity<AppUser, AppRole>(
        options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddLocalization(options => { options.ResourcesPath = ""; });

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

/*
builder.Services.AddControllersWithViews(options =>
    {
        options.ModelBinderProviders.Insert(0, new CustomLangStrBinderProvider());
    })
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
        options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(Base.Resources.Common)));
        */
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    // in case of no explicit version
    options.DefaultApiVersion = new ApiVersion(1, 0);
});
builder.Services.AddVersionedApiExplorer( options => options.GroupNameFormat = "'v'VVV" );
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
    options.SchemaFilter<ApiSchemaFilter>();
});


builder.Services.AddSingleton<IConfigureOptions<MvcOptions>,
    ConfigureModelBindingLocalization>();


/* Setting up the language support system */
var supportedCultures = builder.Configuration
    .GetSection("SupportedCultures")
    .GetChildren()
    .Select(x => new CultureInfo(x.Value!))
    .ToArray();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    // datetime and currency support
    options.SupportedCultures = supportedCultures;
    // UI translated strings
    options.SupportedUICultures = supportedCultures;
    // if nothing is found, use this
    options.DefaultRequestCulture =
        new RequestCulture(builder.Configuration["DefaultCulture"]!, builder.Configuration["DefaultCulture"]!);
    options.SetDefaultCulture(builder.Configuration["DefaultCulture"]!);

    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        // Order is important, its in which order they will be evaluated
        // add support for ?culture=ru-RU
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider(),
        new AcceptLanguageHeaderRequestCultureProvider()
    };
});

builder.Services.AddAutoMapper(typeof(App.DAL.EF.AutoMapperConfig),
    typeof(App.BLL.AutoMapperConfig),
    typeof(AutoMapperConfig));
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie(options => { options.SlidingExpiration = true; })
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
            ClockSkew = TimeSpan.Zero // remove delay of token when expire
        };
    });


builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddTransient<IMailService, MailService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name:"CorsAllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
} );

var app = builder.Build();
Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");

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

app.UseCors("CorsAllowAll");


var requestLocalizationOptions = ((IApplicationBuilder)app).ApplicationServices
    .GetService<IOptions<RequestLocalizationOptions>>()?.Value;
if (requestLocalizationOptions != null)
    app.UseRequestLocalization(requestLocalizationOptions);

app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    "areas",
    "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>(); 
    foreach ( var description in provider.ApiVersionDescriptions )
    {
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant() 
        );
    }

    // serve from root
    // options.RoutePrefix = string.Empty;
});
app.Run();
