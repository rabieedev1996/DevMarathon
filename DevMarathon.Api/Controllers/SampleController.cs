using DevMarathon.Application.Common;
using DevMarathon.Application.Features.Sample.Commands.GenerateSampleToken;
using DevMarathon.Application.Features.Sample.Commands.ValidateToken;
using DevMarathon.Application.Features.Sample.SampleService;
using DevMarathon.Application.Models;
using DevMarathon.Domain;
using DevMarathon.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YasgapNew.Api.Filters;

namespace DevMarathon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        IMediator _mediator;
        ResponseGenerator _rsponseGenerator;
        public SampleController(IMediator mediator, ResponseGenerator rsponseGenerator)
        {
            _mediator = mediator;
            _rsponseGenerator = rsponseGenerator;
        }
        [HttpPost("/SampleService")]
        [ProducesResponseType(typeof(ApiResponseModel<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SampleService(SampleServiceInput input)
        {
            var resultData = await _mediator.Send(input);
            var resultObj = _rsponseGenerator.GetHTTPResponseModel(ResponseCodes.SUCCESS, resultData);
            return resultObj;
        }
        [HttpPost("/GenerateToken")]
        [ProducesResponseType(typeof(ApiResponseModel<string>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateToken()
        {
            var resultData = await _mediator.Send(new GenerateSampleTokenInput { });
            var resultObj = _rsponseGenerator.GetHTTPResponseModel(ResponseCodes.SUCCESS, resultData);
            return resultObj;
        }
        [HttpPost("/ValidateToken")]
        [ProducesResponseType(typeof(ApiResponseModel<ValidateTokenVm>), StatusCodes.Status200OK)]
        [CustomAuthorize(IdentityRoles.USER,IdentityReason.ON_REGISTER)]
        public async Task<IActionResult> ValidateToken()
        {
            var resultData = await _mediator.Send(new ValidateTokenInput { });
            var resultObj = _rsponseGenerator.GetHTTPResponseModel(ResponseCodes.SUCCESS, resultData);
            return resultObj;
        }


    }
}
