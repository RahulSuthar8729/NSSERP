using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSSERP.Models;
using NSSERP;
using System;
using NSSERP.DbFunctions;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<YourDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConStr"))
);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Access/Login";
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.SlidingExpiration = true;
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddTransient<GetAllUsersDetails>();
builder.Services.AddScoped<DbClass>();

var webApiBaseUrl = builder.Configuration["WebApiSettings:BaseUrl"];

builder.Services.AddHttpClient("WebApi", client =>
{
    client.BaseAddress = new Uri(webApiBaseUrl);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    // Add any other headers or configurations if needed
});
//builder.Services.AddScoped<WebApiService>();
//builder.Services.AddScoped<IWebApiService, WebApiService>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession(); // Add this line to enable session middleware

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "nationalGangotri",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Access}/{action=Login}/{id?}");
});

app.Run();
