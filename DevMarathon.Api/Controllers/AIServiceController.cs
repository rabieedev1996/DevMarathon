using DevMarathon.Application.Contract.Services;
using DevMarathon.Domain.Enums;
using DevMarathon.Domain.ServiceModels;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/ai")]
    public class AIController : ControllerBase
    {
        private readonly IAIService _aiService;

        public AIController(IAIService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("speech-to-text")]
        public async Task<ActionResult<TextResponse>> SpeechToText(
            [FromForm] IFormFile file,
            [FromQuery] WhisperModel level
        )
        {
            using var stream = file.OpenReadStream();
            return Ok(await _aiService.SpeechToTextAsync(stream, file.FileName, level));
        }

        [HttpPost("image-to-text")]
        public async Task<ActionResult<TextResponse>> ImageToText(
            [FromForm] IFormFile file,
            [FromQuery] bool translate
        )
        {
            using var stream = file.OpenReadStream();
            return Ok(await _aiService.ImageToTextAsync(stream, file.FileName, translate));
        }

        [HttpPost("embedding")]
        public async Task<ActionResult<EmbeddingResponse>> Embedding(
            [FromBody] EmbeddingRequest request
        )
            => Ok(await _aiService.EmbeddingAsync(request));

        [HttpPost("similarity")]
        public async Task<ActionResult<SimilarityResponse>> Similarity(
            [FromBody] SimilarityRequest request
        )
            => Ok(await _aiService.SimilarityAsync(request));

        [HttpPost("llm")]
        public async Task<ActionResult<TextResponse>> LLM(
            [FromBody] LLMRequest request
        )
            => Ok(await _aiService.LLMRequestAsync(request));

        [HttpPost("generate-image")]
        public async Task<IActionResult> GenerateImage(
            [FromBody] PromptRequest request
        )
        {
            var bytes = await _aiService.GenerateImageAsync(new DevMarathon.Domain.ServiceModels.PromptRequest
            {
                Prompt = request.Prompt
            });
            return File(bytes, "image/png");
        }

        [HttpPost("generate-audio")]
        public async Task<IActionResult> GenerateAudio(
            [FromBody] SoundPromptRequest request
        )
        {
            var bytes = await _aiService.GenerateAudioAsync(new DevMarathon.Domain.ServiceModels.SoundPromptRequest
            {
                Prompt = request.Prompt,
                Speed = request.Speed
            });
            return File(bytes, "audio/wav");
        }

        [HttpPost("text-summarize")]
        public async Task<ActionResult<TextResponse>> Summarize(
            [FromBody] TextRequest request
        )
            => Ok(await _aiService.TextSummarizeAsync(new DevMarathon.Domain.ServiceModels.TextRequest
            {
                Text = request.Text
            }));
        
        [HttpPost("speech-to-audio-summarize")]
        public async Task<IActionResult> SpeechToAudioSummarize([FromForm] IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var bytes = await _aiService.SpeechToAudioSummarizeAsync(stream, file.FileName);
            return File(bytes, "audio/wav");
        }

        [HttpPost("speech-to-text-summarize")]
        public async Task<ActionResult<TextResponse>> SpeechToTextSummarize([FromForm] IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var result = await _aiService.SpeechToTextSummarizeAsync(stream, file.FileName);
            return Ok(result);
        }
    }

    // =========================
    // Controller DTOs (Inline)
    // =========================
    public class TextRequest
    {
        public string Text { get; set; } = default!;
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
}
