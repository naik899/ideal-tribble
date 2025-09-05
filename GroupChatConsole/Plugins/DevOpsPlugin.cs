using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace GroupChatConsole.Plugins;

/// <summary>
/// Plugin for DevOps operations and monitoring
/// Used by DevOpsEngineer agent
/// </summary>
public class DevOpsPlugin
{
    [KernelFunction]
    [Description("Checks deployment status and health of services")]
    public string CheckDeploymentStatus(string environment = "production")
    {
        // Simulate deployment status check
        var services = new[]
        {
            new { Name = "Web API", Status = "Healthy", Uptime = "99.9%", LastDeploy = "2 hours ago" },
            new { Name = "Database", Status = "Healthy", Uptime = "99.8%", LastDeploy = "1 day ago" },
            new { Name = "Cache Service", Status = "Warning", Uptime = "98.5%", LastDeploy = "3 hours ago" },
            new { Name = "Message Queue", Status = "Healthy", Uptime = "99.7%", LastDeploy = "6 hours ago" }
        };

        var result = $"Deployment Status for {environment.ToUpper()}:\n";
        result += "=" + new string('=', result.Length - 1) + "\n";

        foreach (var service in services)
        {
            var statusIcon = service.Status == "Healthy" ? "✅" : "⚠️";
            result += $"{statusIcon} {service.Name}: {service.Status} (Uptime: {service.Uptime})\n";
            result += $"   Last Deploy: {service.LastDeploy}\n\n";
        }

        return result;
    }

    [KernelFunction]
    [Description("Generates deployment pipeline configuration")]
    public string GeneratePipelineConfig(string projectType, string environment)
    {
        var config = projectType.ToLower() switch
        {
            "dotnet" => GenerateDotNetPipeline(environment),
            "node" => GenerateNodePipeline(environment),
            "python" => GeneratePythonPipeline(environment),
            _ => GenerateGenericPipeline(environment)
        };

        return config;
    }

    [KernelFunction]
    [Description("Analyzes system performance metrics")]
    public string AnalyzePerformanceMetrics()
    {
        // Simulate performance analysis
        var metrics = new
        {
            CpuUsage = "45%",
            MemoryUsage = "67%",
            DiskUsage = "23%",
            NetworkLatency = "12ms",
            ResponseTime = "156ms",
            Throughput = "1,250 req/min"
        };

        var alerts = new List<string>();
        if (int.Parse(metrics.MemoryUsage.Replace("%", "")) > 60)
            alerts.Add("High memory usage detected");
        if (int.Parse(metrics.ResponseTime.Replace("ms", "")) > 100)
            alerts.Add("Response time above threshold");

        var result = "Performance Metrics Analysis:\n";
        result += "=" + new string('=', 30) + "\n";
        result += $"CPU Usage: {metrics.CpuUsage}\n";
        result += $"Memory Usage: {metrics.MemoryUsage}\n";
        result += $"Disk Usage: {metrics.DiskUsage}\n";
        result += $"Network Latency: {metrics.NetworkLatency}\n";
        result += $"Response Time: {metrics.ResponseTime}\n";
        result += $"Throughput: {metrics.Throughput}\n\n";

        if (alerts.Any())
        {
            result += "⚠️ Alerts:\n";
            foreach (var alert in alerts)
            {
                result += $"- {alert}\n";
            }
        }
        else
        {
            result += "✅ All metrics within normal ranges\n";
        }

        return result;
    }

    [KernelFunction]
    [Description("Suggests infrastructure improvements")]
    public string SuggestInfrastructureImprovements(string currentSetup)
    {
        var suggestions = new List<string>();

        if (currentSetup.ToLower().Contains("single server"))
        {
            suggestions.Add("Consider load balancing for high availability");
            suggestions.Add("Implement auto-scaling groups");
        }

        if (currentSetup.ToLower().Contains("manual deployment"))
        {
            suggestions.Add("Implement CI/CD pipeline automation");
            suggestions.Add("Add automated testing in deployment process");
        }

        if (!currentSetup.ToLower().Contains("monitoring"))
        {
            suggestions.Add("Implement comprehensive monitoring and alerting");
            suggestions.Add("Add log aggregation and analysis");
        }

        if (!currentSetup.ToLower().Contains("backup"))
        {
            suggestions.Add("Implement automated backup strategies");
            suggestions.Add("Add disaster recovery procedures");
        }

        var result = "Infrastructure Improvement Suggestions:\n";
        result += "=" + new string('=', 40) + "\n";

        if (suggestions.Any())
        {
            foreach (var suggestion in suggestions)
            {
                result += $"• {suggestion}\n";
            }
        }
        else
        {
            result += "Current setup looks good! Consider regular reviews.\n";
        }

        return result;
    }

    private string GenerateDotNetPipeline(string environment)
    {
        return $@"# .NET Deployment Pipeline for {environment}
name: Deploy .NET App

on:
  push:
    branches: [ main, develop ]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0'
    - name: Restore dependencies
      run: dotnet restore
    - name: Build
      run: dotnet build --configuration Release
    - name: Test
      run: dotnet test --configuration Release
    - name: Publish
      run: dotnet publish --configuration Release --output ./publish
    - name: Deploy to {environment}
      run: echo 'Deploying to {environment}...'";
    }

    private string GenerateNodePipeline(string environment)
    {
        return $@"# Node.js Deployment Pipeline for {environment}
name: Deploy Node App

on:
  push:
    branches: [ main, develop ]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    - name: Setup Node.js
      uses: actions/setup-node@v3
      with:
        node-version: '18'
    - name: Install dependencies
      run: npm ci
    - name: Run tests
      run: npm test
    - name: Build
      run: npm run build
    - name: Deploy to {environment}
      run: echo 'Deploying to {environment}...'";
    }

    private string GeneratePythonPipeline(string environment)
    {
        return $@"# Python Deployment Pipeline for {environment}
name: Deploy Python App

on:
  push:
    branches: [ main, develop ]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    - name: Setup Python
      uses: actions/setup-python@v4
      with:
        python-version: '3.11'
    - name: Install dependencies
      run: pip install -r requirements.txt
    - name: Run tests
      run: pytest
    - name: Deploy to {environment}
      run: echo 'Deploying to {environment}...'";
    }

    private string GenerateGenericPipeline(string environment)
    {
        return $@"# Generic Deployment Pipeline for {environment}
name: Deploy Application

on:
  push:
    branches: [ main, develop ]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    - name: Build
      run: echo 'Building application...'
    - name: Test
      run: echo 'Running tests...'
    - name: Deploy to {environment}
      run: echo 'Deploying to {environment}...'";
    }
}
