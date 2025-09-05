using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace GroupChatConsole.Plugins;

/// <summary>
/// Plugin for security analysis and recommendations
/// Used by SecurityEngineer agent
/// </summary>
public class SecurityPlugin
{
    [KernelFunction]
    [Description("Performs security vulnerability scan")]
    public string PerformSecurityScan(string target)
    {
        // Simulate security scan results
        var vulnerabilities = new List<SecurityIssue>
        {
            new() { Type = "SQL Injection", Severity = "High", Description = "Potential SQL injection in user input handling" },
            new() { Type = "XSS", Severity = "Medium", Description = "Cross-site scripting vulnerability in form validation" },
            new() { Type = "Weak Authentication", Severity = "High", Description = "Password policy not enforced" },
            new() { Type = "Insecure Communication", Severity = "Medium", Description = "HTTP used instead of HTTPS" }
        };

        var result = $"Security Scan Results for: {target}\n";
        result += "=" + new string('=', 40) + "\n";
        result += $"Scan Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n";
        result += $"Total Issues Found: {vulnerabilities.Count}\n\n";

        var highSeverity = vulnerabilities.Count(v => v.Severity == "High");
        var mediumSeverity = vulnerabilities.Count(v => v.Severity == "Medium");
        var lowSeverity = vulnerabilities.Count(v => v.Severity == "Low");

        result += $"High Severity: {highSeverity}\n";
        result += $"Medium Severity: {mediumSeverity}\n";
        result += $"Low Severity: {lowSeverity}\n\n";

        foreach (var vuln in vulnerabilities.OrderByDescending(v => v.Severity))
        {
            var icon = vuln.Severity == "High" ? "🔴" : vuln.Severity == "Medium" ? "🟡" : "🟢";
            result += $"{icon} [{vuln.Severity}] {vuln.Type}\n";
            result += $"   {vuln.Description}\n\n";
        }

        return result;
    }

    [KernelFunction]
    [Description("Generates security recommendations based on scan results")]
    public string GenerateSecurityRecommendations(string scanResults)
    {
        var recommendations = new List<string>();

        if (scanResults.Contains("SQL Injection"))
        {
            recommendations.Add("Implement parameterized queries and input validation");
            recommendations.Add("Use ORM frameworks with built-in SQL injection protection");
        }

        if (scanResults.Contains("XSS"))
        {
            recommendations.Add("Implement output encoding for all user-generated content");
            recommendations.Add("Use Content Security Policy (CSP) headers");
        }

        if (scanResults.Contains("Weak Authentication"))
        {
            recommendations.Add("Enforce strong password policies (min 12 chars, complexity)");
            recommendations.Add("Implement multi-factor authentication (MFA)");
            recommendations.Add("Use OAuth 2.0 or OpenID Connect for authentication");
        }

        if (scanResults.Contains("Insecure Communication"))
        {
            recommendations.Add("Force HTTPS for all communications");
            recommendations.Add("Implement HSTS (HTTP Strict Transport Security)");
        }

        if (scanResults.Contains("Missing Security Headers"))
        {
            recommendations.Add("Add security headers: X-Frame-Options, X-Content-Type-Options");
            recommendations.Add("Implement proper CORS configuration");
        }

        var result = "Security Recommendations:\n";
        result += "=" + new string('=', 25) + "\n";

        if (recommendations.Any())
        {
            foreach (var rec in recommendations)
            {
                result += $"• {rec}\n";
            }
        }
        else
        {
            result += "No specific recommendations based on scan results.\n";
        }

        result += "\nGeneral Security Best Practices:\n";
        result += "• Regular security audits and penetration testing\n";
        result += "• Keep all dependencies and frameworks updated\n";
        result += "• Implement proper logging and monitoring\n";
        result += "• Follow OWASP Top 10 guidelines\n";

        return result;
    }

    [KernelFunction]
    [Description("Checks compliance with security standards")]
    public string CheckCompliance(string standard, string systemDescription)
    {
        var complianceChecks = new Dictionary<string, List<string>>
        {
            ["OWASP"] = new() { "Input Validation", "Authentication", "Session Management", "Access Control" },
            ["PCI-DSS"] = new() { "Data Encryption", "Network Security", "Access Control", "Regular Testing" },
            ["GDPR"] = new() { "Data Minimization", "Consent Management", "Right to Erasure", "Data Portability" },
            ["SOC2"] = new() { "Security", "Availability", "Processing Integrity", "Confidentiality", "Privacy" }
        };

        var result = $"Compliance Check for {standard}:\n";
        result += "=" + new string('=', 30) + "\n";

        if (complianceChecks.ContainsKey(standard.ToUpper()))
        {
            var checks = complianceChecks[standard.ToUpper()];
            result += $"Required Controls:\n";
            
            foreach (var check in checks)
            {
                // Simple check based on system description
                var isCompliant = systemDescription.ToLower().Contains(check.ToLower().Replace(" ", ""));
                var status = isCompliant ? "✅" : "❌";
                result += $"{status} {check}\n";
            }

            var compliantCount = checks.Count(check => systemDescription.ToLower().Contains(check.ToLower().Replace(" ", "")));
            var compliancePercentage = (compliantCount * 100) / checks.Count;
            
            result += $"\nCompliance Score: {compliancePercentage}%\n";
            
            if (compliancePercentage < 70)
            {
                result += "⚠️ Action Required: Significant compliance gaps detected\n";
            }
            else if (compliancePercentage < 90)
            {
                result += "⚠️ Improvement Needed: Some compliance gaps detected\n";
            }
            else
            {
                result += "✅ Good: High compliance level\n";
            }
        }
        else
        {
            result += $"Unknown standard: {standard}\n";
            result += "Supported standards: OWASP, PCI-DSS, GDPR, SOC2\n";
        }

        return result;
    }

    [KernelFunction]
    [Description("Generates security incident response plan")]
    public string GenerateIncidentResponsePlan(string incidentType)
    {
        var phases = new Dictionary<string, List<string>>
        {
            ["Preparation"] = new() { "Establish incident response team", "Create communication plan", "Document procedures" },
            ["Identification"] = new() { "Detect and analyze the incident", "Assess impact and scope", "Document initial findings" },
            ["Containment"] = new() { "Isolate affected systems", "Prevent further damage", "Preserve evidence" },
            ["Eradication"] = new() { "Remove threat from systems", "Patch vulnerabilities", "Clean infected systems" },
            ["Recovery"] = new() { "Restore systems to normal operation", "Monitor for recurrence", "Validate security measures" },
            ["Lessons Learned"] = new() { "Conduct post-incident review", "Update procedures", "Improve security measures" }
        };

        var result = $"Security Incident Response Plan for: {incidentType}\n";
        result += "=" + new string('=', 50) + "\n\n";

        foreach (var phase in phases)
        {
            result += $"🔹 {phase.Key} Phase:\n";
            foreach (var step in phase.Value)
            {
                result += $"   • {step}\n";
            }
            result += "\n";
        }

        result += "Emergency Contacts:\n";
        result += "• Security Team Lead: +1-555-SECURITY\n";
        result += "• IT Operations: +1-555-IT-OPS\n";
        result += "• Legal/Compliance: +1-555-LEGAL\n";
        result += "• Management: +1-555-MGMT\n";

        return result;
    }

    private class SecurityIssue
    {
        public string Type { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
