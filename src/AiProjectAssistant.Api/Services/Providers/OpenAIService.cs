using AiProjectAssistant.Api.Options;
using AiProjectAssistant.Api.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace AiProjectAssistant.Api.Services.Providers;

public class OpenAIService : IAiProvider
{
    private readonly OpenAIOptions _options;

    public OpenAIService(IOptions<OpenAIOptions> options)
    {
        _options = options.Value;
    }

    public async Task<string> AskAsync(
        string systemPrompt,
        string question)
    {
        await Task.CompletedTask;

        throw new NotImplementedException();
    }
}