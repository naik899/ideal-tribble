# ideal-tribble

**Interactive Multi-Cultural Philosophy Discussion** with AI Agents using Microsoft Semantic Kernel.

This project creates an interactive chat experience where you can have philosophical discussions with AI agents representing different cultural perspectives from around the world. Each agent brings unique viewpoints shaped by their cultural background, profession, and life experiences.

## ✨ Features

### 🌍 **Diverse Cultural Perspectives**
- **🌾 Farmer** from Southeast Asia - Values tradition, sustainability, and family connection
- **💻 Developer** from United States - Focuses on innovation, technology, and work-life balance  
- **📚 Teacher** from Eastern Europe - Brings historical wisdom and cultural continuity
- **✊ Activist** from South America - Champions social justice and environmental rights
- **🕊️ Spiritual Leader** from Middle East - Provides moral and community-focused insights
- **🎨 Artist** from Africa - Offers creative expression and storytelling perspectives

### 🤖 **Intelligent Conversation Management**
- **Smart Agent Selection**: AI coordinator automatically selects 2-3 most relevant agents for each question
- **Multi-Turn Conversations**: Continuous, context-aware discussions that build over time
- **Dynamic Interaction**: Agents respond based on conversation history and cultural authenticity

### 💬 **Interactive Commands**
- `help` - Show available commands and sample topics
- `agents` - View all agents and their cultural backgrounds  
- `clear` - Reset conversation history
- `quit` - Exit the application

## 🚀 Quick Start

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download) or later
- OpenAI API key OR Azure OpenAI service

### Setup

1. **Set Environment Variables**:

   **For OpenAI:**
   ```bash
   # Windows Command Prompt
   set OPENAI_API_KEY=your_openai_api_key_here
   set OPENAI_MODEL=gpt-4
   
   # Windows PowerShell  
   $env:OPENAI_API_KEY="your_openai_api_key_here"
   $env:OPENAI_MODEL="gpt-4"
   
   # Linux/Mac
   export OPENAI_API_KEY="your_openai_api_key_here"
   export OPENAI_MODEL="gpt-4"
   ```

   **For Azure OpenAI:**
   ```bash
   set AZURE_OPENAI_KEY=your_azure_key_here
   set AZURE_OPENAI_ENDPOINT=https://your-resource.openai.azure.com/
   set AZURE_OPENAI_DEPLOYMENT_NAME=gpt-4o
   set AZURE_OPENAI_MODEL=gpt-4o
   ```

2. **Run the Application**:
   ```bash
   dotnet run --project GroupChatConsole/GroupChatConsole.csproj
   ```

## 🎮 How to Use

1. **Start the Application** - You'll see a welcome message and prompt
2. **Ask Questions** - Type any philosophical or cultural question
3. **Get Diverse Responses** - The coordinator selects relevant agents to respond
4. **Continue the Conversation** - Build on previous responses with follow-up questions
5. **Use Commands** - Type `help` to see available commands

### 💡 Sample Conversation Starters

```
What does a good life mean to you?
How do you balance tradition with progress?
What role should technology play in society?
How do you define success?
What values should we pass to the next generation?
How do you view work-life balance?
What is the role of community in individual happiness?
How do you handle conflict and disagreement?
What does freedom mean in your culture?
How do you think about environmental responsibility?
```

## 🏗️ Architecture

### **Clean, Readable Code Structure**
- **Modular Design**: Organized into logical regions (Kernel, Agents, Chat, Help)
- **Comprehensive Documentation**: Every method documented with XML comments
- **Error Handling**: Robust error handling for console input and API calls
- **Async/Await Pattern**: Proper asynchronous programming throughout

### **AI Integration**
- **Microsoft Semantic Kernel**: Advanced AI orchestration framework
- **Dual API Support**: Works with both OpenAI and Azure OpenAI
- **ChatCompletionAgent**: Core agent implementation for natural conversations
- **Dynamic Coordination**: AI-driven selection of relevant perspectives

## 🎯 Educational Value

This project demonstrates:
- **Multi-agent AI systems** with cultural diversity
- **Interactive conversation design** and user experience
- **Cross-cultural perspective modeling** in AI
- **Clean software architecture** and documentation
- **Asynchronous programming** patterns in C#
- **Microsoft Semantic Kernel** capabilities

## 🔧 Technical Details

- **Framework**: .NET 8.0
- **AI Services**: OpenAI GPT-4 / Azure OpenAI
- **Key Dependencies**:
  - Microsoft.SemanticKernel (1.64.0)
  - Microsoft.SemanticKernel.Connectors.OpenAI (1.64.0) 
  - Microsoft.SemanticKernel.Agents.Core (1.64.0)

## 📱 Sample Output

```
=== Multi-Cultural Interactive Discussion ===
Chat with AI agents representing different cultural perspectives!
Type 'help' for commands or start chatting.

Using OpenAI...
Ready! Start by asking a question or type 'help' for commands.

You: What does success mean to you?

[Developer]:
Success for me is about creating innovative solutions that make people's lives easier while maintaining a healthy work-life balance. It's not just about climbing the corporate ladder, but about contributing meaningful code to projects that matter and having time for personal growth and relationships.

[SpiritualLeader]:
True success lies in serving others and growing closer to the divine. It's measured not by material possessions, but by the positive impact we have on our community, the wisdom we gain through spiritual practice, and how well we fulfill our moral obligations to help those in need.

[Farmer]:
Success means having healthy crops that feed my family and community, maintaining the land for future generations, and preserving the farming traditions passed down from my ancestors. It's about sustainability and being connected to the natural cycles of life.

You: How do these different views complement each other?
```

## 🤝 Contributing

This project welcomes contributions! Whether you want to:
- Add new cultural perspectives
- Improve the conversation flow
- Enhance the user interface
- Add new features

Feel free to submit issues and pull requests.
