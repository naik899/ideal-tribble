using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace GroupChatConsole.Services;

/// <summary>
/// Service for initializing and managing the Semantic Kernel
/// </summary>
public class KernelService
{
    /// <summary>
    /// Initialize the Semantic Kernel with appropriate AI service
    /// </summary>
    public static Task<Kernel?> InitializeKernelAsync()
    {
        // Check for API keys (supports both OpenAI and Azure OpenAI)
        string? openAiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        string? azureKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_KEY");

        if (string.IsNullOrEmpty(openAiKey) && string.IsNullOrEmpty(azureKey))
        {
            Console.WriteLine("ERROR: Please set either OPENAI_API_KEY or AZURE_OPENAI_KEY environment variable.");
            return null;
        }

        var builder = Kernel.CreateBuilder();

        if (!string.IsNullOrEmpty(azureKey))
        {
            Console.WriteLine("Using Azure OpenAI...");
            builder.AddAzureOpenAIChatCompletion(
                deploymentName: Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME") ?? "gpt-4o",
                modelId: Environment.GetEnvironmentVariable("AZURE_OPENAI_MODEL") ?? "gpt-4o",
                endpoint: Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT") ?? "https://YOUR-RESOURCE-NAME.openai.azure.com/",
                apiKey: azureKey);
        }
        else
        {
            Console.WriteLine("Using OpenAI...");
            builder.AddOpenAIChatCompletion(
                modelId: Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-4o",
                apiKey: openAiKey!);
        }

        return Task.FromResult<Kernel?>(builder.Build());
    }
}
