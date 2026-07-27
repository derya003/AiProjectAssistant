using System.Net.Http.Json;
using System.Text.Json;
using AiProjectAssistant.Api.Options;
using AiProjectAssistant.Api.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace AiProjectAssistant.Api.Services.Providers;

public class ClaudeService : IAiProvider
{
    private readonly HttpClient _httpClient;
    private readonly ClaudeOptions _options;

    public ClaudeService(
        HttpClient httpClient,
        IOptions<ClaudeOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<string> AskAsync(
        string systemPrompt,
        string question)
    {
        var requestBody = new
        {
            model = _options.Model,
            max_tokens = _options.MaxTokens,
            system = systemPrompt,
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = question
                }
            }
        };

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"{_options.BaseUrl.TrimEnd('/')}/v1/messages");

        request.Headers.Add(
            "x-api-key",
            _options.ApiKey);

        request.Headers.Add(
            "anthropic-version",
            "2023-06-01");

        request.Content = JsonContent.Create(requestBody);

        using var response =
            await _httpClient.SendAsync(request);

        var responseContent =
            await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Claude API isteği başarısız oldu. " +
                $"Durum kodu: {(int)response.StatusCode}. " +
                $"Cevap: {responseContent}");
        }

        using var jsonDocument =
            JsonDocument.Parse(responseContent);

        var contentArray = jsonDocument
            .RootElement
            .GetProperty("content");

        if (contentArray.GetArrayLength() == 0)
        {
            throw new InvalidOperationException(
                "Claude API boş cevap döndürdü.");
        }

        var answer = contentArray[0]
            .GetProperty("text")
            .GetString();

        if (string.IsNullOrWhiteSpace(answer))
        {
            throw new InvalidOperationException(
                "Claude API cevabındaki metin bulunamadı.");
        }

        return answer;
    }
}