using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using GroupChatConsole.Common;

namespace GroupChatConsole.MagenticOrchestration;

/// <summary>
/// Monitor for capturing agent responses
/// </summary>
public sealed class OrchestrationMonitor
{
    public ValueTask ResponseCallback(ChatMessageContent response)
    {
        // Display agent responses with color coding
        var agentName = response.AuthorName ?? "Unknown";
        AgentColorHelper.DisplayAgentResponse(agentName, response.Content ?? "");

        Console.WriteLine($"ResponseCallback completed for {agentName}");
        return ValueTask.CompletedTask;
    }
}
