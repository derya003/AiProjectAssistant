namespace AiProjectAssistant.Api.Services.Interfaces;

public interface IAiProvider
{
    Task<string> AskAsync(
        string systemPrompt,
        string question);
}