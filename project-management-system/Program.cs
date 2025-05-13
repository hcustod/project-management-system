using COMP2139_ICE.Data;
using COMP2139_ICE.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Db connection registered
builder.Services.AddDbContext<ApplicationDbContext>( options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

// Configure Serilog
// Logging level: Verbose (most aggressive - produces a lot of logs), Debug, Information, Warning, Error, Fatal
Log.Logger = new LoggerConfiguration()
    //.MinimumLevel.Information()
    //.WriteTo.Console()
    //.WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    //.Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Email 
builder.Services.AddSingleton<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseStatusCodePagesWithRedirects("Home/NotFoundPage");

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.MapStaticAssets();

app.MapDefaultControllerRoute();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

/* 
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Projects}/{action=Index}/{id?})"
);
*/ 

app.Run();