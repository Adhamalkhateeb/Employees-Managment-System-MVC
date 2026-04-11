using EmployeesManager.Application.Features.LeaveApplications.Commands.ApproveLeaveApplication;
using EmployeesManager.Application.Features.LeaveApplications.Commands.CancelLeaveApplication;
using EmployeesManager.Application.Features.LeaveApplications.Commands.DeleteLeaveApplication;
using EmployeesManager.Application.Features.LeaveApplications.Commands.RejectLeaveApplication;
using EmployeesManager.Application.Features.LeaveApplications.Queries.GetAllLeaveApplications;
using EmployeesManager.Application.Features.LeaveApplications.Queries.GetLeaveApplicationById;
using EmployeesManager.Application.Features.LeaveApplications.Queries.GetLeaveApplicationLookups;
using EmployeesManager.Contracts.Requests.LeaveApplications;
using EmployeesManager.Domain.Common.Results;
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

        return result.Match(
            leaveApplications => View(leaveApplications.ToResponses()),
            errors => HandleError(errors)
        );
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await LoadLookupsAsync(cancellationToken);
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
            await LoadLookupsAsync(cancellationToken);
            return View(request);
        }

        var result = await _mediator.Send(request.ToCommand(), cancellationToken);

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));

        await LoadLookupsAsync(cancellationToken);
        return HandleError(result.Errors, request);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLeaveApplicationByIdQuery(id), cancellationToken);

        if (result.IsError)
            return HandleError(result.Errors);

        if (result.Value.Status != LeaveApplicationStatus.Pending)
        {
            TempData["WorkflowError"] = "Only pending leave applications can be edited.";
            return RedirectToAction(nameof(Index));
        }

        await LoadLookupsAsync(cancellationToken);
        ViewBag.Id = result.Value.Id;
        return View(result.Value.ToUpdateRequest());
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
            await LoadLookupsAsync(cancellationToken);
            return View(request);
        }

        var currentItemResult = await _mediator.Send(
            new GetLeaveApplicationByIdQuery(id),
            cancellationToken
        );

        if (currentItemResult.IsError)
            return HandleError(currentItemResult.Errors);

        if (currentItemResult.Value.Status != LeaveApplicationStatus.Pending)
        {
            TempData["WorkflowError"] = "Only pending leave applications can be edited.";
            return RedirectToAction(nameof(Index));
        }

        var result = await _mediator.Send(
            request.ToCommand(id, currentItemResult.Value.Status),
            cancellationToken
        );

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));

        await LoadLookupsAsync(cancellationToken);
        return HandleError(result.Errors, request);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLeaveApplicationByIdQuery(id), cancellationToken);
        return result.Match(
            leaveApplication => View(leaveApplication.ToResponse()),
            errors => HandleError(errors)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLeaveApplicationByIdQuery(id), cancellationToken);

        if (result.IsError)
            return HandleError(result.Errors);

        if (result.Value.Status != LeaveApplicationStatus.Pending)
        {
            TempData["WorkflowError"] = "Only pending leave applications can be deleted.";
            return RedirectToAction(nameof(Index));
        }

        return View(result.Value);
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteLeaveApplicationCommand(id), cancellationToken);
        return result.Match(_ => RedirectToAction(nameof(Index)), errors => HandleError(errors));
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ApproveLeaveApplicationCommand(id),
            cancellationToken
        );

        if (result.IsSuccess)
        {
            TempData["WorkflowSuccess"] = "Leave application approved successfully.";
            return RedirectToAction(nameof(Index));
        }

        return HandleWorkflowErrors(result.Errors);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Reject(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLeaveApplicationByIdQuery(id), cancellationToken);

        if (result.IsError)
            return HandleError(result.Errors);

        if (result.Value.Status != LeaveApplicationStatus.Pending)
        {
            TempData["WorkflowError"] = "Only pending leave applications can be rejected.";
            return RedirectToAction(nameof(Details), new { id });
        }

        return View(result.Value.ToResponse());
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(
        Guid id,
        RejectLeaveApplicationRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return await ReturnRejectViewAsync(id, request.RejectionReason, cancellationToken);

        var result = await _mediator.Send(
            new RejectLeaveApplicationCommand(id, request.RejectionReason),
            cancellationToken
        );

        if (result.IsSuccess)
        {
            TempData["WorkflowSuccess"] = "Leave application rejected.";
            return RedirectToAction(nameof(Index));
        }

        return HandleWorkflowErrors(result.Errors);
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CancelLeaveApplicationCommand(id), cancellationToken);

        if (result.IsSuccess)
        {
            TempData["WorkflowSuccess"] = "Leave application cancelled.";
            return RedirectToAction(nameof(Index));
        }

        return HandleWorkflowErrors(result.Errors);
    }

    private async Task LoadLookupsAsync(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLeaveApplicationLookupsQuery(), cancellationToken);

        result.Match(
            values =>
            {
                ViewBag.Employees = values
                    .Employees.Select(e => new SelectListItem(e.FullName, e.Id.ToString()))
                    .ToList();

                ViewBag.LeaveTypes = values
                    .LeaveTypes.Select(lt => new SelectListItem(lt.Code, lt.Id.ToString()))
                    .ToList();

                ViewBag.Durations = values.Durations.Select(d => new SelectListItem(d, d)).ToList();

                return true;
            },
            _ => false
        );
    }

    private async Task<IActionResult> ReturnRejectViewAsync(
        Guid id,
        string? rejectionReason,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(new GetLeaveApplicationByIdQuery(id), cancellationToken);

        if (result.IsError)
            return HandleError(result.Errors);

        ViewBag.RejectionReason = rejectionReason ?? string.Empty;
        return View("Reject", result.Value.ToResponse());
    }

    private IActionResult HandleWorkflowErrors(List<Error> errors)
    {
        if (errors.All(e => e.Type is ErrorKind.Validation or ErrorKind.Conflict))
        {
            TempData["WorkflowError"] = string.Join(" ", errors.Select(e => e.Description));
            return RedirectToAction(nameof(Index));
        }

        return HandleError(errors);
    }
}
