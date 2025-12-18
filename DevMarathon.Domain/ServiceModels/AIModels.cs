using System.Text.Json.Serialization;
using DevMarathon.Domain.Enums;

namespace DevMarathon.Domain.ServiceModels;

public class TextResponse
{
    public string Text { get; set; } = default!;
}

public class EmbeddingResponse
{
    public List<float> Vector { get; set; } = new();
}

public class SimilarityResponse
{
    public double Score { get; set; }
}


public class TextRequest
{
    public string Text { get; set; } = default!;
}


public class TranslateRequest
{
    public string Text { get; set; } = default!;
    public string FromLang { get; set; } = default!;
    public string ToLang { get; set; } = default!;
}

public class EmbeddingRequest
{
    public string Text { get; set; } = default!;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EmbeddingType Type { get; set; }
}


public class SimilarityRequest
{
    public string Text1 { get; set; } = default!;
    public string Text2 { get; set; } = default!;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EmbeddingType Type { get; set; }
}


public class LLMRequest
{
    public string Query { get; set; } = default!;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public LlmModelType Type { get; set; }
}


public class PromptRequest
{
    public string Prompt { get; set; } = default!;
}


public class SoundPromptRequest
{
    public string Prompt { get; set; } = default!;
    public double Speed { get; set; }
}