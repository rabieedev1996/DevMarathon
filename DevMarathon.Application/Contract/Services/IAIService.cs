using DevMarathon.Domain.Enums;
using DevMarathon.Domain.ServiceModels;

namespace DevMarathon.Application.Contract.Services;

public interface IAIService
{
    Task<TextResponse> SpeechToTextAsync(
        Stream file,
        string fileName,
        WhisperModel level,
        CancellationToken cancellationToken = default
    );

    Task<TextResponse> ImageToTextAsync(
        Stream file,
        string fileName,
        bool translate,
        CancellationToken cancellationToken = default
    );

    Task<TextResponse> TranslateAsync(
        TranslateRequest request,
        CancellationToken cancellationToken = default
    );

    Task<EmbeddingResponse> EmbeddingAsync(
        EmbeddingRequest request,
        CancellationToken cancellationToken = default
    );

    Task<SimilarityResponse> SimilarityAsync(
        SimilarityRequest request,
        CancellationToken cancellationToken = default
    );

    Task<TextResponse> LLMRequestAsync(
        LLMRequest request,
        CancellationToken cancellationToken = default
    );

    Task<byte[]> GenerateImageAsync(
        PromptRequest request,
        CancellationToken cancellationToken = default
    );

    Task<byte[]> GenerateAudioAsync(
        SoundPromptRequest request,
        CancellationToken cancellationToken = default
    );

    Task<TextResponse> TextSummarizeAsync(
        TextRequest request,
        CancellationToken cancellationToken = default
    );
    Task<byte[]> SpeechToAudioSummarizeAsync(
        Stream file,
        string fileName,
        CancellationToken cancellationToken = default
    );

    Task<TextResponse> SpeechToTextSummarizeAsync(
        Stream file,
        string fileName,
        CancellationToken cancellationToken = default
    );
}