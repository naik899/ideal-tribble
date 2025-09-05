using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace GroupChatConsole.Plugins;

/// <summary>
/// Plugin for code analysis and quality assessment
/// Used by SeniorDeveloper agent
/// </summary>
public class CodeAnalysisPlugin
{
    [KernelFunction]
    [Description("Analyzes code quality and provides recommendations")]
    public string AnalyzeCodeQuality(string codeSnippet, string language = "C#")
    {
        // Simple code analysis logic
        var issues = new List<string>();
        var recommendations = new List<string>();

        // Check for common code quality issues
        if (codeSnippet.Contains("TODO") || codeSnippet.Contains("FIXME"))
        {
            issues.Add("Contains TODO/FIXME comments");
            recommendations.Add("Address technical debt by implementing TODO items");
        }

        if (codeSnippet.Contains("catch (Exception)"))
        {
            issues.Add("Generic exception handling");
            recommendations.Add("Use specific exception types for better error handling");
        }

        if (codeSnippet.Contains("var ") && codeSnippet.Split('\n').Length > 10)
        {
            issues.Add("Excessive use of 'var' in long methods");
            recommendations.Add("Consider explicit typing for better readability");
        }

        if (codeSnippet.Contains("public static void Main"))
        {
            issues.Add("Main method in non-console application");
            recommendations.Add("Consider dependency injection and proper architecture");
        }

        // Calculate complexity score (simplified)
        var lines = codeSnippet.Split('\n').Length;
        var complexity = lines > 50 ? "High" : lines > 20 ? "Medium" : "Low";

        var result = $"Code Analysis Results for {language}:\n";
        result += $"Complexity: {complexity} ({lines} lines)\n";
        
        if (issues.Any())
        {
            result += "\nIssues Found:\n";
            foreach (var issue in issues)
            {
                result += $"- {issue}\n";
            }
        }
        else
        {
            result += "\nNo major issues detected.\n";
        }

        if (recommendations.Any())
        {
            result += "\nRecommendations:\n";
            foreach (var rec in recommendations)
            {
                result += $"- {rec}\n";
            }
        }

        return result;
    }

    [KernelFunction]
    [Description("Suggests design patterns for the given code context")]
    public string SuggestDesignPattern(string context, string problem)
    {
        var patterns = new Dictionary<string, string[]>
        {
            ["state management"] = new[] { "State Pattern", "Observer Pattern", "Command Pattern" },
            ["object creation"] = new[] { "Factory Pattern", "Builder Pattern", "Singleton Pattern" },
            ["communication"] = new[] { "Observer Pattern", "Mediator Pattern", "Event Aggregator" },
            ["data access"] = new[] { "Repository Pattern", "Unit of Work", "Data Mapper" },
            ["error handling"] = new[] { "Chain of Responsibility", "Circuit Breaker", "Retry Pattern" }
        };

        var suggestions = new List<string>();
        
        foreach (var kvp in patterns)
        {
            if (context.ToLower().Contains(kvp.Key) || problem.ToLower().Contains(kvp.Key))
            {
                suggestions.AddRange(kvp.Value);
            }
        }

        if (!suggestions.Any())
        {
            suggestions.AddRange(new[] { "Strategy Pattern", "Adapter Pattern", "Decorator Pattern" });
        }

        return $"Suggested Design Patterns for '{problem}':\n" + 
               string.Join("\n", suggestions.Take(3).Select(p => $"- {p}"));
    }

    [KernelFunction]
    [Description("Calculates cyclomatic complexity of a code block")]
    public string CalculateComplexity(string code)
    {
        var complexity = 1; // Base complexity
        var keywords = new[] { "if", "while", "for", "foreach", "switch", "case", "catch", "&&", "||", "?" };
        
        foreach (var keyword in keywords)
        {
            complexity += code.Split(new[] { keyword }, StringSplitOptions.None).Length - 1;
        }

        var level = complexity <= 10 ? "Low" : complexity <= 20 ? "Medium" : "High";
        
        return $"Cyclomatic Complexity: {complexity} ({level})\n" +
               $"Recommendation: {(complexity > 15 ? "Consider refactoring to reduce complexity" : "Complexity is acceptable")}";
    }
}
