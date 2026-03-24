using EmployeesManager.Application.Features.Cities.Commands.CreateCity;
using EmployeesManager.Application.Features.Cities.Commands.DeleteCity;
using EmployeesManager.Application.Features.Cities.Commands.UpdateCity;
using EmployeesManager.Application.Features.Cities.Queries.GetAllCities;
using EmployeesManager.Application.Features.Cities.Queries.GetCityById;
using EmployeesManager.Application.Features.Countries.Queries.GetAllCountries;
using EmployeesManager.Contracts.Requests.Cities;
using EmployeesManager.Web.Mappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeesManager.Web.Controllers;

//[Authorize]
[Route("[controller]/[action]")]
public sealed class CitiesController : MvcController
{
    private readonly IMediator _mediator;

    public CitiesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Route("/[controller]")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllCitiesQuery(), cancellationToken);
        return result.Match(items => View(items.ToResponses()), errors => HandleError(errors));
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await LoadCountryLookupsAsync(cancellationToken);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateCityRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
        {
            await LoadCountryLookupsAsync(cancellationToken);
            return View(request);
        }

        var command = new CreateCityCommand(request.Code, request.Name, request.CountryId);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));

        await LoadCountryLookupsAsync(cancellationToken);
        return HandleError(result.Errors, request);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        await LoadCountryLookupsAsync(cancellationToken);

        var result = await _mediator.Send(new GetCityByIdQuery(id), cancellationToken);
        return result.Match(
            item =>
            {
                ViewBag.Id = item.Id;
                var request = new UpdateCityRequest
                {
                    Code = item.Code,
                    Name = item.Name,
                    CountryId = item.CountryId,
                };
                return View(request);
            },
            errors => HandleError(errors)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCityByIdQuery(id), cancellationToken);
        return result.Match(item => View(item.ToResponse()), errors => HandleError(errors));
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id,
        UpdateCityRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
        {
            await LoadCountryLookupsAsync(cancellationToken);
            return View(request);
        }

        var command = new UpdateCityCommand(
            Id: id,
            Code: request.Code,
            Name: request.Name,
            CountryId: request.CountryId
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));

        await LoadCountryLookupsAsync(cancellationToken);
        return HandleError(result.Errors, request);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCityByIdQuery(id), cancellationToken);
        return result.Match(item => View(item.ToResponse()), errors => HandleError(errors));
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteCityCommand(id), cancellationToken);
        return result.Match(_ => RedirectToAction(nameof(Index)), errors => HandleError(errors));
    }

    private async Task LoadCountryLookupsAsync(CancellationToken cancellationToken)
    {
        var countries = await _mediator.Send(new GetAllCountriesQuery(), cancellationToken);

        ViewBag.Countries = countries.IsSuccess
            ? countries
                .Value.Select(x => new SelectListItem($"{x.Name} ({x.Code})", x.Id.ToString()))
                .ToList()
            : [];
    }
}
