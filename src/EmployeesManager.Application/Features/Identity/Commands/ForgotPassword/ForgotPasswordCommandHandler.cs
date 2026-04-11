// using EmployeesManager.Application.Common.Interfaces;
// using EmployeesManager.Domain.Common.Results;
// using MediatR;

// namespace EmployeesManager.Application.Features.Identity.Commands.ForgotPassword;

// public sealed class ForgotPasswordCommandHandler
//     : IRequestHandler<ForgotPasswordCommand, Result<Updated>>
// {
//     private readonly IIdentityService _identityService;

//     public ForgotPasswordCommandHandler(IIdentityService identityService) =>
//         _identityService = identityService;

//     public Task<Result<Updated>> Handle(
//         ForgotPasswordCommand request,
//         CancellationToken cancellationToken
//     ) =>
//         _identityService.SendPasswordResetAsync(
//             request.Email,
//             request.ResetPasswordBaseUrl,
//             request.ReturnUrl,
//             cancellationToken
//         );
// }
