using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using learningASP.NET_CORE.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Register UserService as a singleton
builder.Services.AddSingleton<IUserService, UserService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}"
);

app.MapGet("/", context =>
{
    context.Response.Redirect("/LogIn/LogIn");
    return Task.CompletedTask;
});

app.Run();

