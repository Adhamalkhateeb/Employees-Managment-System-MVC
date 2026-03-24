using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EmployeesManager.Domain.Common.Results;

public interface IResult
{
    bool IsSuccess { get; }
    List<Error> Errors { get; }
}

public interface IResult<TValue> : IResult
{
    TValue Value { get; }
}
