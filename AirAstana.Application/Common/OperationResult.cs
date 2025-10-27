namespace AirAstana.Application.Common;

public record OperationResult<T>(
    bool Success,
    string Message,
    T? Data = default,
    HttpCode Code = HttpCode.Success
);