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


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Retrieve the API key from appsettings.json
var key = configuration["SMTP:key"];
var apiKey = configuration["Mistral:key"];
var modelId = configuration["Mistral:name"];


builder.Services.AddChatClient(new OllamaChatClient(
    new Uri("http://localhost:11434"), "deepseek-r1:7b") // Replace with your preferred model if needed
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

