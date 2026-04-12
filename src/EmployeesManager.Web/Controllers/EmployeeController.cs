using EmployeesManager.Application.Features.Departments.Queries.GetAllDepartments;
using EmployeesManager.Application.Features.Employees.Commands.CreateEmployee;
using EmployeesManager.Application.Features.Employees.Commands.DeleteEmployee;
using EmployeesManager.Application.Features.Employees.Commands.UpdateEmployee;
using EmployeesManager.Application.Features.Employees.Queries.GetAllEmployees;
using EmployeesManager.Application.Features.Employees.Queries.GetEmployeeById;
using EmployeesManager.Contracts.Requests.Employees;
using EmployeesManager.Web.Mappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeesManager.Web.Controllers;

// [Authorize]
[Route("[controller]/[action]")]
public sealed class EmployeesController : MvcController
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Route("/[controller]")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllEmployeesQuery(), cancellationToken);
        return result.Match(
            employees => View(employees.ToResponses()),
            errors => HandleError(errors)
        );
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await LoadEmployeeLookupsAsync(cancellationToken);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateEmployeeRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
        {
            await LoadEmployeeLookupsAsync(cancellationToken);
            return View(request);
        }

        var command = new CreateEmployeeCommand(
            request.FirstName,
            request.LastName,
            request.NationalId,
            request.PhoneNumber,
            request.EmailAddress,
            request.HireDate,
            request.Address,
            request.DepartmentId,
            request.BranchId
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));

        await LoadEmployeeLookupsAsync(cancellationToken);
        return HandleError(result.Errors, request);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        await LoadEmployeeLookupsAsync(cancellationToken);

        var result = await _mediator.Send(new GetEmployeeByIdQuery(id), cancellationToken);
        return result.Match(
            item =>
            {
                ViewBag.Id = item.Id;
                var request = new UpdateEmployeeRequest
                {
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    NationalId = item.NationalId,
                    PhoneNumber = item.PhoneNumber,
                    EmailAddress = item.EmailAddress,
                    HireDate = item.HireDate,
                    Address = item.Address,
                    DepartmentId = item.DepartmentId,
                    BranchId = item.BranchId,
                };
                return View(request);
            },
            errors => HandleError(errors)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetEmployeeByIdQuery(id), cancellationToken);
        return result.Match(item => View(item.ToResponse()), errors => HandleError(errors));
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id,
        UpdateEmployeeRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
        {
            await LoadEmployeeLookupsAsync(cancellationToken);
            return View(request);
        }

        var command = new UpdateEmployeeCommand(
            Id: id,
            FirstName: request.FirstName,
            LastName: request.LastName,
            NationalId: request.NationalId,
            PhoneNumber: request.PhoneNumber,
            EmailAddress: request.EmailAddress,
            HireDate: request.HireDate,
            Address: request.Address,
            DepartmentId: request.DepartmentId,
            BranchId: request.BranchId
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));

        await LoadEmployeeLookupsAsync(cancellationToken);
        return HandleError(result.Errors, request);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetEmployeeByIdQuery(id), cancellationToken);
        return result.Match(item => View(item.ToResponse()), errors => HandleError(errors));
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteEmployeeCommand(id), cancellationToken);
        return result.Match(_ => RedirectToAction(nameof(Index)), errors => HandleError(errors));
    }

    private async Task LoadEmployeeLookupsAsync(CancellationToken cancellationToken)
    {
        var departments = await _mediator.Send(
            new GetDepartmentsQuery(null, 1, 200),
            cancellationToken
        );

        ViewBag.Departments = departments.IsSuccess
            ? departments
                .Value.Items.Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList()
            : [];
    }
}
