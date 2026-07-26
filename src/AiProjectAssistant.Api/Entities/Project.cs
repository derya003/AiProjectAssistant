namespace AiProjectAssistant.Api.Entities;

public class Project
{
    public int Id { get; set; }

    public string ProjectName { get; set; } = string.Empty;

    public string Prompt { get; set; } = string.Empty;
}