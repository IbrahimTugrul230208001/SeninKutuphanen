using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using learningASP.NET_CORE.Services;
using Microsoft.SemanticKernel;
using OpenAI;
using System.ClientModel;
using learningASP.NET_CORE.Hubs;
using Microsoft.AspNetCore.Builder;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Retrieve the API key from appsettings.json
var key = configuration["SMTP:key"];
var apikey = configuration["Gemini:Apikey"];
builder.Services
    .AddKernel()
    .AddOpenAIChatCompletion(
        modelId: "google/gemini-2.0-pro-exp-02-05:free",
        openAIClient: new OpenAIClient(
            credential: new ApiKeyCredential($"{apikey}"),
            options: new OpenAIClientOptions
            {
                Endpoint = new Uri("https://openrouter.ai/api/v1")
            }
        )
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
    context.Response.Redirect("/LogIn/LogIn");
    return Task.CompletedTask;
});


app.MapHub<AIHub>("ai-hub");
app.Run();

