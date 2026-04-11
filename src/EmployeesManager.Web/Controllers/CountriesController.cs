using EmployeesManager.Application.Features.Countries.Commands.CreateCountry;
using EmployeesManager.Application.Features.Countries.Commands.DeleteCountry;
using EmployeesManager.Application.Features.Countries.Commands.UpdateCountry;
using EmployeesManager.Application.Features.Countries.Queries.GetAllCountries;
using EmployeesManager.Application.Features.Countries.Queries.GetCountryById;
using EmployeesManager.Contracts.Requests.Countries;
using EmployeesManager.Contracts.Responses.Countries;
using EmployeesManager.Web.Mappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesManager.Web.Controllers;

//[Authorize]
[Route("[controller]/[action]")]
public sealed class CountriesController : MvcController
{
    private readonly IMediator _mediator;

    public CountriesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Route("/[controller]")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllCountriesQuery(), cancellationToken);
        return result.Match(items => View(items.ToResponses()), errors => HandleError(errors));
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateCountryRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return View(request);

        var command = new CreateCountryCommand(request.Code, request.Name);

        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(
            _ => RedirectToAction(nameof(Index)),
            errors => HandleError(errors, request)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCountryByIdQuery(id), cancellationToken);
        return result.Match(
            item =>
            {
                ViewBag.Id = item.Id;
                var request = new UpdateCountryRequest { Code = item.Code, Name = item.Name };
                return View(request);
            },
            errors => HandleError(errors)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCountryByIdQuery(id), cancellationToken);
        return result.Match(item => View(item.ToResponse()), errors => HandleError(errors));
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id,
        UpdateCountryRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return View(request);

        var command = new UpdateCountryCommand(Id: id, Code: request.Code, Name: request.Name);

        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(
            _ => RedirectToAction(nameof(Index)),
            errors => HandleError(errors, request)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCountryByIdQuery(id), cancellationToken);
        return result.Match(item => View(item.ToResponse()), errors => HandleError(errors));
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteCountryCommand(id), cancellationToken);
        return result.Match(_ => RedirectToAction(nameof(Index)), errors => HandleError(errors));
    }
}
