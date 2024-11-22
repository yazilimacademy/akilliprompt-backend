using AkilliPrompt.Domain.ValueObjects;

namespace AkilliPrompt.WebApi.Models;

public sealed class ResponseDto<T>
{
    public T? Data { get; set; }
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }
    public IReadOnlyList<ValidationError> ValidationErrors { get; set; }

    public ResponseDto(T? data, string message, bool isSuccess, IReadOnlyList<ValidationError> validationErrors)
    {
        Data = data;
        Message = message;
        IsSuccess = isSuccess;
        ValidationErrors = validationErrors;
    }

    public static ResponseDto<T> Success(T data, string message)
    {
        return new ResponseDto<T>(data, message, true, []);
    }
    public static ResponseDto<T?> Success(string message)
    {
        return new ResponseDto<T?>(default, message, true, []);
    }


    public static ResponseDto<T> Error(string message, List<ValidationError> validationErrors)
    {
        return new ResponseDto<T>(default, message, false, validationErrors);
    }

    public static ResponseDto<T> Error(string message)
    {
        return new ResponseDto<T>(default, message, false, []);
    }

    public static ResponseDto<T> Error(string message, ValidationError validationError)
    {
        return new ResponseDto<T>(default, message, false, [validationError]);
    }


}
