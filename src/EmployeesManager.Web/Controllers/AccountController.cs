using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Identity.Commands.Register;
using EmployeesManager.Application.Features.Identity.Queries.Login;
using EmployeesManager.Contracts.Requests.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesManager.Web.Controllers;

[Route("[controller]/[action]")]
public sealed class AccountController : MvcController
{
    private readonly IMediator _mediator;
    private readonly IIdentityService _identityService;

    public AccountController(IMediator mediator, IIdentityService identityService)
    {
        _mediator = mediator;
        _identityService = identityService;
    }

    // ── Register ──────────────────────────────────────────────────────────

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register() => View();

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        var command = new RegisterCommand(
            request.UserName,
            request.Email,
            request.PhoneNumber,
            request.Password
        );

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return RedirectToAction(nameof(Login));

        return HandleError(result.Errors, request);
    }

    // ── Login ─────────────────────────────────────────────────────────────

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login() => View();

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        var query = new LoginQuery(request.Email, request.Password, request.RememberMe);

        var result = await _mediator.Send(query);

        if (result.IsSuccess)
            return RedirectToAction("Index", "Home");

        return HandleError(result.Errors, request);
    }

    // ── Logout ────────────────────────────────────────────────────────────

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _identityService.LogoutAsync();
        return RedirectToAction(nameof(Login));
    }

    // ── Access Denied ──────────────────────────────────────────────────────

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied() => View();

    // ── Remote validation ─────────────────────────────────────────────────

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> IsEmailAvailable(string email)
    {
        var available = await _identityService.IsEmailAvailableAsync(email);
        return Json(available ? true : (object)"This email is already in use.");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> IsUserNameAvailable(string userName)
    {
        var available = await _identityService.IsUserNameAvailableAsync(userName);
        return Json(available ? true : (object)"This username is already taken.");
    }
}
