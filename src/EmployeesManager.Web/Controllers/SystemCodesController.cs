using EmployeesManager.Application.Features.SystemCodes.Commands.CreateSystemCode;
using EmployeesManager.Application.Features.SystemCodes.Commands.DeleteSystemCode;
using EmployeesManager.Application.Features.SystemCodes.Commands.UpdateSystemCode;
using EmployeesManager.Application.Features.SystemCodes.Queries.GetAllSystemCodes;
using EmployeesManager.Application.Features.SystemCodes.Queries.GetSystemCodeById;
using EmployeesManager.Contracts.Requests.SystemCodes;
using EmployeesManager.Web.Mappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesManager.Web.Controllers;

// [Authorize]
[Route("[controller]/[action]")]
public sealed class SystemCodesController : MvcController
{
    private readonly IMediator _mediator;

    public SystemCodesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Route("/[controller]")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllSystemCodesQuery(), cancellationToken);
        return result.Match(items => View(items.ToResponses()), errors => HandleError(errors));
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateSystemCodeRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return View(request);

        var command = new CreateSystemCodeCommand(request.Code, request.Description);

        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(
            _ => RedirectToAction(nameof(Index)),
            errors => HandleError(errors, request)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSystemCodeByIdQuery(id), cancellationToken);
        return result.Match(
            item =>
            {
                ViewBag.Id = item.Id;
                var request = new UpdateSystemCodeRequest
                {
                    Description = item.Description,
                    Code = item.Code,
                };
                return View(request);
            },
            errors => HandleError(errors)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSystemCodeByIdQuery(id), cancellationToken);
        return result.Match(item => View(item.ToResponse()), errors => HandleError(errors));
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id,
        UpdateSystemCodeRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return View(request);

        var command = new UpdateSystemCodeCommand(
            Id: id,
            Description: request.Description,
            Code: request.Code
        );

        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(
            _ => RedirectToAction(nameof(Index)),
            errors => HandleError(errors, request)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSystemCodeByIdQuery(id), cancellationToken);
        return result.Match(item => View(item.ToResponse()), errors => HandleError(errors));
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteSystemCodeCommand(id), cancellationToken);
        return result.Match(_ => RedirectToAction(nameof(Index)), errors => HandleError(errors));
    }
}
