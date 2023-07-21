using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Mihcelle.Hwavmvid;
using Mihcelle.Hwavmvid.Client;
using Mihcelle.Hwavmvid.Modules.Htmleditor;
using Mihcelle.Hwavmvid.Server;
using Mihcelle.Hwavmvid.Server.Data;
using Mihcelle.Hwavmvid.Server.Tasks;
using Mihcelle.Hwavmvid.Shared.Constants;
using Mihcelle.Hwavmvid.Shared.Models;
using System;
using System.CodeDom;
using System.Runtime.CompilerServices;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// mihcelle.hwavmvid
var configbuilder = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.ContentRootPath}.json", true, true);
var config = configbuilder.Build();

// mihcelle.hwavmvid
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var installed = !string.IsNullOrEmpty(connectionString);

if (installed == false)
{
    var configpath = string.Concat(builder.Environment.ContentRootPath, "\\wwwroot\\", "tobaccoindustries.json");
    var jsonconfig = System.IO.File.ReadAllText(configpath);
    var deserializedconfig = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonconfig);
    if (deserializedconfig != null)
    {
        deserializedconfig["installation"] = new { createdon = string.Empty };
        var updatedconfigfile = JsonSerializer.Serialize(deserializedconfig, new JsonSerializerOptions { WriteIndented = true });
        System.IO.File.WriteAllText(configpath, updatedconfigfile);
    }
}

try
{
    builder.Services.AddDbContext<Mihcelle.Hwavmvid.Server.Data.Applicationdbcontext>(options => options.UseSqlServer(connectionString));
    builder.Services.AddIdentity<Applicationuser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 2;
    })
        .AddEntityFrameworkStores<Mihcelle.Hwavmvid.Server.Data.Applicationdbcontext>();

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.Name = Authentication.Authcookiename;
        options.Cookie.HttpOnly = false;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
    });
} catch (Exception exception) { Console.WriteLine(exception.Message); }

builder.Services.AddMvc()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = false;
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;
                options.JsonSerializerOptions.AllowTrailingCommas = true;
                options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
                options.JsonSerializerOptions.DefaultBufferSize = 4096;
                options.JsonSerializerOptions.MaxDepth = 41;
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IHostedService, Mihcelle.Hwavmvid.Server.Tasks.Hostedservicebase>();

// mihcelle hwavmvid
builder.Services.AddScoped<Mihcelle.Hwavmvid.Framework.Modules.Databasesettings.Applicationdbcontext, Mihcelle.Hwavmvid.Framework.Modules.Databasesettings.Applicationdbcontext>();
builder.Services.AddScoped<Mihcelle.Hwavmvid.Framework.Modules.Pagesettings.Applicationdbcontext, Mihcelle.Hwavmvid.Framework.Modules.Pagesettings.Applicationdbcontext>();
builder.Services.AddScoped<Mihcelle.Hwavmvid.Framework.Modules.Sitesettings.Applicationdbcontext, Mihcelle.Hwavmvid.Framework.Modules.Sitesettings.Applicationdbcontext>();
builder.Services.AddScoped<Mihcelle.Hwavmvid.Framework.Modules.Taskmanager.Applicationdbcontext, Mihcelle.Hwavmvid.Framework.Modules.Taskmanager.Applicationdbcontext>();


builder.Services.AddCors(option =>
{
    option.AddPolicy("mihcellehwavmvidcorspolicy", (builder) =>
    {
        builder.SetIsOriginAllowedToAllowWildcardSubdomains()
               .SetIsOriginAllowed(isOriginAllowed => true)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

// mihcelle.hwavmvid
builder.Services.AddSignalR()
    .AddHubOptions<Applicationhub>(options =>
    {
        options.EnableDetailedErrors = true;
        options.KeepAliveInterval = TimeSpan.FromSeconds(15);
        options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
        options.MaximumReceiveMessageSize = Int64.MaxValue;
        options.StreamBufferCapacity = Int32.MaxValue;
    })
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.WriteIndented = false;
        options.PayloadSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;
        options.PayloadSerializerOptions.AllowTrailingCommas = true;
        options.PayloadSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
        options.PayloadSerializerOptions.DefaultBufferSize = 4096;
        options.PayloadSerializerOptions.MaxDepth = 41;
        options.PayloadSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.PayloadSerializerOptions.PropertyNamingPolicy = null;
    });


if (installed == true)
{
    try
    {
        var programitems = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(assemblytypes => (typeof(Programinterface)).IsAssignableFrom(assemblytypes));
        foreach (var item in programitems)
        {
            if (item.IsClass)
            {
                Programinterface? programinterfaceinstance = (Programinterface?) Activator.CreateInstance(item);
                if (programinterfaceinstance != null)
                    programinterfaceinstance.Configure(builder.Services);
            }
        }
    } 
    catch (Exception exception) { Console.WriteLine(exception.Message); }
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("mihcellehwavmvidcorspolicy");

app.UseAuthentication();
app.UseAuthorization();

// mihcelle.hwavmvid
app.MapHub<Applicationhub>("/api/applicationhub", options =>
    {
        options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
        options.ApplicationMaxBufferSize = long.MaxValue;
        options.TransportMaxBufferSize = long.MaxValue;
        options.WebSockets.CloseTimeout = TimeSpan.FromSeconds(10);
        options.LongPolling.PollTimeout = TimeSpan.FromSeconds(10);
    });

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

if (installed == true)
{

    try // run modules installer migrate database and add package references to database
    {
        var installeritems = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(assemblytypes => (typeof(IModuleinstallerinterface)).IsAssignableFrom(assemblytypes));
        foreach (var item in installeritems)
        {
            if (item.IsClass)
            {
                using (var scope = app.Services.CreateScope())
                {
                    var moduleinstaller = (IModuleinstallerinterface?) scope.ServiceProvider.GetService(item);
                    if (moduleinstaller != null)
                    {                        
                        var package = moduleinstaller.applicationmodulepackage;
                        var dbcontext = scope.ServiceProvider.GetService<Mihcelle.Hwavmvid.Server.Data.Applicationdbcontext>();

                        if (dbcontext != null)
                        {
                            var installedpackage = dbcontext.Applicationmodulepackages.Where(item => item.Name == package.Name).FirstOrDefault();
                            if (installedpackage == null)
                            {

                                await moduleinstaller.Install();
                                dbcontext.Applicationmodulepackages.Add(package);
                                await dbcontext.SaveChangesAsync();

                            }
                        }                        
                    }
                }
            }
        }
    }
    catch (Exception exception) { Console.WriteLine(exception.Message); }
    
}

if (installed == true)
{
    try
    {
        var programitems = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(assemblytypes => (typeof(Programinterface)).IsAssignableFrom(assemblytypes));
        foreach (var item in programitems)
        {
            if (item.IsClass)
            {
                Programinterface? programinterfaceinstance = (Programinterface?)Activator.CreateInstance(item);
                if (programinterfaceinstance != null)
                    programinterfaceinstance.Configureapp(app);
            }
        }
    }
    catch (Exception exception) { Console.WriteLine(exception.Message); }
}

app.Run();


