using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using GroupChatConsole.Models;
using GroupChatConsole.Plugins;

namespace GroupChatConsole.Services;

/// <summary>
/// Factory for creating AI agents based on configuration
/// </summary>
public class AgentFactory
{
    /// <summary>
    /// Create all software development team agents
    /// </summary>
    public static ChatCompletionAgent[] CreateAgents(Kernel kernel)
    {
        return AgentConfigurations.AllAgents
            .Select(config => CreateChatCompletionAgent(kernel, config))
            .ToArray();
    }

    /// <summary>
    /// Create coordinator agent to manage conversation flow
    /// </summary>
    public static ChatCompletionAgent CreateCoordinator(Kernel kernel)
    {
        return CreateChatCompletionAgent(kernel, AgentConfigurations.Coordinator);
    }

    /// <summary>
    /// Helper method to create a ChatCompletionAgent from configuration
    /// </summary>
    private static ChatCompletionAgent CreateChatCompletionAgent(Kernel kernel, AgentConfiguration config)
    {
        var agent = new ChatCompletionAgent()
        {
            Kernel = kernel,
            Name = config.Name,
            Instructions = config.Instructions
        };

        // Attach specific plugins based on agent role
        AttachPluginsToAgent(kernel, agent, config.Name);

        return agent;
    }

    /// <summary>
    /// Attach relevant plugins to specific agents
    /// </summary>
    private static void AttachPluginsToAgent(Kernel kernel, ChatCompletionAgent agent, string agentName)
    {
        switch (agentName)
        {
            case "SeniorDeveloper":
                kernel.Plugins.AddFromObject(new CodeAnalysisPlugin(), "CodeAnalysis");
                break;
            case "DevOpsEngineer":
                kernel.Plugins.AddFromObject(new DevOpsPlugin(), "DevOps");
                break;
            case "SecurityEngineer":
                kernel.Plugins.AddFromObject(new SecurityPlugin(), "Security");
                break;
            // Add more plugin assignments as needed
        }
    }
}
