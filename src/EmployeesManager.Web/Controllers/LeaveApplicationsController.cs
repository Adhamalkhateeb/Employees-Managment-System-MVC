using EmployeesManager.Application.Features.Employees.Queries.GetAllEmployees;
using EmployeesManager.Application.Features.LeaveApplications.Commands.CreateLeaveApplication;
using EmployeesManager.Application.Features.LeaveApplications.Commands.DeleteLeaveApplication;
using EmployeesManager.Application.Features.LeaveApplications.Commands.UpdateLeaveApplication;
using EmployeesManager.Application.Features.LeaveApplications.Common;
using EmployeesManager.Application.Features.LeaveApplications.Queries.GetAllLeaveApplications;
using EmployeesManager.Application.Features.LeaveApplications.Queries.GetLeaveApplicationById;
using EmployeesManager.Application.Features.LeaveTypes.Queries.GetAllLeaveTypes;
using EmployeesManager.Application.Features.SystemCodeDetails.Common;
using EmployeesManager.Application.Features.SystemCodeDetails.Queries.GetSystemCodeDetailsBySystemCode;
using EmployeesManager.Contracts.Requests.LeaveApplications;
using EmployeesManager.Web.Mappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeesManager.Web.Controllers;

// [Authorize]
[Route("[controller]/[action]")]
public sealed class LeaveApplicationsController : MvcController
{
    private readonly IMediator _mediator;

    public LeaveApplicationsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Route("/[controller]")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllLeaveApplicationsQuery(), cancellationToken);
        return result.Match(items => View(items.ToResponses()), errors => HandleError(errors));
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await LoadLeaveApplicationLookupsAsync(cancellationToken);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateLeaveApplicationRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
        {
            await LoadLeaveApplicationLookupsAsync(cancellationToken);
            return View(request);
        }

        var command = new CreateLeaveApplicationCommand(
            request.EmployeeId,
            request.LeaveTypeId,
            request.DurationId,
            request.StatusId,
            request.StartDate,
            request.EndDate,
            request.Description,
            request.Attachment
        );

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));

        await LoadLeaveApplicationLookupsAsync(cancellationToken);
        return HandleError(result.Errors, request);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        await LoadLeaveApplicationLookupsAsync(cancellationToken);

        var result = await _mediator.Send(new GetLeaveApplicationByIdQuery(id), cancellationToken);
        return result.Match(
            item =>
            {
                ViewBag.Id = item.Id;
                var request = new UpdateLeaveApplicationRequest
                {
                    EmployeeId = item.EmployeeId,
                    LeaveTypeId = item.LeaveTypeId,
                    DurationId = item.DurationId,
                    StatusId = item.StatusId,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    Description = item.Description,
                    Attachment = item.Attachment,
                };
                return View(request);
            },
            errors => HandleError(errors)
        );
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id,
        UpdateLeaveApplicationRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
        {
            await LoadLeaveApplicationLookupsAsync(cancellationToken);
            return View(request);
        }

        var command = new UpdateLeaveApplicationCommand(
            Id: id,
            EmployeeId: request.EmployeeId,
            LeaveTypeId: request.LeaveTypeId,
            DurationId: request.DurationId,
            StatusId: request.StatusId,
            StartDate: request.StartDate,
            EndDate: request.EndDate,
            Description: request.Description,
            Attachment: request.Attachment
        );

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));

        await LoadLeaveApplicationLookupsAsync(cancellationToken);
        return HandleError(result.Errors, request);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLeaveApplicationByIdQuery(id), cancellationToken);
        return result.Match(item => View(item.ToResponse()), errors => HandleError(errors));
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteLeaveApplicationCommand(id), cancellationToken);
        return result.Match(_ => RedirectToAction(nameof(Index)), errors => HandleError(errors));
    }

    private async Task LoadLeaveApplicationLookupsAsync(CancellationToken cancellationToken)
    {
        var employees = await _mediator.Send(new GetAllEmployeesQuery(), cancellationToken);
        var leaveTypes = await _mediator.Send(new GetAllLeaveTypesQuery(), cancellationToken);
        var durationDetails = await _mediator.Send(
            new GetSystemCodeDetailsBySystemCodeQuery(
                SystemCodeLookUpConstants.LeaveDurationSystemCode
            ),
            cancellationToken
        );
        var statusDetails = await _mediator.Send(
            new GetSystemCodeDetailsBySystemCodeQuery(
                SystemCodeLookUpConstants.LeaveApplicationStatusSystemCode
            ),
            cancellationToken
        );

        ViewBag.Employees = employees.Match(
            value =>
                value
                    .Select(x => new SelectListItem($"{x.FirstName} {x.LastName}", x.Id.ToString()))
                    .ToList(),
            _ => []
        );

        ViewBag.LeaveTypes = leaveTypes.Match(
            value => value.Select(x => new SelectListItem($"{x.Code}", x.Id.ToString())).ToList(),
            _ => []
        );

        ViewBag.Durations = durationDetails.Match(
            value =>
                value
                    .Select(x => new SelectListItem(x.Description ?? x.Code, x.Id.ToString()))
                    .ToList(),
            _ => []
        );

        ViewBag.Statuses = statusDetails.Match(
            value =>
                value
                    .Select(x => new SelectListItem(x.Description ?? x.Code, x.Id.ToString()))
                    .ToList(),
            _ => []
        );
    }
}
