namespace AirAstana.Application.Common;

public enum HttpCode
{
    Success = 0,
    NotFound = 404,
    ValidationError = 1001,
    Unauthorized = 1002,
    UnknownError = 9999
}