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

        // Use StringBuilder to accumulate the full response
        var responseBuilder = new StringBuilder();

        try
        {
            await foreach (var response in chatCompletionService.GetStreamingChatMessageContentsAsync(
                history,
                executionSettings: openAIPromptExecutionSettings,
                kernel: kernel))
            {
                cancellationToken?.ThrowIfCancellationRequested();

                // Append each chunk of the response to the buffer
                responseBuilder.Append(response.ToString());
            }

            // Get the final response after the streaming is complete
            string finalResponse = responseBuilder.ToString();

            // Send the final response as one message
            await hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", finalResponse);

            // Add the full response to history (after streaming completes)
            history.AddAssistantMessage(finalResponse);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}


