using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevMarathon.Domain;
using DevMarathon.Identity;
using FluentValidation;
using MediatR;

namespace DevMarathon.Application.Features.Sample.Commands.GenerateSampleToken
{
    #region Handler
    public class GenerateSampleTokenHandler : IRequestHandler<GenerateSampleTokenInput, GenerateSampleTokenVm>
    {
        public Configs Configs;

        public GenerateSampleTokenHandler(Configs configs)
        {
            Configs = configs;
        }

        public async Task<GenerateSampleTokenVm> Handle(GenerateSampleTokenInput request, CancellationToken cancellationToken)
        {
            var token = IdentityUtility.GenerateToken(new TokenParams
            {
                UserId=Guid.NewGuid().ToString(),   
                TokenId=Guid.NewGuid().ToString(),  
                ExpireTime=TimeSpan.FromMinutes(30),
                Roles=new List<string> { "USER"},
                OtherClaims=new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("FULL_NAME","Mohammad Rabiee"),
                },
                Reason="ON_REGISTER"             
            },Configs.TokenConfigs);
            return new GenerateSampleTokenVm
            { 
             Token=token
            };
        }
    }
    #endregion

    #region Query
    public class GenerateSampleTokenInput : IRequest<GenerateSampleTokenVm>
    { }
    #endregion

    #region ViewModel
    public class GenerateSampleTokenVm
    {
        public string Token { get; set; }
    }
    #endregion
}