namespace AiProjectAssistant.Api.DTOs.AI;

public class AskRequestDto
{
    public int ProjectId { get; set; }

    public string Question { get; set; } = string.Empty;
}