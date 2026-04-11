using EmployeesManager.Domain.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesManager.Web.Controllers;

public class MvcController : Controller
{
    protected IActionResult HandleError<TModel>(List<Error> errors, TModel model) =>
        HandleErrorInternal(errors, () => View(model));

    protected IActionResult HandleError(List<Error> errors) =>
        HandleErrorInternal(errors, () => View());

    private IActionResult HandleErrorInternal(List<Error> errors, Func<IActionResult> onValidation)
    {
        if (errors is null || errors.Count == 0)
            return RedirectToAction(
                "Error",
                "Home",
                new { statusCode = StatusCodes.Status500InternalServerError }
            );

        if (errors.All(e => e.Type == ErrorKind.Validation || e.Type == ErrorKind.Conflict))
        {
            foreach (var error in errors)
            {
                var key = string.IsNullOrWhiteSpace(error.PropertyName)
                    ? error.Code
                    : error.PropertyName;

                ModelState.AddModelError(key, error.Description);
            }

            return onValidation();
        }

        var primary = errors[0];
        var statusCode = MapToStatusCode(primary.Type);

        TempData["ResultErrorCode"] = primary.Code;
        TempData["ResultErrorMessage"] = primary.Description;
        TempData["ResultErrorDetails"] = string.Join(
            " | ",
            errors.Select(e => $"{e.Code}: {e.Description}").Distinct()
        );

        return RedirectToAction("Error", "Home", new { statusCode });
    }

    private static int MapToStatusCode(ErrorKind type) =>
        type switch
        {
            ErrorKind.Validation => StatusCodes.Status400BadRequest,
            ErrorKind.Conflict => StatusCodes.Status409Conflict,
            ErrorKind.NotFound => StatusCodes.Status404NotFound,
            ErrorKind.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorKind.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError,
        };
}
