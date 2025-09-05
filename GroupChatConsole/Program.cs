using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using GroupChatConsole.Services;

namespace GroupChatConsole;

/// <summary>
/// Interactive AI Chat with Multi-Cultural Perspectives
/// Demonstrates how to use AI agents representing different cultural backgrounds
/// to engage in thoughtful discussions and debates.
/// </summary>
internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("=== Software Development Team Discussion ===");
        Console.WriteLine("Chat with AI agents representing different roles in a software development team!");
        Console.WriteLine("Type 'help' for commands or start chatting.\n");

        // Initialize the kernel with AI service
        var kernel = await KernelService.InitializeKernelAsync();
        if (kernel == null)
        {
            Console.WriteLine("Press Enter to exit...");
            try { Console.ReadLine(); } catch { }
            return;
        }

        // Create agents and orchestration service
        var agents = Services.AgentFactory.CreateAgents(kernel);
        var coordinator = Services.AgentFactory.CreateCoordinator(kernel);
        var orchestrationService = new OrchestrationService(kernel, agents, coordinator);
        var chatHistory = new ChatHistory();

        await RunInteractiveChatAsync(orchestrationService, chatHistory);
    }

    /// <summary>
    /// Main interactive chat loop
    /// </summary>
    private static async Task RunInteractiveChatAsync(OrchestrationService orchestrationService, ChatHistory chatHistory)
    {
        Console.WriteLine("Ready! Start by asking a question or type 'help' for commands.\n");

        while (true)
        {
            Console.Write("You: ");
            string? userInput = await GetUserInputAsync();

            if (userInput == null) break; // Console not available
            if (string.IsNullOrWhiteSpace(userInput)) continue;

            // Handle special commands
            if (await CommandHandler.HandleCommandAsync(userInput, chatHistory, orchestrationService)) continue;

            // Process user message using the current orchestration strategy
            await orchestrationService.ProcessUserMessageAsync(userInput, chatHistory);
        }
    }

    /// <summary>
    /// Get user input with error handling
    /// </summary>
    private static Task<string?> GetUserInputAsync()
    {
        try
        {
            return Task.FromResult(Console.ReadLine());
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("Console input not available. Exiting...");
            return Task.FromResult<string?>(null);
        }
    }
}