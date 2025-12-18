using System.Text.Json;
using DevMarathon.Application.Contract.Services;
using DevMarathon.Domain;
using DevMarathon.Domain.Enums;
using DevMarathon.Domain.ServiceModels;
using RestSharp;

namespace DevMarathon.Infrastructure.Service;

 public class AIService : IAIService
    {
        private readonly RestClient _client;

        public AIService(Configs configs)
        {
            _client = new RestClient(configs.AIServiceConfigs.Endpoint);
        }

        public async Task<TextResponse> SpeechToTextAsync(
            Stream file,
            string fileName,
            WhisperModel level,
            CancellationToken cancellationToken = default
        )
        {
            var request = new RestRequest("/speech-to-text", Method.Post);
            request.AddQueryParameter("level", level.ToString());
            request.AddFile("file", ReadAll(file), fileName);

            var response = await _client.ExecuteAsync(request, cancellationToken);
            return Deserialize<TextResponse>(response);
        }

        public async Task<TextResponse> ImageToTextAsync(
            Stream file,
            string fileName,
            bool translate,
            CancellationToken cancellationToken = default
        )
        {
            var request = new RestRequest("/image-to-text", Method.Post);
            request.AddQueryParameter("translate", translate);
            request.AddFile("file", ReadAll(file), fileName);

            var response = await _client.ExecuteAsync(request, cancellationToken);
            return Deserialize<TextResponse>(response);
        }

        public async Task<TextResponse> TranslateAsync(
            TranslateRequest requestModel,
            CancellationToken cancellationToken = default
        )
        {
            var request = new RestRequest("/translate", Method.Post)
                .AddJsonBody(requestModel);

            var response = await _client.ExecuteAsync(request, cancellationToken);
            return Deserialize<TextResponse>(response);
        }

        public async Task<EmbeddingResponse> EmbeddingAsync(
            EmbeddingRequest requestModel,
            CancellationToken cancellationToken = default
        )
        {
            var request = new RestRequest("/embedding", Method.Post)
                .AddJsonBody(requestModel);

            var response = await _client.ExecuteAsync(request, cancellationToken);
            return Deserialize<EmbeddingResponse>(response);
        }

        public async Task<SimilarityResponse> SimilarityAsync(
            SimilarityRequest requestModel,
            CancellationToken cancellationToken = default
        )
        {
            var request = new RestRequest("/similarity", Method.Post)
                .AddJsonBody(requestModel);

            var response = await _client.ExecuteAsync(request, cancellationToken);
            return Deserialize<SimilarityResponse>(response);
        }

        public async Task<TextResponse> LLMRequestAsync(
            LLMRequest requestModel,
            CancellationToken cancellationToken = default
        )
        {
            var request = new RestRequest("/llm-request", Method.Post)
                .AddJsonBody(requestModel);

            var response = await _client.ExecuteAsync(request, cancellationToken);
            return Deserialize<TextResponse>(response);
        }

        public async Task<byte[]> GenerateImageAsync(
            PromptRequest requestModel,
            CancellationToken cancellationToken = default
        )
        {
            var request = new RestRequest("/generate-image", Method.Post)
                .AddJsonBody(requestModel);

            var response = await _client.ExecuteAsync(request, cancellationToken);
            return response.RawBytes!;
        }

        public async Task<byte[]> GenerateAudioAsync(
            SoundPromptRequest requestModel,
            CancellationToken cancellationToken = default
        )
        {
            var request = new RestRequest("/generate-audio", Method.Post)
                .AddJsonBody(requestModel);

            var response = await _client.ExecuteAsync(request, cancellationToken);
            return response.RawBytes!;
        }

        public async Task<TextResponse> TextSummarizeAsync(
            TextRequest requestModel,
            CancellationToken cancellationToken = default
        )
        {
            var request = new RestRequest("/text-summarize", Method.Post)
                .AddJsonBody(requestModel);

            var response = await _client.ExecuteAsync(request, cancellationToken);
            return Deserialize<TextResponse>(response);
        }
        
        public async Task<byte[]> SpeechToAudioSummarizeAsync(
            Stream file,
            string fileName,
            CancellationToken cancellationToken = default
        )
        {
            var request = new RestRequest("/speech-to-audio-summarize", Method.Post);
            request.AddFile("file", ReadAll(file), fileName);

            var response = await _client.ExecuteAsync(request, cancellationToken);
            return response.RawBytes!;
        }

        public async Task<TextResponse> SpeechToTextSummarizeAsync(
            Stream file,
            string fileName,
            CancellationToken cancellationToken = default
        )
        {
            var request = new RestRequest("/speech-to-text-summarize", Method.Post);
            request.AddFile("file", ReadAll(file), fileName);

            var response = await _client.ExecuteAsync(request, cancellationToken);
            return Deserialize<TextResponse>(response);
        }

        // =========================
        // Helpers
        // =========================
        private static byte[] ReadAll(Stream stream)
        {
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }

        private static T Deserialize<T>(RestResponse response)
        {
            return JsonSerializer.Deserialize<T>(
                response.Content!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            )!;
        }
    }