namespace Finances.Domain.Settings;

public sealed class CbrSettings()
{
    public string Url { get; set; } = null!;
    public int TimeoutInSeconds { get; set; } = 60;
}