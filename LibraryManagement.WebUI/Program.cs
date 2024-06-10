using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using learningASP.NET_CORE.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<ILibraryService, LibraryService>();

// Add services to the container
builder.Services.AddControllersWithViews();

// Register UserService as a singleton
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=RegisterLogIn}/{action=LogInPage}"
);

app.Run();
