using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;

namespace GroupChatConsole.Common;

/// <summary>
/// Helper class for getting agent responses
/// </summary>
public static class AgentResponseHelper
{
    /// <summary>
    /// Get response from a single agent
    /// </summary>
    public static async Task<string> GetAgentResponseAsync(ChatCompletionAgent agent, ChatHistory history)
    {
        string response = "";
        await foreach (var responseItem in agent.InvokeAsync(history))
        {
            response = responseItem.Message.Content ?? "";
        }
        return response;
    }
}
