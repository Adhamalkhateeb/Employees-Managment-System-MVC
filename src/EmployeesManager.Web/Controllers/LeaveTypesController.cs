using EmployeesManager.Application.Features.LeaveTypes.Commands.CreateLeaveType;
using EmployeesManager.Application.Features.LeaveTypes.Commands.DeleteLeaveType;
using EmployeesManager.Application.Features.LeaveTypes.Commands.UpdateLeaveType;
using EmployeesManager.Application.Features.LeaveTypes.Queries.GetAllLeaveTypes;
using EmployeesManager.Application.Features.LeaveTypes.Queries.GetLeaveTypeById;
using EmployeesManager.Contracts.Requests.LeaveTypes;
using EmployeesManager.Contracts.Responses.LeaveTypes;
using EmployeesManager.Web.Mappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesManager.Web.Controllers;

// [Authorize]
[Route("[controller]/[action]")]
public sealed class LeaveTypesController : MvcController
{
    private readonly IMediator _mediator;

    public LeaveTypesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Route("/[controller]")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllLeaveTypesQuery(), cancellationToken);
        return result.Match(items => View(items.ToResponses()), errors => HandleError(errors));
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateLeaveTypeRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return View(request);

        var command = new CreateLeaveTypeCommand(request.Name, request.Code);

        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(
            _ => RedirectToAction(nameof(Index)),
            errors => HandleError(errors, request)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLeaveTypeByIdQuery(id), cancellationToken);
        return result.Match(
            item =>
            {
                ViewBag.Id = item.Id;
                var request = new UpdateLeaveTypeRequest { Name = item.Name, Code = item.Code };
                return View(request);
            },
            errors => HandleError(errors)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLeaveTypeByIdQuery(id), cancellationToken);
        return result.Match(item => View(item.ToResponse()), errors => HandleError(errors));
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id,
        UpdateLeaveTypeRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return View(request);

        var command = new UpdateLeaveTypeCommand(Id: id, Name: request.Name, Code: request.Code);

        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(
            _ => RedirectToAction(nameof(Index)),
            errors => HandleError(errors, request)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLeaveTypeByIdQuery(id), cancellationToken);
        return result.Match(item => View(item.ToResponse()), errors => HandleError(errors));
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteLeaveTypeCommand(id), cancellationToken);
        return result.Match(_ => RedirectToAction(nameof(Index)), errors => HandleError(errors));
    }
}
