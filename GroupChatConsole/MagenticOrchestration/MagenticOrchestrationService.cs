#pragma warning disable SKEXP0110
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Orchestration.GroupChat;
using Microsoft.SemanticKernel.Agents.Runtime.InProcess;
using Microsoft.SemanticKernel.ChatCompletion;
using GroupChatConsole.Common;

namespace GroupChatConsole.MagenticOrchestration;

/// <summary>
/// Magentic orchestration service using Semantic Kernel's built-in GroupChatOrchestration
/// </summary>
public class MagenticOrchestrationService
{
    private readonly Kernel _kernel;
    private readonly ChatCompletionAgent[] _agents;
    private readonly string _topic;

    public MagenticOrchestrationService(Kernel kernel, ChatCompletionAgent[] agents, string topic = "Software Development Discussion")
    {
        _kernel = kernel;
        _agents = agents;
        _topic = topic;
    }

    /// <summary>
    /// Run the magentic orchestration with AI group chat manager
    /// </summary>
    public async Task<string> RunOrchestrationAsync(string userInput, ChatHistory chatHistory)
    {
        try
        {
            // Create a monitor to capture agent responses
            var monitor = new OrchestrationMonitor();

            // Create the AI group chat manager
            var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
            var groupChatManager = new AIGroupChatManager(_topic, chatCompletionService)
            {
                MaximumInvocationCount = 5 // Allow more invocations for proper group chat
            };

            // Create a logger factory
            var loggerFactory = LoggerFactory.Create(builder => 
                builder.AddConsole().SetMinimumLevel(LogLevel.Warning));

            // Debug: Show which agents are being used
            Console.WriteLine($"Available agents: {string.Join(", ", _agents.Select(a => a.Name))}");
            Console.WriteLine($"Starting GroupChatOrchestration for: {userInput}");
            
            // Create the orchestration
            var orchestration = new GroupChatOrchestration(groupChatManager, _agents)
            {
                LoggerFactory = loggerFactory,
                ResponseCallback = monitor.ResponseCallback,
            };

            // Start the runtime
            await using var runtime = new InProcessRuntime();
            await runtime.StartAsync();

            // Use the full GroupChatOrchestration workflow
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            
            try
            {
                var result = await orchestration.InvokeAsync(userInput, runtime, cts.Token);
                var text = await result.GetValueAsync(TimeSpan.FromSeconds(60), cts.Token);

                await runtime.RunUntilIdleAsync();

                Console.WriteLine($"GroupChatOrchestration completed. Result: {text?.Substring(0, Math.Min(100, text?.Length ?? 0))}...");
                return text ?? "No response generated.";
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("GroupChatOrchestration timed out after 30 seconds");
                return "GroupChatOrchestration timed out. The AI group chat manager may need more time to coordinate agents.";
            }
        }
        catch (Exception ex)
        {
            return $"Error in magentic orchestration: {ex.Message}";
        }
    }
}
