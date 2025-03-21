using LibraryManagement.Business.Concrete;
using LibraryManagement.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using learningASP.NET_CORE.Services;
using Microsoft.SemanticKernel;
using OpenAI;
using System.ClientModel;
using learningASP.NET_CORE.Hubs;
using OllamaSharp.Models.Chat;
using learningASP.NET_CORE.ViewModels;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Retrieve the API key from appsettings.json
var key = configuration["SMTP:key"];
var apikey = configuration["DeepSeek:Apikey"];
builder.Services
    .AddKernel()
    .AddOpenAIChatCompletion(
        modelId: "deepseek/deepseek-r1-zero:free",
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
builder.Services.AddSingleton<AIService>();

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
app.MapPost("/chat", async (AIService aiService, ChatRequestVM chatRequest, CancellationToken cancellationToken)
    => await aiService.GetMessageStreamAsync(chatRequest.Prompt, chatRequest.ConnectionId, cancellationToken));
app.MapHub<AIHub>("ai-hub");
app.Run();

