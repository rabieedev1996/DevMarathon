using DevMarathon.Application.Models;
using DevMarathon.Application.Common;
using DevMarathon.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DevMarathon.Application.ExceptionHandler;

public class ApiResponseException : Exception
{
    private ObjectResult _responseObject;
    private ResponseGenerator _responseGenerator;

    public ApiResponseException(ResponseGenerator responseGenerator)
    {
        _responseGenerator = responseGenerator;
    }

    public void SetDetail(ResponseCodes code)
    {
        _responseObject = _responseGenerator.GetHTTPResponseModel<object>(code, new { });
    }
    public void SetDetail<TModel>(ResponseCodes code, TModel Data)
    {
        _responseObject = _responseGenerator.GetHTTPResponseModel<object>(code, Data);
    }

    public ObjectResult ResponseObject
    {
        get { return _responseObject; }
    }
}