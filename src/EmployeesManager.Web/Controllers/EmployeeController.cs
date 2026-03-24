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
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateEmployeeRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return View(request);

        var command = new CreateEmployeeCommand(
            request.FirstName,
            request.MiddleName,
            request.LastName,
            request.PhoneNumber,
            request.EmailAddress,
            request.Country,
            request.DateOfBirth,
            request.Address,
            request.Department,
            request.Designation
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
        var result = await _mediator.Send(new GetEmployeeByIdQuery(id), cancellationToken);
        return result.Match(
            item =>
            {
                ViewBag.Id = item.Id;
                var request = new UpdateEmployeeRequest
                {
                    FirstName = item.FirstName,
                    MiddleName = item.MiddleName,
                    LastName = item.LastName,
                    PhoneNumber = item.PhoneNumber,
                    EmailAddress = item.EmailAddress,
                    Country = item.Country,
                    DateOfBirth = item.DateOfBirth,
                    Address = item.Address,
                    Department = item.Department,
                    Designation = item.Designation,
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
        UpdateEmployeeRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return View(request);

        var command = new UpdateEmployeeCommand(
            Id: id,
            FirstName: request.FirstName,
            MiddleName: request.MiddleName,
            LastName: request.LastName,
            PhoneNumber: request.PhoneNumber,
            EmailAddress: request.EmailAddress,
            Country: request.Country,
            DateOfBirth: request.DateOfBirth,
            Address: request.Address,
            Department: request.Department,
            Designation: request.Designation
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
}
