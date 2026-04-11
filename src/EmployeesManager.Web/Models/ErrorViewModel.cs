namespace EmployeesManager.Web.Models;

public record ErrorViewModel
{
    public int StatusCode { get; init; }
    public string? TraceId { get; init; }
    public string? ErrorCode { get; init; }
    public string? Message { get; init; }
    public string? Details { get; init; }
}
