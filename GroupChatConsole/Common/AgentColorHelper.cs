namespace GroupChatConsole.Common;

/// <summary>
/// Helper class for managing agent colors in console output
/// </summary>
public static class AgentColorHelper
{
    /// <summary>
    /// Get console color for each agent
    /// </summary>
    public static ConsoleColor GetAgentColor(string agentName)
    {
        return (agentName ?? string.Empty) switch
        {
            "ProductManager" => ConsoleColor.Cyan,
            "SeniorDeveloper" => ConsoleColor.Green,
            "DevOpsEngineer" => ConsoleColor.Yellow,
            "QAEngineer" => ConsoleColor.Magenta,
            "UXDesigner" => ConsoleColor.Red,
            "TechLead" => ConsoleColor.Blue,
            "DataScientist" => ConsoleColor.DarkCyan,
            "SecurityEngineer" => ConsoleColor.DarkRed,
            _ => ConsoleColor.Gray
        };
    }

    /// <summary>
    /// Display agent response with color coding
    /// </summary>
    public static void DisplayAgentResponse(string agentName, string response)
    {
        var agentColor = GetAgentColor(agentName);
        var originalBackground = Console.BackgroundColor;
        var originalForeground = Console.ForegroundColor;

        try
        {
            Console.BackgroundColor = agentColor;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"\n[{agentName}]:");
            Console.WriteLine(response);
        }
        finally
        {
            Console.BackgroundColor = originalBackground;
            Console.ForegroundColor = originalForeground;
        }
    }
}
