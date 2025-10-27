using AirAstana.Application.Common;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace AirAstana.Presentation.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        string message;
        HttpCode httpCode;

        if (context.Exception is ValidationException validationEx)
        {
            message = validationEx.Errors.FirstOrDefault()?.ErrorMessage ?? Messages.ValidationError;
            httpCode = HttpCode.ValidationError;
        }
        else
        {
            Log.Error(context.Exception, Messages.UnknownError);
            message = Messages.UnknownError;
            httpCode = HttpCode.UnknownError;
        }

        context.Result = new JsonResult(new
        {
            success = false,
            message,
            data = (object?)null,
            httpCode
        })
        {
            StatusCode = 200
        };

        context.ExceptionHandled = true;
    }
}