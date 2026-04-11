using EmployeesManager.Application.Features.SystemCodeDetails.Commands.CreateSystemCodeDetail;
using EmployeesManager.Application.Features.SystemCodeDetails.Commands.DeleteSystemCodeDetail;
using EmployeesManager.Application.Features.SystemCodeDetails.Commands.UpdateSystemCodeDetail;
using EmployeesManager.Application.Features.SystemCodeDetails.Queries.GetAllSystemCodeDetails;
using EmployeesManager.Application.Features.SystemCodeDetails.Queries.GetSystemCodeDetailById;
using EmployeesManager.Application.Features.SystemCodes.Queries.GetAllSystemCodes;
using EmployeesManager.Contracts.Requests.SystemCodeDetails;
using EmployeesManager.Web.Mappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeesManager.Web.Controllers;

// [Authorize]
[Route("[controller]/[action]")]
public sealed class SystemCodeDetailsController : MvcController
{
    private readonly IMediator _mediator;

    public SystemCodeDetailsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Route("/[controller]")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllSystemCodeDetailsQuery(), cancellationToken);
        return result.Match(items => View(items.ToResponses()), errors => HandleError(errors));
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await LoadSystemCodeLookupsAsync(cancellationToken);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateSystemCodeDetailRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
        {
            await LoadSystemCodeLookupsAsync(cancellationToken);
            return View(request);
        }

        var command = new CreateSystemCodeDetailCommand(
            request.SystemCodeId,
            request.Code,
            request.Description,
            request.OrderNo
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));

        await LoadSystemCodeLookupsAsync(cancellationToken);
        return HandleError(result.Errors, request);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        await LoadSystemCodeLookupsAsync(cancellationToken);

        var result = await _mediator.Send(new GetSystemCodeDetailByIdQuery(id), cancellationToken);
        return result.Match(
            item =>
            {
                ViewBag.Id = item.Id;
                var request = new UpdateSystemCodeDetailRequest
                {
                    SystemCodeId = item.SystemCodeId,
                    Code = item.Code,
                    Description = item.Description,
                    OrderNo = item.OrderNo,
                };
                return View(request);
            },
            errors => HandleError(errors)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSystemCodeDetailByIdQuery(id), cancellationToken);
        return result.Match(item => View(item.ToResponse()), errors => HandleError(errors));
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id,
        UpdateSystemCodeDetailRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
        {
            await LoadSystemCodeLookupsAsync(cancellationToken);
            return View(request);
        }

        var command = new UpdateSystemCodeDetailCommand(
            Id: id,
            SystemCodeId: request.SystemCodeId,
            Code: request.Code,
            Description: request.Description,
            OrderNo: request.OrderNo
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));

        await LoadSystemCodeLookupsAsync(cancellationToken);
        return HandleError(result.Errors, request);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSystemCodeDetailByIdQuery(id), cancellationToken);
        return result.Match(item => View(item.ToResponse()), errors => HandleError(errors));
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteSystemCodeDetailCommand(id), cancellationToken);
        return result.Match(_ => RedirectToAction(nameof(Index)), errors => HandleError(errors));
    }

    private async Task LoadSystemCodeLookupsAsync(CancellationToken cancellationToken)
    {
        var systemCodes = await _mediator.Send(new GetAllSystemCodesQuery(), cancellationToken);

        ViewBag.SystemCodes = systemCodes.IsSuccess
            ? systemCodes
                .Value.Select(x => new SelectListItem($"{x.Code}", x.Id.ToString()))
                .ToList()
            : [];
    }
}
