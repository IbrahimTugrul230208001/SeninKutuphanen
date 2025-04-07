using learningASP.NET_CORE.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using learningASP.NET_CORE.Services;
using System.Text;

public class AIService(IHubContext<AIHub> hubContext, IChatCompletionService chatCompletionService, Kernel kernel) : IAIServices
{
    public async Task GetMessageStreamAsync(string prompt, string connectionId, CancellationToken? cancellationToken = default!)
    {
        OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        var history = HistoryService.GetChatHistory(connectionId);
        history.AddUserMessage(prompt);

        const int maxRetries = 3;
        int attempt = 0;
        bool success = false;

        while (attempt < maxRetries && !success)
        {
            var responseBuilder = new StringBuilder();
            attempt++;

            try
            {
                await foreach (var response in chatCompletionService.GetStreamingChatMessageContentsAsync(
                    history,
                    executionSettings: openAIPromptExecutionSettings,
                    kernel: kernel))
                {
                    cancellationToken?.ThrowIfCancellationRequested();

                    if (!string.IsNullOrWhiteSpace(response.ToString()))
                    {
                        responseBuilder.Append(response.ToString());
                    }
                }

                string finalResponse = responseBuilder.ToString().Trim();

                if (!string.IsNullOrWhiteSpace(finalResponse))
                {
                    // Send and save
                    await hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", finalResponse);
                    history.AddAssistantMessage(finalResponse);
                    success = true;
                }
                else
                {
                    Console.WriteLine($"[Attempt {attempt}] Empty response, retrying...");
                    await Task.Delay(1000); // Wait before retry
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Attempt {attempt}] Exception: {ex.Message}");
                await Task.Delay(1000); // Wait before retry
            }
        }

        if (!success)
        {
            const string failMsg = "⚠️ Sorry, the AI didn't respond. Please try again shortly.";
            await hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", failMsg);
            history.AddAssistantMessage(failMsg);
        }
    }

}


