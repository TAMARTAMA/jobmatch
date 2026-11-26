using System.ClientModel;
using Microsoft.Extensions.AI;
using OpenAI;

namespace JobMatch.Api.AI;

public static class ChatClientFactory
{
    public static IChatClient Create(IConfiguration config)
    {
        var provider = config["AI:Provider"] ?? "GitHubModels";
        var model = config["AI:Model"]
            ?? throw new InvalidOperationException("Missing AI:Model in appsettings.json");
        var apiKey = config["AI:ApiKey"]
            ?? throw new InvalidOperationException("Missing AI:ApiKey. Set it with dotnet user-secrets.");

        return provider switch
        {
            "GitHubModels" => new OpenAIClient(
                    new ApiKeyCredential(apiKey),
                    new OpenAIClientOptions
                    {
                        Endpoint = new Uri(config["AI:Endpoint"] ?? "https://models.github.ai/inference")
                    })
                .GetChatClient(model)
                .AsIChatClient(),

            "Anthropic" => new Anthropic.AnthropicClient(
                    new Anthropic.Core.ClientOptions { ApiKey = apiKey })
                .AsIChatClient(model, 2048),

            _ => throw new InvalidOperationException($"Unknown AI provider: {provider}")
        };
    }
}