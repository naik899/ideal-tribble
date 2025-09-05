# Interactive AI Chat - Software Development Team

A sophisticated AI chat application that brings together agents representing different roles in a software development organization to engage in thoughtful discussions and debates. Based on the Microsoft Semantic Kernel example, this application demonstrates how AI agents can provide diverse perspectives on software development challenges and decisions.

## 🏗️ Project Structure

The application has been refactored into a clean, modular architecture:

```
GroupChatConsole/
├── Models/
│   └── AgentConfiguration.cs      # Agent definitions and configurations
├── Services/
│   ├── AgentFactory.cs           # Factory for creating AI agents
│   ├── ChatService.cs            # Chat conversation management
│   ├── CommandHandler.cs         # Command processing (help, quit, etc.)
│   └── KernelService.cs          # Semantic Kernel initialization
├── Program.cs                    # Main application entry point
└── GroupChatConsole.csproj       # Project configuration
```

## 🤖 Available Agents

The application features 8 software development team members:

1. **📊 ProductManager** - Product manager focused on user needs and business value
   - Values: user experience, business value, market research
2. **👨‍💻 SeniorDeveloper** - Senior software developer focused on code quality and architecture
   - Values: code quality, architecture, best practices
3. **🔧 DevOpsEngineer** - DevOps engineer focused on deployment, monitoring, and infrastructure
   - Values: automation, reliability, scalability
4. **🧪 QAEngineer** - QA engineer focused on testing, quality assurance, and risk mitigation
   - Values: quality assurance, testing, risk mitigation
5. **🎨 UXDesigner** - UX designer focused on user experience and design thinking
   - Values: user experience, design thinking, accessibility
6. **👨‍🏫 TechLead** - Technical lead focused on team coordination and technical strategy
   - Values: team coordination, technical strategy, mentoring
7. **📈 DataScientist** - Data scientist focused on analytics, machine learning, and insights
   - Values: data insights, machine learning, analytics
8. **🔒 SecurityEngineer** - Security engineer focused on cybersecurity and compliance
   - Values: security, compliance, risk assessment

## 🎯 Key Features

- **Intelligent Agent Selection**: AI coordinator selects relevant agents based on topic
- **Debate-Focused Interactions**: Agents engage in respectful debates and discussions
- **Role Diversity**: Each agent brings unique professional perspectives
- **Modular Architecture**: Clean separation of concerns with dedicated services
- **Command System**: Built-in help, agent listing, and conversation management
- **Error Handling**: Robust error handling and user feedback

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK
- OpenAI API key or Azure OpenAI credentials

### Setup

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd ideal-tribble/GroupChatConsole
   ```

2. **Set up API credentials**

   For OpenAI:

   ```bash
   set OPENAI_API_KEY=your_openai_api_key_here
   ```

   For Azure OpenAI:

   ```bash
   set AZURE_OPENAI_KEY=your_azure_key_here
   set AZURE_OPENAI_ENDPOINT=your_azure_endpoint_here
   set AZURE_OPENAI_DEPLOYMENT_NAME=your_deployment_name_here
   ```

3. **Build and run**
   ```bash
   dotnet build
   dotnet run
   ```

## 💬 Usage

### Commands

- `help` - Show available commands and sample topics
- `agents` - List all available agents with descriptions
- `clear` - Clear conversation history
- `quit` or `exit` - Exit the application

### Sample Topics

- "Should we prioritize speed of delivery or code quality?"
- "How should we handle technical debt in our current project?"
- "What's the best approach for implementing user authentication?"
- "How can we improve our deployment process?"
- "Should we use microservices or monolithic architecture?"

## 🏛️ Architecture

### Models

- **AgentConfiguration**: Defines agent properties, instructions, and cultural backgrounds
- **AgentConfigurations**: Static configuration for all available agents

### Services

- **AgentFactory**: Creates AI agents based on configuration
- **ChatService**: Manages conversation flow and agent interactions
- **CommandHandler**: Processes user commands and displays help information
- **KernelService**: Initializes and configures the Semantic Kernel

### Main Program

- **Program**: Entry point that orchestrates all services and manages the main chat loop

## 🔧 Customization

### Adding New Agents

1. Add a new `AgentConfiguration` to `AgentConfigurations.AllAgents` in `Models/AgentConfiguration.cs`
2. Define the agent's name, description, emoji, values, and instructions
3. The agent will automatically be available in the application

### Modifying Agent Instructions

Update the `Instructions` property in the agent's configuration in `Models/AgentConfiguration.cs`.

### Adding New Commands

1. Add the command to the switch statement in `Services/CommandHandler.cs`
2. Implement the command handler method
3. Update the help text in `ShowHelp()` method

## 🎨 Improvements Made

Based on the Microsoft Semantic Kernel example, this application includes:

- **Enhanced Agent Instructions**: More debate-focused and engaging
- **Additional Agents**: Immigrant and Doctor agents for more diverse perspectives
- **Modular Architecture**: Clean separation of concerns for maintainability
- **Better Error Handling**: Robust error handling throughout the application
- **Improved User Experience**: Better help system and command handling
- **Cultural Authenticity**: More authentic cultural backgrounds and values

## 📝 License

This project is based on the Microsoft Semantic Kernel example and follows similar licensing terms.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit issues, feature requests, or pull requests.

## 📞 Support

For questions or support, please open an issue in the repository.
