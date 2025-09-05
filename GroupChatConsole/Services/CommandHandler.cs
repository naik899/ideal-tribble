using Microsoft.SemanticKernel.ChatCompletion;
using GroupChatConsole.Models;

namespace GroupChatConsole.Services;

/// <summary>
/// Handles special commands like help, quit, etc.
/// </summary>
public class CommandHandler
{
    /// <summary>
    /// Handle special commands like help, quit, etc.
    /// </summary>
    public static async Task<bool> HandleCommandAsync(string input, ChatHistory chatHistory, OrchestrationService orchestrationService)
    {
        input = input.Trim();

        return input.ToLowerInvariant() switch
        {
            "quit" or "exit" => await HandleQuitCommandAsync(),
            "help" => await HandleHelpCommandAsync(),
            "agents" => await HandleAgentsCommandAsync(),
            "clear" => await HandleClearCommandAsync(chatHistory),
            "strategy" => await HandleStrategyCommandAsync(orchestrationService),
            "custom" => await HandleCustomStrategyCommandAsync(orchestrationService),
            "magentic" => await HandleMagenticStrategyCommandAsync(orchestrationService),
            _ => false // Not a command, process as normal message
        };
    }

    private static Task<bool> HandleQuitCommandAsync()
    {
        Console.WriteLine("Thanks for the conversation! Goodbye!");
        return Task.FromResult(true); // Exit the main loop
    }

    private static Task<bool> HandleHelpCommandAsync()
    {
        ShowHelp();
        return Task.FromResult(true); // Continue the loop
    }

    private static Task<bool> HandleAgentsCommandAsync()
    {
        ShowAgents();
        return Task.FromResult(true); // Continue the loop
    }

    private static Task<bool> HandleClearCommandAsync(ChatHistory chatHistory)
    {
        chatHistory.Clear();
        Console.WriteLine("Conversation history cleared.\n");
        return Task.FromResult(true); // Continue the loop
    }

    private static Task<bool> HandleStrategyCommandAsync(OrchestrationService orchestrationService)
    {
        ShowStrategyInfo(orchestrationService);
        return Task.FromResult(true); // Continue the loop
    }

    private static Task<bool> HandleCustomStrategyCommandAsync(OrchestrationService orchestrationService)
    {
        orchestrationService.SetStrategy(OrchestrationService.OrchestrationStrategy.CustomOrchestration);
        Console.WriteLine("Switched to Custom Orchestration strategy.\n");
        return Task.FromResult(true); // Continue the loop
    }

    private static Task<bool> HandleMagenticStrategyCommandAsync(OrchestrationService orchestrationService)
    {
        orchestrationService.SetStrategy(OrchestrationService.OrchestrationStrategy.MagenticOrchestration);
        Console.WriteLine("Switched to Magentic Orchestration strategy.\n");
        return Task.FromResult(true); // Continue the loop
    }

    /// <summary>
    /// Show help information
    /// </summary>
    private static void ShowHelp()
    {
        Console.WriteLine("\n=== Available Commands ===");
        Console.WriteLine("help     - Show this help message");
        Console.WriteLine("agents   - List all available agents");
        Console.WriteLine("strategy - Show current orchestration strategy");
        Console.WriteLine("custom   - Switch to Custom Orchestration strategy");
        Console.WriteLine("magentic - Switch to Magentic Orchestration strategy");
        Console.WriteLine("clear    - Clear conversation history");
        Console.WriteLine("quit     - Exit the application");
        Console.WriteLine("\n=== Sample Topics ===");
        Console.WriteLine("• Should we prioritize speed of delivery or code quality?");
        Console.WriteLine("• How should we handle technical debt in our current project?");
        Console.WriteLine("• What's the best approach for implementing user authentication?");
        Console.WriteLine("• How can we improve our deployment process?");
        Console.WriteLine("• Should we use microservices or monolithic architecture?");
        Console.WriteLine("• How do we balance feature development with security requirements?");
        Console.WriteLine("• What testing strategy should we adopt for this new feature?");
        Console.WriteLine("• Can you analyze this code snippet for quality issues?");
        Console.WriteLine("• What's the current status of our production deployment?");
        Console.WriteLine("• Can you perform a security scan on our application?");
        Console.WriteLine();
    }

    /// <summary>
    /// Show available agents and their backgrounds
    /// </summary>
    private static void ShowAgents()
    {
        Console.WriteLine("\n=== Available Agents ===");
        
        foreach (var agent in AgentConfigurations.AllAgents)
        {
            Console.WriteLine($"{agent.Emoji} {agent.Name,-15} - {agent.Description}");
            Console.WriteLine($"                   Values: {string.Join(", ", agent.Values)}");
            Console.WriteLine();
        }
        
        Console.WriteLine("The coordinator automatically selects relevant agents based on your questions.");
        Console.WriteLine();
    }

    /// <summary>
    /// Show current orchestration strategy information
    /// </summary>
    private static void ShowStrategyInfo(OrchestrationService orchestrationService)
    {
        Console.WriteLine("\n=== Orchestration Strategy ===");
        Console.WriteLine($"Current Strategy: {orchestrationService.CurrentStrategy}");
        Console.WriteLine();
        Console.WriteLine("Available Strategies:");
        Console.WriteLine("• CustomOrchestration  - Uses coordinator to select 1-2 agents based on context");
        Console.WriteLine("• MagenticOrchestration - Uses Semantic Kernel's built-in GroupChatOrchestration");
        Console.WriteLine();
        Console.WriteLine("Commands:");
        Console.WriteLine("• custom   - Switch to Custom Orchestration");
        Console.WriteLine("• magentic - Switch to Magentic Orchestration");
        Console.WriteLine();
    }
}