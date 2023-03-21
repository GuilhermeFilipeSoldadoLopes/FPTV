using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using FPTV.Data;
using FPTV.Services.EmailSenderService;
using Microsoft.EntityFrameworkCore.Infrastructure;
using FPTV.Models.UserModels;
using FPTV;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddAuthentication()
    .AddSteam()
    .AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
});

var connectionString = builder.Configuration.GetConnectionString("FPTV_ContextProd");
builder.Services.AddDbContext<FPTVContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<UserBase, IdentityRole>(options =>
    { options.SignIn.RequireConfirmedAccount = true;
        options.Tokens.ProviderMap.Add("CustomEmailConfirmation",
            new TokenProviderDescriptor(
                typeof(CustomEmailConfirmationTokenProvider<UserBase>)));
        options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
    }).AddEntityFrameworkStores<FPTVContext>()
    .AddTokenProvider<DataProtectorTokenProvider<UserBase>>(TokenOptions.DefaultProvider);

builder.Services.AddTransient<CustomEmailConfirmationTokenProvider<UserBase>>();

//builder.Services.AddDefaultIdentity<UserBase>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<FPTVContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

builder.Services.ConfigureApplicationCookie(o => {
    o.ExpireTimeSpan = TimeSpan.FromDays(5);
    o.SlidingExpiration = true;
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
       o.TokenLifespan = TimeSpan.FromHours(3));

services.AddMvc();
var app = builder.Build();

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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.CreateRolesAsync(builder.Configuration).Wait();

app.UseEndpoints(endpoints =>
{
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();
});

app.Run();

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> CreateRolesAsync(this WebApplication app, IConfiguration configuration)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetService<FPTVContext>();
        var userStore = scope.ServiceProvider.GetService<IUserStore<UserBase>>();
        var emailStore = userStore as IUserEmailStore<UserBase>;
        var env = scope.ServiceProvider.GetService<IWebHostEnvironment>();

        /*Console.WriteLine("Context: " + context);
        Console.WriteLine("UserStore: " + userStore);
        Console.WriteLine("EmailStore: " + emailStore);
        Console.WriteLine("Env: " + env);*/

        if (context != null && userStore != null && emailStore != null && env != null)
        {
            if (context.UserBase.Count<UserBase>() == 0)
            {
                await Configurations.CreateRoles(scope.ServiceProvider, context, userStore, emailStore, env);
            }
        }

        return app;
    }
}
