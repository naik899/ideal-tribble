using GroupChatConsole.CustomOrchestration;
using GroupChatConsole.MagenticOrchestration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;

namespace GroupChatConsole.Services;

/// <summary>
/// Service for managing different orchestration strategies
/// </summary>
public class OrchestrationService
{
    public enum OrchestrationStrategy
    {
        CustomOrchestration,
        MagenticOrchestration
    }

    private OrchestrationStrategy _currentStrategy = OrchestrationStrategy.MagenticOrchestration; // Make MagenticOrchestration default
    private readonly Kernel _kernel;
    private readonly ChatCompletionAgent[] _agents;
    private readonly ChatCompletionAgent _coordinator;
    private readonly CustomOrchestrationService _customOrchestration;
    private readonly MagenticOrchestrationService _magenticOrchestration;

    public OrchestrationService(Kernel kernel, ChatCompletionAgent[] agents, ChatCompletionAgent coordinator)
    {
        _kernel = kernel;
        _agents = agents;
        _coordinator = coordinator;
        _customOrchestration = new CustomOrchestrationService(agents, coordinator);
        _magenticOrchestration = new MagenticOrchestrationService(kernel, agents, "Software Development Discussion");
    }

    /// <summary>
    /// Get the current orchestration strategy
    /// </summary>
    public OrchestrationStrategy CurrentStrategy => _currentStrategy;

    /// <summary>
    /// Set the orchestration strategy
    /// </summary>
    public void SetStrategy(OrchestrationStrategy strategy)
    {
        _currentStrategy = strategy;
    }

    /// <summary>
    /// Process user message using the current orchestration strategy
    /// </summary>
    public async Task ProcessUserMessageAsync(string userInput, ChatHistory chatHistory)
    {
        switch (_currentStrategy)
        {
            case OrchestrationStrategy.CustomOrchestration:
                await _customOrchestration.ProcessUserMessageAsync(userInput, chatHistory);
                break;
            case OrchestrationStrategy.MagenticOrchestration:
                var result = await _magenticOrchestration.RunOrchestrationAsync(userInput, chatHistory);
                chatHistory.AddAssistantMessage(result);
                break;
            default:
                throw new ArgumentException($"Unknown orchestration strategy: {_currentStrategy}");
        }
    }

}