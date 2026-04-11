using EmployeesManager.Application.Features.Identity.Authentication.Commands.Login;
using EmployeesManager.Application.Features.Identity.Authentication.Commands.LoginWith2fa;
using EmployeesManager.Application.Features.Identity.Authentication.Commands.LoginWithRecoveryCode;
using EmployeesManager.Application.Features.Identity.Authentication.Commands.Logout;
using EmployeesManager.Application.Features.Identity.Authentication.Commands.Register;
using EmployeesManager.Application.Features.Identity.Email.Commands.ConfirmEmail;
using EmployeesManager.Application.Features.Identity.Email.Commands.ConfirmEmailChange;
using EmployeesManager.Application.Features.Identity.Email.Commands.SendEmailConfirmation;
using EmployeesManager.Contracts.Requests.Identity;
using EmployeesManager.Web.Models.Account;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesManager.Web.Controllers;

[Route("[controller]/[action]")]
public sealed class AccountController(IMediator mediator) : MvcController
{
    private readonly IMediator _mediator = mediator;

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Register(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(
        RegisterRequest request,
        string? returnUrl = null,
        CancellationToken cancellationToken = default
    )
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(request);

        var confirmationBaseUrl = Url.Action(
            nameof(ConfirmEmail),
            "Account",
            values: null,
            protocol: Request.Scheme
        );

        if (string.IsNullOrWhiteSpace(confirmationBaseUrl))
        {
            ModelState.AddModelError(
                string.Empty,
                "Could not initialize registration callback URL."
            );
            return View(request);
        }

        var registerResult = await _mediator.Send(
            new RegisterCommand(
                request.UserName,
                request.Email,
                request.PhoneNumber,
                request.Password,
                confirmationBaseUrl,
                returnUrl
            ),
            cancellationToken
        );

        if (registerResult.IsError)
            return HandleError(registerResult.Errors, request);

        return RedirectToAction(
            nameof(RegisterConfirmation),
            new { email = request.Email, returnUrl }
        );
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult ResendEmailConfirmation(string? email = null, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new ResendEmailConfirmationRequest { Email = email ?? string.Empty });
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResendEmailConfirmation(
        ResendEmailConfirmationRequest request,
        string? returnUrl = null,
        CancellationToken cancellationToken = default
    )
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(request);

        var confirmationBaseUrl = Url.Action(
            nameof(ConfirmEmail),
            "Account",
            values: null,
            protocol: Request.Scheme
        );

        if (string.IsNullOrWhiteSpace(confirmationBaseUrl))
        {
            ModelState.AddModelError(
                string.Empty,
                "Could not initialize confirmation callback URL."
            );
            return View(request);
        }

        var resendResult = await _mediator.Send(
            new SendEmailConfirmationCommand(request.Email, confirmationBaseUrl, returnUrl),
            cancellationToken
        );

        if (resendResult.IsError)
            return HandleError(resendResult.Errors, request);

        TempData["StatusMessage"] =
            "If your account exists, a new confirmation email has been sent.";

        return RedirectToAction(
            nameof(RegisterConfirmation),
            new { email = request.Email, returnUrl }
        );
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult RegisterConfirmation(string email, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(model: email);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(
        Guid userId,
        string code,
        string? returnUrl = null,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(new ConfirmEmailCommand(userId, code), cancellationToken);

        var model = new ConfirmEmailViewModel
        {
            Succeeded = result.IsSuccess,
            Message = result.IsSuccess
                ? "Your email has been confirmed successfully. You can now sign in."
                : result.TopError.Description,
            ReturnUrl = returnUrl,
        };

        return View(model);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> ConfirmEmailChange(
        Guid userId,
        string email,
        string code,
        string? returnUrl = null,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(
            new ConfirmEmailChangeCommand(userId, email, code),
            cancellationToken
        );

        var model = new ConfirmEmailViewModel
        {
            Succeeded = result.IsSuccess,
            Message = result.IsSuccess
                ? "Your email address has been changed successfully."
                : result.TopError.Description,
            ReturnUrl = returnUrl,
        };

        return View(nameof(ConfirmEmail), model);
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginRequest());
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        LoginRequest request,
        string? returnUrl = null,
        CancellationToken cancellationToken = default
    )
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(request);

        var result = await _mediator.Send(
            new LoginCommand(request.Email, request.Password, request.RememberMe),
            cancellationToken
        );

        if (result.IsSuccess)
            return RedirectToLocal(returnUrl);

        var code = result.TopError.Code;

        if (code is "Auth.AccountLocked" or "Auth.LockedOut")
            return RedirectToAction(nameof(Lockout));

        if (code == "Auth.TwoFactorRequired")
            return RedirectToAction(
                nameof(LoginWith2fa),
                new { returnUrl, rememberMe = request.RememberMe }
            );

        if (code is "Auth.InvalidCredentials" or "Auth.EmailNotConfirmed")
        {
            ModelState.AddModelError(string.Empty, result.TopError.Description);
            return View(request);
        }

        return HandleError(result.Errors, request);
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult LoginWith2fa(bool rememberMe, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        ViewData["RememberMe"] = rememberMe;
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginWith2fa(
        LoginWith2faRequest request,
        bool rememberMe,
        string? returnUrl = null,
        CancellationToken cancellationToken = default
    )
    {
        ViewData["ReturnUrl"] = returnUrl;
        ViewData["RememberMe"] = rememberMe;

        if (!ModelState.IsValid)
            return View(request);

        var result = await _mediator.Send(
            new LoginWith2faCommand(request.TwoFactorCode, rememberMe, request.RememberMachine),
            cancellationToken
        );

        if (result.IsSuccess)
            return RedirectToLocal(returnUrl);

        if (result.TopError.Code is "Auth.AccountLocked" or "Auth.LockedOut")
            return RedirectToAction(nameof(Lockout));

        return HandleError(result.Errors, request);
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult LoginWithRecoveryCode(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginWithRecoveryCodeRequest());
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginWithRecoveryCode(
        LoginWithRecoveryCodeRequest request,
        string? returnUrl = null,
        CancellationToken cancellationToken = default
    )
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(request);

        var result = await _mediator.Send(
            new LoginWithRecoveryCodeCommand(request.RecoveryCode),
            cancellationToken
        );

        if (result.IsSuccess)
            return RedirectToLocal(returnUrl);

        if (result.TopError.Code is "Auth.AccountLocked" or "Auth.LockedOut")
            return RedirectToAction(nameof(Lockout));

        return HandleError(result.Errors, request);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(
        string? returnUrl = null,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(new LogoutCommand(), cancellationToken);

        return result.Match(
            _ => RedirectToLocal(returnUrl, fallbackAction: nameof(Login)),
            errors => HandleError(errors)
        );
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult AccessDenied() => View();

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Lockout() => View();

    private IActionResult RedirectToLocal(
        string? returnUrl,
        string fallbackAction = nameof(HomeController.Index)
    )
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return LocalRedirect(returnUrl);

        return fallbackAction == nameof(HomeController.Index)
            ? RedirectToAction(nameof(HomeController.Index), "Home")
            : RedirectToAction(fallbackAction);
    }
}
