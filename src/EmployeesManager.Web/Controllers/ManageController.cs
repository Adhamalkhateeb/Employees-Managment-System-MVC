using EmployeesManager.Application.Features.Identity.Email.Commands.SendChangeEmailLink;
using EmployeesManager.Application.Features.Identity.Email.Commands.SendVerificationEmail;
using EmployeesManager.Contracts.Requests.Identity;
using EmployeesManager.Infrastructure.Identity;
using EmployeesManager.Web.Models.Account;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesManager.Web.Controllers;

[Authorize]
[Route("[controller]/[action]")]
public sealed class ManageController(IMediator mediator, UserManager<AppUser> userManager) : MvcController
{
    private readonly IMediator _mediator = mediator;
    private readonly UserManager<AppUser> _userManager = userManager;

    [HttpGet]
    public IActionResult Index() => RedirectToAction(nameof(Email));

    [HttpGet]
    public async Task<IActionResult> Email(string? returnUrl = null)
    {
        var model = await BuildEmailViewModelAsync();
        if (model is null)
            return RedirectToAction("Login", "Account", new { returnUrl });

        ViewData["ReturnUrl"] = returnUrl;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendVerificationEmail(
        string? returnUrl = null,
        CancellationToken cancellationToken = default
    )
    {
        var confirmEmailBaseUrl = Url.Action(
            "ConfirmEmail",
            "Account",
            values: null,
            protocol: Request.Scheme
        );

        if (string.IsNullOrWhiteSpace(confirmEmailBaseUrl))
        {
            TempData["StatusMessage"] = "Could not initialize confirmation callback URL.";
            return RedirectToAction(nameof(Email), new { returnUrl });
        }

        var sendResult = await _mediator.Send(
            new SendVerificationEmailCommand(confirmEmailBaseUrl, returnUrl),
            cancellationToken
        );

        if (sendResult.IsError)
        {
            TempData["StatusMessage"] = sendResult.TopError.Description;
            return RedirectToAction(nameof(Email), new { returnUrl });
        }

        TempData["StatusMessage"] = "Verification email sent. Please check your inbox.";

        return RedirectToAction(nameof(Email), new { returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeEmail(
        ChangeEmailRequest input,
        string? returnUrl = null,
        CancellationToken cancellationToken = default
    )
    {
        ViewData["ReturnUrl"] = returnUrl;

        var model = await BuildEmailViewModelAsync(input);
        if (model is null)
            return RedirectToAction("Login", "Account", new { returnUrl });

        if (!ModelState.IsValid)
            return View(nameof(Email), model);

        var confirmEmailChangeBaseUrl = Url.Action(
            "ConfirmEmailChange",
            "Account",
            values: null,
            protocol: Request.Scheme
        );

        if (string.IsNullOrWhiteSpace(confirmEmailChangeBaseUrl))
        {
            ModelState.AddModelError(string.Empty, "Could not initialize email-change callback URL.");
            return View(nameof(Email), model);
        }

        var changeResult = await _mediator.Send(
            new SendChangeEmailLinkCommand(input.NewEmail, confirmEmailChangeBaseUrl, returnUrl),
            cancellationToken
        );

        if (changeResult.IsError)
            return HandleError(changeResult.Errors, model);

        TempData["StatusMessage"] =
            "A confirmation link to change your email has been sent. Please check your inbox.";

        return RedirectToAction(nameof(Email), new { returnUrl });
    }

    private async Task<ManageEmailViewModel?> BuildEmailViewModelAsync(ChangeEmailRequest? input = null)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
            return null;

        return new ManageEmailViewModel
        {
            Email = user.Email ?? string.Empty,
            IsEmailConfirmed = user.EmailConfirmed,
            Input = input ?? new ChangeEmailRequest(),
        };
    }
}
