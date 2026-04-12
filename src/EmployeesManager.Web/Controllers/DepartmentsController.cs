using EmployeesManager.Application.Features.Departments.Commands.CreateDepartment;
using EmployeesManager.Application.Features.Departments.Commands.DeleteDepartment;
using EmployeesManager.Application.Features.Departments.Commands.UpdateDepartment;
using EmployeesManager.Application.Features.Departments.Queries.GetAllDepartments;
using EmployeesManager.Application.Features.Departments.Queries.GetDepartmentById;
using EmployeesManager.Contracts.Requests.Departments;
using EmployeesManager.Web.Mappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesManager.Web.Controllers;

// [Authorize]
[Route("[controller]/[action]")]
public sealed class DepartmentsController : MvcController
{
    private readonly ISender _sender;

    public DepartmentsController(ISender mediator) => _sender = mediator;

    [HttpGet]
    [Route("/[controller]")]
    public async Task<IActionResult> Index(
        string? search,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        var result = await _sender.Send(
            new GetDepartmentsQuery(search, pageNumber, pageSize),
            cancellationToken
        );
        return result.Match(
            departments => View(departments.Items.ToResponses()),
            errors => HandleError(errors)
        );
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateDepartmentRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return View(request);

        var command = new CreateDepartmentCommand(request.Name, request.ManagerId);

        var result = await _sender.Send(command, cancellationToken);
        return result.Match(
            _ => RedirectToAction(nameof(Index)),
            errors => HandleError(errors, request)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetDepartmentByIdQuery(id), cancellationToken);
        return result.Match(
            item =>
            {
                ViewBag.Id = item.Id;
                var request = new UpdateDepartmentRequest
                {
                    Name = item.Name,
                    ManagerId = item.ManagerId,
                };
                return View(request);
            },
            errors => HandleError(errors)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetDepartmentByIdQuery(id), cancellationToken);
        return result.Match(item => View(item.ToResponse()), errors => HandleError(errors));
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id,
        UpdateDepartmentRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return View(request);

        var command = new UpdateDepartmentCommand(id, request.Name, request.ManagerId);

        var result = await _sender.Send(command, cancellationToken);
        return result.Match(
            _ => RedirectToAction(nameof(Index)),
            errors => HandleError(errors, request)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetDepartmentByIdQuery(id), cancellationToken);
        return result.Match(item => View(item.ToResponse()), errors => HandleError(errors));
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteDepartmentCommand(id), cancellationToken);
        return result.Match(_ => RedirectToAction(nameof(Index)), errors => HandleError(errors));
    }
}
