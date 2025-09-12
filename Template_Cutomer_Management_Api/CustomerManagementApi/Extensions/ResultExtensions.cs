using CSharpFunctionalExtensions;
using FluentValidation.Results;
using Shared.Exceptions;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace CustomerManagementApi.Extensions;

public static class ResultExtensions
{
    public static async Task<IResult> ToResponse(this Task<UnitResult<Exception>> task)
    {
        var result = await task;

        return result.ToResponse();
    }

    private static IResult ToResponse(this UnitResult<Exception> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok();
        }

        if (result.Error is NoContentException)
        {
            return Results.NoContent();
        }
        
        if (result.Error is NotFoundException)
        {
            return Results.NotFound();
        }

        return Results.InternalServerError();
    }
    
    public static IResult ToValidationResponse(this ValidationResult result)
    {
        if (result.IsValid)
        {
            return Results.Ok();
        }
        
        var errors = result.Errors.Select(error => error.ErrorMessage);

        return Results.Conflict(errors.ToList());
    }
}