using DevMarathon.Application.Common;
using DevMarathon.Application.Features.Account.Commands;
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
    public class  AccountController: ControllerBase
    {
        IMediator _mediator;
        ResponseGenerator _rsponseGenerator;
        public AccountController(IMediator mediator, ResponseGenerator rsponseGenerator)
        {
            _mediator = mediator;
            _rsponseGenerator = rsponseGenerator;
        }
        [HttpPost("/Register")]
        [ProducesResponseType(typeof(ApiResponseModel<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var resultData = await _mediator.Send(command);
            var resultObj = _rsponseGenerator.GetHTTPResponseModel(ResponseCodes.SUCCESS, resultData);
            return resultObj;
        }
       
        [HttpPost("/Verify")]
        [ProducesResponseType(typeof(ApiResponseModel<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Verify(VerifyCommand command)
        {
            var resultData = await _mediator.Send(command);
            var resultObj = _rsponseGenerator.GetHTTPResponseModel(ResponseCodes.SUCCESS, resultData);
            return resultObj;
        }

    }
}
