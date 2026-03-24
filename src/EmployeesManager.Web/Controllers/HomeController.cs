using System.Diagnostics;
using EmployeesManager.Web.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesManager.Web.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index() => View();

    [Route("Error")]
    public IActionResult Error(int? statusCode = null)
    {
        var effective = statusCode ?? StatusCodes.Status500InternalServerError;
        Response.StatusCode = effective;

        var model = new ErrorViewModel
        {
            StatusCode = effective,
            TraceId = HttpContext.TraceIdentifier,
            Details = TempData["ResultErrorDetails"]?.ToString(),
            ErrorCode = TempData["ResultErrorCode"]?.ToString(),
            Message =
                TempData["ResultErrorMessage"]?.ToString()
                ?? effective switch
                {
                    400 => "Bad Request",
                    401 => "Unauthorized",
                    403 => "Forbidden",
                    404 => "Page Not Found",
                    409 => "Conflict",
                    _ => "An unexpected error occurred.",
                },
        };

        var ex = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (ex?.Error is not null)
            model = model with
            {
                Details = string.IsNullOrWhiteSpace(model.Details)
                    ? ex.Error.Message
                    : $"{model.Details} | {ex.Error.Message}",
            };

        return View("~/Views/Shared/Error.cshtml", model);
    }
}
