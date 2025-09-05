#pragma warning disable SKEXP0110
using Microsoft.SemanticKernel.Agents.Orchestration.GroupChat;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;

namespace GroupChatConsole.MagenticOrchestration;

/// <summary>
/// AI Group Chat Manager for controlling the conversation flow
/// </summary>
public sealed class AIGroupChatManager(string topic, IChatCompletionService chatCompletion) : GroupChatManager
{
    private int _invocationCount = 0;
    
    private static class Prompts
    {
        public static string Termination(string topic) =>
            $"""
            Should we end this discussion about "{topic}"?
            Respond with True to end, False to continue.
            """;

        public static string Selection(string topic, string participants) =>
            $"""
            Select the best agent to respond to: "{topic}"
            
            Available agents: {participants}
            
            Consider the conversation context and select the most appropriate agent:
            - Deployment, infrastructure, monitoring → DevOpsEngineer
            - Code quality, architecture, development → SeniorDeveloper  
            - Security, vulnerabilities, compliance → SecurityEngineer
            - Testing, QA, validation → QAEngineer
            - User experience, design → UXDesigner
            - Data analysis, ML, analytics → DataScientist
            - Project management, requirements → ProductManager
            - Technical leadership, coordination → TechLead
            
            Respond with just the agent name.
            """;

        public static string Filter(string topic) =>
            $"""
            You are mediator that guides a discussion on the topic of '{topic}'.
            You have just concluded the discussion.
            Please summarize the discussion and provide a closing statement.
            """;
    }

    /// <inheritdoc/>
    public override ValueTask<GroupChatManagerResult<string>> FilterResults(ChatHistory history, CancellationToken cancellationToken = default) =>
        this.GetResponseAsync<string>(history, Prompts.Filter(topic), cancellationToken);

    /// <inheritdoc/>
    public override async ValueTask<GroupChatManagerResult<string>> SelectNextAgent(ChatHistory history, GroupChatTeam team, CancellationToken cancellationToken = default)
    {
        _invocationCount++;
        Console.WriteLine($"SelectNextAgent called (invocation #{_invocationCount})");
        
        if (_invocationCount > 5) // Allow more invocations for proper group chat
        {
            Console.WriteLine("Forcing termination due to too many invocations");
            return new GroupChatManagerResult<string>("TERMINATE");
        }
        
        return await this.GetResponseAsync<string>(history, Prompts.Selection(topic, team.FormatList()), cancellationToken);
    }

    /// <inheritdoc/>
    public override ValueTask<GroupChatManagerResult<bool>> ShouldRequestUserInput(ChatHistory history, CancellationToken cancellationToken = default) =>
        ValueTask.FromResult(new GroupChatManagerResult<bool>(false) { Reason = "The AI group chat manager does not request user input." });

    /// <inheritdoc/>
    public override async ValueTask<GroupChatManagerResult<bool>> ShouldTerminate(ChatHistory history, CancellationToken cancellationToken = default)
    {
        // Force termination if we've had too many invocations
        if (_invocationCount > 5)
        {
            Console.WriteLine("Forcing termination due to invocation limit");
            return new GroupChatManagerResult<bool>(true) { Reason = "Invocation limit reached" };
        }
        
        // Check if we have any responses yet
        if (history.Count <= 1) // Only user input, no agent responses
        {
            return new GroupChatManagerResult<bool>(false) { Reason = "No agent responses yet" };
        }
        
        // Allow more agent interactions before considering termination
        if (history.Count < 3) // Need at least user input + 2 agent responses
        {
            return new GroupChatManagerResult<bool>(false) { Reason = "Need more agent responses" };
        }
        
        // If we have enough responses, consider terminating
        var result = await this.GetResponseAsync<bool>(history, Prompts.Termination(topic), cancellationToken);
        Console.WriteLine($"ShouldTerminate result: {result.Value}");
        return result;
    }

    private async ValueTask<GroupChatManagerResult<TValue>> GetResponseAsync<TValue>(ChatHistory history, string prompt, CancellationToken cancellationToken = default)
    {
        try
        {
            // Use simple text completion instead of structured JSON
            var request = new ChatHistory([.. history, new ChatMessageContent(AuthorRole.System, prompt)]);
            var response = await chatCompletion.GetChatMessageContentAsync(request, executionSettings: null, kernel: null, cancellationToken);
            var responseText = response.ToString()?.Trim() ?? "";
            
            Console.WriteLine($"AI Group Chat Manager Response: {responseText}");
            
            // Handle different return types
            if (typeof(TValue) == typeof(string))
            {
                // For agent selection, clean up the response
                var cleanResponse = responseText.Replace("```", "").Replace("json", "").Trim();
                
                // Check for all possible agents
                if (cleanResponse.Contains("DevOpsEngineer"))
                    return (GroupChatManagerResult<TValue>)(object)new GroupChatManagerResult<string>("DevOpsEngineer");
                if (cleanResponse.Contains("SeniorDeveloper"))
                    return (GroupChatManagerResult<TValue>)(object)new GroupChatManagerResult<string>("SeniorDeveloper");
                if (cleanResponse.Contains("SecurityEngineer"))
                    return (GroupChatManagerResult<TValue>)(object)new GroupChatManagerResult<string>("SecurityEngineer");
                if (cleanResponse.Contains("QAEngineer"))
                    return (GroupChatManagerResult<TValue>)(object)new GroupChatManagerResult<string>("QAEngineer");
                if (cleanResponse.Contains("UXDesigner"))
                    return (GroupChatManagerResult<TValue>)(object)new GroupChatManagerResult<string>("UXDesigner");
                if (cleanResponse.Contains("DataScientist"))
                    return (GroupChatManagerResult<TValue>)(object)new GroupChatManagerResult<string>("DataScientist");
                if (cleanResponse.Contains("ProductManager"))
                    return (GroupChatManagerResult<TValue>)(object)new GroupChatManagerResult<string>("ProductManager");
                if (cleanResponse.Contains("TechLead"))
                    return (GroupChatManagerResult<TValue>)(object)new GroupChatManagerResult<string>("TechLead");
                
                // Default fallback
                return (GroupChatManagerResult<TValue>)(object)new GroupChatManagerResult<string>("TechLead");
            }
            else if (typeof(TValue) == typeof(bool))
            {
                // For termination, look for True/False
                var isTrue = responseText.Contains("True", StringComparison.OrdinalIgnoreCase);
                return (GroupChatManagerResult<TValue>)(object)new GroupChatManagerResult<bool>(isTrue);
            }
            else
            {
                // For filter results, return the text as string
                return (GroupChatManagerResult<TValue>)(object)new GroupChatManagerResult<string>(responseText);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetResponseAsync: {ex.Message}");
            
            // Fallback based on type
            if (typeof(TValue) == typeof(string))
            {
                return (GroupChatManagerResult<TValue>)(object)new GroupChatManagerResult<string>("TechLead");
            }
            else if (typeof(TValue) == typeof(bool))
            {
                return (GroupChatManagerResult<TValue>)(object)new GroupChatManagerResult<bool>(true);
            }
            else
            {
                return (GroupChatManagerResult<TValue>)(object)new GroupChatManagerResult<string>("Discussion completed.");
            }
        }
    }
}
