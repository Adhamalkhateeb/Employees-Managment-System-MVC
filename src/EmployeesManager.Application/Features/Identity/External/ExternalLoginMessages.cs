// using EmployeesManager.Application.Common.Interfaces;
// using EmployeesManager.Application.Features.Identity.Dtos;
// using EmployeesManager.Domain.Common.Results;
// using MediatR;

// namespace EmployeesManager.Application.Features.Identity.External;

// public sealed record GetExternalAuthenticationSchemesQuery
//     : IRequest<Result<IReadOnlyList<ExternalAuthenticationSchemeDto>>>;

// public sealed class GetExternalAuthenticationSchemesQueryHandler(IIdentityService identityService)
//     : IRequestHandler<
//         GetExternalAuthenticationSchemesQuery,
//         Result<IReadOnlyList<ExternalAuthenticationSchemeDto>>
//     >
// {
//     private readonly IIdentityService _identityService = identityService;

//     public Task<Result<IReadOnlyList<ExternalAuthenticationSchemeDto>>> Handle(
//         GetExternalAuthenticationSchemesQuery request,
//         CancellationToken cancellationToken
//     ) => _identityService.GetExternalAuthenticationSchemesAsync(cancellationToken);
// }

// public sealed record ClearExternalLoginCookieCommand : IRequest<Result<Updated>>;

// public sealed class ClearExternalLoginCookieCommandHandler(IIdentityService identityService)
//     : IRequestHandler<ClearExternalLoginCookieCommand, Result<Updated>>
// {
//     private readonly IIdentityService _identityService = identityService;

//     public Task<Result<Updated>> Handle(
//         ClearExternalLoginCookieCommand request,
//         CancellationToken cancellationToken
//     ) => _identityService.ClearExternalLoginCookieAsync(cancellationToken);
// }

// public sealed record HandleExternalLoginCallbackQuery(string? RemoteError)
//     : IRequest<Result<ExternalLoginCallbackDto>>;

// public sealed class HandleExternalLoginCallbackQueryHandler(IIdentityService identityService)
//     : IRequestHandler<HandleExternalLoginCallbackQuery, Result<ExternalLoginCallbackDto>>
// {
//     private readonly IIdentityService _identityService = identityService;

//     public Task<Result<ExternalLoginCallbackDto>> Handle(
//         HandleExternalLoginCallbackQuery request,
//         CancellationToken cancellationToken
//     ) => _identityService.HandleExternalLoginCallbackAsync(request.RemoteError, cancellationToken);
// }

// public sealed record ConfirmExternalLoginCommand(
//     string Email,
//     string ConfirmationBaseUrl,
//     string? ReturnUrl
// ) : IRequest<Result<ExternalLoginConfirmationDto>>;

// public sealed class ConfirmExternalLoginCommandHandler(IIdentityService identityService)
//     : IRequestHandler<ConfirmExternalLoginCommand, Result<ExternalLoginConfirmationDto>>
// {
//     private readonly IIdentityService _identityService = identityService;

//     public Task<Result<ExternalLoginConfirmationDto>> Handle(
//         ConfirmExternalLoginCommand request,
//         CancellationToken cancellationToken
//     ) =>
//         _identityService.ConfirmExternalLoginAsync(
//             request.Email,
//             request.ConfirmationBaseUrl,
//             request.ReturnUrl,
//             cancellationToken
//         );
// }
