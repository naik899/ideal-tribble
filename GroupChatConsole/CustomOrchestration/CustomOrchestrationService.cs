using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using GroupChatConsole.Common;

namespace GroupChatConsole.CustomOrchestration;

/// <summary>
/// Custom orchestration service for managing chat conversations and agent interactions
/// </summary>
public class CustomOrchestrationService
{
    private readonly ChatCompletionAgent[] _agents;
    private readonly ChatCompletionAgent _coordinator;

    public CustomOrchestrationService(ChatCompletionAgent[] agents, ChatCompletionAgent coordinator)
    {
        _agents = agents;
        _coordinator = coordinator;
    }

    /// <summary>
    /// Process user message and coordinate agent responses
    /// </summary>
    public async Task ProcessUserMessageAsync(string userInput, ChatHistory chatHistory)
    {
        // Add user message to history
        chatHistory.AddUserMessage(userInput);

        try
        {
            // Get coordinator's recommendation for which agents should respond
            var selectedAgents = await SelectAgentsAsync(_coordinator, userInput, _agents, chatHistory);

            // Get responses from selected agents
            foreach (var agent in selectedAgents)
            {
                // Get agent color and display header
                var agentColor = AgentColorHelper.GetAgentColor(agent.Name ?? "Unknown");
                var originalBackgroundColor = Console.BackgroundColor;
                var originalForegroundColor = Console.ForegroundColor;

                try
                {
                    // Set agent-specific background color
                    Console.BackgroundColor = agentColor;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"\n[{agent.Name}]:");
                    Console.BackgroundColor = originalBackgroundColor;
                    Console.ForegroundColor = originalForegroundColor;

                    // Display agent response with colored background
                    Console.BackgroundColor = agentColor;
                    Console.ForegroundColor = ConsoleColor.Black;
                    
                    string agentResponse = await AgentResponseHelper.GetAgentResponseAsync(agent, chatHistory);
                    Console.WriteLine(agentResponse);
                    
                    // Reset colors
                    Console.BackgroundColor = originalBackgroundColor;
                    Console.ForegroundColor = originalForegroundColor;

                    chatHistory.AddAssistantMessage(agentResponse);

                    // Small delay for better readability
                    await Task.Delay(500);
                }
                finally
                {
                    // Ensure colors are reset even if an exception occurs
                    Console.BackgroundColor = originalBackgroundColor;
                    Console.ForegroundColor = originalForegroundColor;
                }
            }

            Console.WriteLine(); // Extra line for spacing
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine("Please try again.\n");
        }
    }

    /// <summary>
    /// Use coordinator to select which agents should respond based on current input and conversation history
    /// </summary>
    private async Task<ChatCompletionAgent[]> SelectAgentsAsync(ChatCompletionAgent coordinator, string userInput, ChatCompletionAgent[] agents, ChatHistory chatHistory)
    {
        // Create a comprehensive prompt that includes conversation context
        var conversationContext = ConversationContextHelper.BuildConversationContext(chatHistory);
        var coordinatorPrompt = $"""
            Conversation Context:
            {conversationContext}
            
            Current User Input: "{userInput}"
            
            Based on the conversation history and current input, which agents should respond? 
            Consider:
            1. The current question/topic
            2. Previous agents who have already contributed
            3. Whether this is a follow-up question or new topic
            4. The expertise needed for the current discussion
            
            Select 1-2 most relevant agents. Respond with ONLY the agent names, separated by commas.
            """;

        var coordinatorHistory = new ChatHistory();
        coordinatorHistory.AddUserMessage(coordinatorPrompt);

        string selectedAgentsText = await AgentResponseHelper.GetAgentResponseAsync(coordinator, coordinatorHistory);

        // Parse selected agents
        var selectedAgentNames = selectedAgentsText
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(name => name.Trim())
            .ToArray();

        var selectedAgents = agents.Where(agent =>
            selectedAgentNames.Any(name =>
                string.Equals(agent.Name, name, StringComparison.OrdinalIgnoreCase)))
            .ToArray();

        return selectedAgents;
    }
}
