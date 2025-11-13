using Microsoft.AspNetCore.Mvc;

namespace MyWebAPI.Models;

public class CommonResult<T>
{
  public bool IsSuccess { get; init; }
  public T? Result { get; init; }
  
  public string? Message { get; init; }
  

  public CommonResult (T? result, bool isSuccess, string? message)
  {
    Result = result;
    IsSuccess = isSuccess; 
    Message = message;
  }

  public static CommonResult<T> Success(T result)
  {
    return new CommonResult<T>(result, true, null);
  }

  public static CommonResult<T> Fail(string errorMessage)
  {
    return new CommonResult<T>(default, false, errorMessage);
  }
}