using System.Text.Json;

namespace JobMatch.Api.Agents;

// מוציא את ה-JSON מתשובת המודל, גם אם יש לפניו או אחריו טקסט או ```json
public static class AgentJson
{
    // Web = שמות ב-camelCase ובלי רגישות לאותיות גדולות, כמו ב-ASP.NET Core
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public static T Parse<T>(string text)
    {
        int start = text.IndexOf('{');
        int end = text.LastIndexOf('}');
        if (start < 0 || end <= start)
            throw new InvalidOperationException(
                $"The model did not return JSON. It answered: {text[..Math.Min(text.Length, 300)]}");

        return JsonSerializer.Deserialize<T>(text[start..(end + 1)], Options)
            ?? throw new InvalidOperationException("The model returned empty JSON.");
    }
}