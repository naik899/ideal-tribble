using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.ChatCompletion;

namespace GroupChatConsole;

internal class Program
{
    private static async Task Main(string[] args)
    {
        // Build kernel with OpenAI chat completion
        var builder = Kernel.CreateBuilder();
        builder.AddOpenAIChatCompletion(
            modelId: Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-3.5-turbo",
            apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "" );
        var kernel = builder.Build();

        // Define agents with different expertise
        var chef = new ChatCompletionAgent(kernel)
        {
            Name = "Chef",
            Instructions = "You are a helpful chef who offers cooking tips and recipes."
        };

        var poet = new ChatCompletionAgent(kernel)
        {
            Name = "Poet",
            Instructions = "You are a creative poet who responds only in rhymes."
        };

        var manager = new ChatCompletionAgent(kernel)
        {
            Name = "Manager",
            Instructions = "You are an orchestration agent. Decide which specialist should answer the user's questions."
        };

        var groupChat = new GroupChat(kernel, manager, new[] { chef, poet });

        Console.WriteLine("Enter your message (type 'exit' to quit):");
        while (true)
        {
            Console.Write("User: ");
            var input = Console.ReadLine();
            if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            await foreach (var message in groupChat.InvokeAsync(input!))
            {
                Console.WriteLine($"{message.AuthorName}: {message.Content}");
            }
        }
    }
}
