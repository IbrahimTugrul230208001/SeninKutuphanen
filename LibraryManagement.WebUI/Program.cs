using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using learningASP.NET_CORE.Services;
using Microsoft.SemanticKernel;
using OpenAI;
using System.ClientModel;
using learningASP.NET_CORE.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection.Extensions;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Retrieve the API key from appsettings.json
var key = configuration["SMTP:key"];
string apiKey = configuration["Gemini:key"];
// Removes all whitespace characters (spaces, tabs, newlines, etc.)
string cleanedKey = new string(apiKey.Where(c => !char.IsWhiteSpace(c)).ToArray());
var skBuilder = Kernel.CreateBuilder();
#pragma warning disable SKEXP0070
builder.Services
    .AddKernel()
    .AddGoogleAIGeminiChatCompletion(
        modelId: "gemini-2.0-flash",
        apiKey: apiKey
    );




builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => policy.AllowAnyMethod()
                                       .AllowAnyHeader()
                                       .AllowCredentials()
                                       .SetIsOriginAllowed(s => true)));

builder.Services.AddSignalR();



builder.Services.AddControllersWithViews();

// Register UserService as a singleton
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IAIServices,AIService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.UseCors();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}"
);

app.MapGet("/", context =>
{
    context.Response.Redirect("/Dogrulama/Giris");
    return Task.CompletedTask;
});



app.MapHub<AIHub>("ai-hub");
app.Run();

