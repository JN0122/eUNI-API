namespace eUNI_API.Exception;

public class HttpCustomException(int statusCode, string message) : System.Exception(message)
{
    public int StatusCode { get; } = statusCode;
    public override string Message { get; } = message;
}

public class HttpBadRequestException(string message = "") : HttpCustomException(400, message);

public class HttpUnauthorizedException(string message = "") : HttpCustomException(401, message);

public class HttpForbiddenException(string message = "") : HttpCustomException(403, message);

public class HttpNotFoundException(string message = "") : HttpCustomException(404, message);