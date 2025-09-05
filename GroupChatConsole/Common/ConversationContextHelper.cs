using Microsoft.SemanticKernel.ChatCompletion;

namespace GroupChatConsole.Common;

/// <summary>
/// Helper class for building conversation context
/// </summary>
public static class ConversationContextHelper
{
    /// <summary>
    /// Build conversation context from chat history
    /// </summary>
    public static string BuildConversationContext(ChatHistory chatHistory)
    {
        if (chatHistory.Count == 0)
        {
            return "This is the start of a new conversation.";
        }

        var context = new List<string>();
        var recentMessages = chatHistory.TakeLast(6).ToList(); // Last 3 exchanges

        foreach (var message in recentMessages)
        {
            var role = message.Role.ToString();
            var content = message.Content?.ToString() ?? "";
            
            if (content.Length > 100)
            {
                content = content.Substring(0, 100) + "...";
            }

            context.Add($"{role}: {content}");
        }

        return string.Join("\n", context);
    }
}
