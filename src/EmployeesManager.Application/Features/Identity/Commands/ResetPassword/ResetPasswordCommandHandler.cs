// using EmployeesManager.Application.Common.Interfaces;
// using EmployeesManager.Domain.Common.Results;
// using MediatR;

// namespace EmployeesManager.Application.Features.Identity.Commands.ResetPassword;

// public sealed class ResetPasswordCommandHandler
//     : IRequestHandler<ResetPasswordCommand, Result<Updated>>
// {
//     private readonly IIdentityService _identityService;

//     public ResetPasswordCommandHandler(IIdentityService identityService) =>
//         _identityService = identityService;

//     public Task<Result<Updated>> Handle(
//         ResetPasswordCommand request,
//         CancellationToken cancellationToken
//     ) =>
//         _identityService.ResetPasswordAsync(
//             request.Email,
//             request.Code,
//             request.Password,
//             cancellationToken
//         );
// }
