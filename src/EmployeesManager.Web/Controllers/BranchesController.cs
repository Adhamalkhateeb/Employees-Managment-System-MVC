using EmployeesManager.Application.Features.Branches.Commands.CreateBranch;
using EmployeesManager.Application.Features.Branches.Commands.DeleteBranch;
using EmployeesManager.Application.Features.Branches.Commands.UpdateBranch;
using EmployeesManager.Application.Features.Branches.Queries.GetAllBranchs;
using EmployeesManager.Application.Features.Branches.Queries.GetBranchById;
using EmployeesManager.Contracts.Requests.Branchs;
using EmployeesManager.Contracts.Responses.Branchs;
using EmployeesManager.Web.Mappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesManager.Web.Controllers;

[Authorize]
[Route("[controller]/[action]")]
public sealed class BranchsController : MvcController
{
    private readonly IMediator _mediator;

    public BranchsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Route("/[controller]")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBranchesQuery(), cancellationToken);
        return result.Match(items => View(items.ToResponses()), errors => HandleError(errors));
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateBranchRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return View(request);

        var command = new CreateBranchCommand(
            request.Name,
            request.Address,
            request.PhoneNumber,
            request.EmailAddress,
            request.ManagerId
        );

        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(
            _ => RedirectToAction(nameof(Index)),
            errors => HandleError(errors, request)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBranchByIdQuery(id), cancellationToken);
        return result.Match(
            item =>
            {
                ViewBag.Id = item.Id;
                var request = new UpdateBranchRequest
                {
                    Name = item.Name,
                    Address = item.Address,
                    PhoneNumber = item.PhoneNumber,
                    EmailAddress = item.EmailAddress,
                    ManagerId = item.ManagerId,
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
        UpdateBranchRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return View(request);

        var command = new UpdateBranchCommand(
            Id: id,
            Name: request.Name,
            Address: request.Address,
            PhoneNumber: request.PhoneNumber,
            EmailAddress: request.EmailAddress,
            ManagerId: request.ManagerId
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
        var result = await _mediator.Send(new GetBranchByIdQuery(id), cancellationToken);
        return result.Match(item => View(item.ToResponse()), errors => HandleError(errors));
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteBranchCommand(id), cancellationToken);
        return result.Match(_ => RedirectToAction(nameof(Index)), errors => HandleError(errors));
    }
}
