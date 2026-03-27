// using EmployeesManager.Application.Common.Interfaces;
// using EmployeesManager.Application.Features.Identity.Dtos;
// using EmployeesManager.Domain.Common.Results;
// using MediatR;

// namespace EmployeesManager.Application.Features.Identity.Manage;

// public sealed record GetManageProfileQuery : IRequest<Result<ManageProfileDto>>;

// public sealed class GetManageProfileQueryHandler(
//     IIdentityService identityService,
//     ICurrentUser currentUser
// ) : IRequestHandler<GetManageProfileQuery, Result<ManageProfileDto>>
// {
//     private readonly IIdentityService _identityService = identityService;
//     private readonly ICurrentUser _currentUser = currentUser;

//     public Task<Result<ManageProfileDto>> Handle(
//         GetManageProfileQuery request,
//         CancellationToken cancellationToken
//     )
//     {
//         if (!_currentUser.IsAuthenticated)
//             return Task.FromResult<Result<ManageProfileDto>>(
//                 Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
//             );

//         return _identityService.GetProfileAsync(_currentUser.Id, cancellationToken);
//     }
// }

// public sealed record UpdateManageProfilePhoneCommand(string? PhoneNumber)
//     : IRequest<Result<Updated>>;

// public sealed class UpdateManageProfilePhoneCommandHandler(
//     IIdentityService identityService,
//     ICurrentUser currentUser
// ) : IRequestHandler<UpdateManageProfilePhoneCommand, Result<Updated>>
// {
//     private readonly IIdentityService _identityService = identityService;
//     private readonly ICurrentUser _currentUser = currentUser;

//     public Task<Result<Updated>> Handle(
//         UpdateManageProfilePhoneCommand request,
//         CancellationToken cancellationToken
//     )
//     {
//         if (!_currentUser.IsAuthenticated)
//             return Task.FromResult<Result<Updated>>(
//                 Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
//             );

//         return _identityService.UpdateProfilePhoneNumberAsync(
//             _currentUser.Id,
//             request.PhoneNumber,
//             cancellationToken
//         );
//     }
// }

// public sealed record GetManageEmailQuery : IRequest<Result<ManageEmailDto>>;

// public sealed class GetManageEmailQueryHandler(
//     IIdentityService identityService,
//     ICurrentUser currentUser
// ) : IRequestHandler<GetManageEmailQuery, Result<ManageEmailDto>>
// {
//     private readonly IIdentityService _identityService = identityService;
//     private readonly ICurrentUser _currentUser = currentUser;

//     public Task<Result<ManageEmailDto>> Handle(
//         GetManageEmailQuery request,
//         CancellationToken cancellationToken
//     )
//     {
//         if (!_currentUser.IsAuthenticated)
//             return Task.FromResult<Result<ManageEmailDto>>(
//                 Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
//             );

//         return _identityService.GetEmailAsync(_currentUser.Id, cancellationToken);
//     }
// }

// public sealed record HasPasswordQuery : IRequest<Result<bool>>;

// public sealed class HasPasswordQueryHandler(
//     IIdentityService identityService,
//     ICurrentUser currentUser
// ) : IRequestHandler<HasPasswordQuery, Result<bool>>
// {
//     private readonly IIdentityService _identityService = identityService;
//     private readonly ICurrentUser _currentUser = currentUser;

//     public Task<Result<bool>> Handle(HasPasswordQuery request, CancellationToken cancellationToken)
//     {
//         if (!_currentUser.IsAuthenticated)
//             return Task.FromResult<Result<bool>>(
//                 Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
//             );

//         return _identityService.HasPasswordAsync(_currentUser.Id, cancellationToken);
//     }
// }

// public sealed record ChangePasswordCommand(string CurrentPassword, string NewPassword)
//     : IRequest<Result<Updated>>;

// public sealed class ChangePasswordCommandHandler(
//     IIdentityService identityService,
//     ICurrentUser currentUser
// ) : IRequestHandler<ChangePasswordCommand, Result<Updated>>
// {
//     private readonly IIdentityService _identityService = identityService;
//     private readonly ICurrentUser _currentUser = currentUser;

//     public Task<Result<Updated>> Handle(
//         ChangePasswordCommand request,
//         CancellationToken cancellationToken
//     )
//     {
//         if (!_currentUser.IsAuthenticated)
//             return Task.FromResult<Result<Updated>>(
//                 Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
//             );

//         return _identityService.ChangePasswordAsync(
//             _currentUser.Id,
//             request.CurrentPassword,
//             request.NewPassword,
//             cancellationToken
//         );
//     }
// }

// public sealed record SetPasswordCommand(string NewPassword) : IRequest<Result<Updated>>;

// public sealed class SetPasswordCommandHandler(
//     IIdentityService identityService,
//     ICurrentUser currentUser
// ) : IRequestHandler<SetPasswordCommand, Result<Updated>>
// {
//     private readonly IIdentityService _identityService = identityService;
//     private readonly ICurrentUser _currentUser = currentUser;

//     public Task<Result<Updated>> Handle(
//         SetPasswordCommand request,
//         CancellationToken cancellationToken
//     )
//     {
//         if (!_currentUser.IsAuthenticated)
//             return Task.FromResult<Result<Updated>>(
//                 Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
//             );

//         return _identityService.SetPasswordAsync(
//             _currentUser.Id,
//             request.NewPassword,
//             cancellationToken
//         );
//     }
// }

// public sealed record GetTwoFactorStatusQuery : IRequest<Result<TwoFactorStatusDto>>;

// public sealed class GetTwoFactorStatusQueryHandler(
//     IIdentityService identityService,
//     ICurrentUser currentUser
// ) : IRequestHandler<GetTwoFactorStatusQuery, Result<TwoFactorStatusDto>>
// {
//     private readonly IIdentityService _identityService = identityService;
//     private readonly ICurrentUser _currentUser = currentUser;

//     public Task<Result<TwoFactorStatusDto>> Handle(
//         GetTwoFactorStatusQuery request,
//         CancellationToken cancellationToken
//     )
//     {
//         if (!_currentUser.IsAuthenticated)
//             return Task.FromResult<Result<TwoFactorStatusDto>>(
//                 Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
//             );

//         return _identityService.GetTwoFactorStatusAsync(_currentUser.Id, cancellationToken);
//     }
// }

// public sealed record ForgetTwoFactorClientCommand : IRequest<Result<Updated>>;

// public sealed class ForgetTwoFactorClientCommandHandler(
//     IIdentityService identityService,
//     ICurrentUser currentUser
// ) : IRequestHandler<ForgetTwoFactorClientCommand, Result<Updated>>
// {
//     private readonly IIdentityService _identityService = identityService;
//     private readonly ICurrentUser _currentUser = currentUser;

//     public Task<Result<Updated>> Handle(
//         ForgetTwoFactorClientCommand request,
//         CancellationToken cancellationToken
//     )
//     {
//         if (!_currentUser.IsAuthenticated)
//             return Task.FromResult<Result<Updated>>(
//                 Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
//             );

//         return _identityService.ForgetTwoFactorClientAsync(_currentUser.Id, cancellationToken);
//     }
// }

// public sealed record GetAuthenticatorSetupQuery : IRequest<Result<AuthenticatorSetupDto>>;

// public sealed class GetAuthenticatorSetupQueryHandler(
//     IIdentityService identityService,
//     ICurrentUser currentUser
// ) : IRequestHandler<GetAuthenticatorSetupQuery, Result<AuthenticatorSetupDto>>
// {
//     private readonly IIdentityService _identityService = identityService;
//     private readonly ICurrentUser _currentUser = currentUser;

//     public Task<Result<AuthenticatorSetupDto>> Handle(
//         GetAuthenticatorSetupQuery request,
//         CancellationToken cancellationToken
//     )
//     {
//         if (!_currentUser.IsAuthenticated)
//             return Task.FromResult<Result<AuthenticatorSetupDto>>(
//                 Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
//             );

//         return _identityService.GetAuthenticatorSetupAsync(_currentUser.Id, cancellationToken);
//     }
// }

// public sealed record EnableTwoFactorCommand(string VerificationCode)
//     : IRequest<Result<IReadOnlyList<string>>>;

// public sealed class EnableTwoFactorCommandHandler(
//     IIdentityService identityService,
//     ICurrentUser currentUser
// ) : IRequestHandler<EnableTwoFactorCommand, Result<IReadOnlyList<string>>>
// {
//     private readonly IIdentityService _identityService = identityService;
//     private readonly ICurrentUser _currentUser = currentUser;

//     public Task<Result<IReadOnlyList<string>>> Handle(
//         EnableTwoFactorCommand request,
//         CancellationToken cancellationToken
//     )
//     {
//         if (!_currentUser.IsAuthenticated)
//             return Task.FromResult<Result<IReadOnlyList<string>>>(
//                 Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
//             );

//         return _identityService.EnableTwoFactorAsync(
//             _currentUser.Id,
//             request.VerificationCode,
//             cancellationToken
//         );
//     }
// }

// public sealed record DisableTwoFactorCommand : IRequest<Result<Updated>>;

// public sealed class DisableTwoFactorCommandHandler(
//     IIdentityService identityService,
//     ICurrentUser currentUser
// ) : IRequestHandler<DisableTwoFactorCommand, Result<Updated>>
// {
//     private readonly IIdentityService _identityService = identityService;
//     private readonly ICurrentUser _currentUser = currentUser;

//     public Task<Result<Updated>> Handle(
//         DisableTwoFactorCommand request,
//         CancellationToken cancellationToken
//     )
//     {
//         if (!_currentUser.IsAuthenticated)
//             return Task.FromResult<Result<Updated>>(
//                 Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
//             );

//         return _identityService.DisableTwoFactorAsync(_currentUser.Id, cancellationToken);
//     }
// }

// public sealed record GenerateRecoveryCodesCommand(int Number = 10)
//     : IRequest<Result<IReadOnlyList<string>>>;

// public sealed class GenerateRecoveryCodesCommandHandler(
//     IIdentityService identityService,
//     ICurrentUser currentUser
// ) : IRequestHandler<GenerateRecoveryCodesCommand, Result<IReadOnlyList<string>>>
// {
//     private readonly IIdentityService _identityService = identityService;
//     private readonly ICurrentUser _currentUser = currentUser;

//     public Task<Result<IReadOnlyList<string>>> Handle(
//         GenerateRecoveryCodesCommand request,
//         CancellationToken cancellationToken
//     )
//     {
//         if (!_currentUser.IsAuthenticated)
//             return Task.FromResult<Result<IReadOnlyList<string>>>(
//                 Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
//             );

//         return _identityService.GenerateRecoveryCodesAsync(
//             _currentUser.Id,
//             request.Number,
//             cancellationToken
//         );
//     }
// }

// public sealed record ResetAuthenticatorCommand : IRequest<Result<Updated>>;

// public sealed class ResetAuthenticatorCommandHandler(
//     IIdentityService identityService,
//     ICurrentUser currentUser
// ) : IRequestHandler<ResetAuthenticatorCommand, Result<Updated>>
// {
//     private readonly IIdentityService _identityService = identityService;
//     private readonly ICurrentUser _currentUser = currentUser;

//     public Task<Result<Updated>> Handle(
//         ResetAuthenticatorCommand request,
//         CancellationToken cancellationToken
//     )
//     {
//         if (!_currentUser.IsAuthenticated)
//             return Task.FromResult<Result<Updated>>(
//                 Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
//             );

//         return _identityService.ResetAuthenticatorAsync(_currentUser.Id, cancellationToken);
//     }
// }

// public sealed record DeleteAccountCommand(string? Password) : IRequest<Result<Deleted>>;

// public sealed class DeleteAccountCommandHandler(
//     IIdentityService identityService,
//     ICurrentUser currentUser
// ) : IRequestHandler<DeleteAccountCommand, Result<Deleted>>
// {
//     private readonly IIdentityService _identityService = identityService;
//     private readonly ICurrentUser _currentUser = currentUser;

//     public Task<Result<Deleted>> Handle(
//         DeleteAccountCommand request,
//         CancellationToken cancellationToken
//     )
//     {
//         if (!_currentUser.IsAuthenticated)
//             return Task.FromResult<Result<Deleted>>(
//                 Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
//             );

//         return _identityService.DeleteAccountAsync(
//             _currentUser.Id,
//             request.Password,
//             cancellationToken
//         );
//     }
// }

// public sealed record DownloadPersonalDataQuery : IRequest<Result<PersonalDataFileDto>>;

// public sealed class DownloadPersonalDataQueryHandler(
//     IIdentityService identityService,
//     ICurrentUser currentUser
// ) : IRequestHandler<DownloadPersonalDataQuery, Result<PersonalDataFileDto>>
// {
//     private readonly IIdentityService _identityService = identityService;
//     private readonly ICurrentUser _currentUser = currentUser;

//     public Task<Result<PersonalDataFileDto>> Handle(
//         DownloadPersonalDataQuery request,
//         CancellationToken cancellationToken
//     )
//     {
//         if (!_currentUser.IsAuthenticated)
//             return Task.FromResult<Result<PersonalDataFileDto>>(
//                 Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
//             );

//         return _identityService.DownloadPersonalDataAsync(_currentUser.Id, cancellationToken);
//     }
// }

// public sealed record GetManageExternalLoginsQuery : IRequest<Result<ExternalLoginsDto>>;

// public sealed class GetManageExternalLoginsQueryHandler(
//     IIdentityService identityService,
//     ICurrentUser currentUser
// ) : IRequestHandler<GetManageExternalLoginsQuery, Result<ExternalLoginsDto>>
// {
//     private readonly IIdentityService _identityService = identityService;
//     private readonly ICurrentUser _currentUser = currentUser;

//     public Task<Result<ExternalLoginsDto>> Handle(
//         GetManageExternalLoginsQuery request,
//         CancellationToken cancellationToken
//     )
//     {
//         if (!_currentUser.IsAuthenticated)
//             return Task.FromResult<Result<ExternalLoginsDto>>(
//                 Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
//             );

//         return _identityService.GetExternalLoginsAsync(_currentUser.Id, cancellationToken);
//     }
// }

// public sealed record RemoveManageExternalLoginCommand(string LoginProvider, string ProviderKey)
//     : IRequest<Result<Updated>>;

// public sealed class RemoveManageExternalLoginCommandHandler(
//     IIdentityService identityService,
//     ICurrentUser currentUser
// ) : IRequestHandler<RemoveManageExternalLoginCommand, Result<Updated>>
// {
//     private readonly IIdentityService _identityService = identityService;
//     private readonly ICurrentUser _currentUser = currentUser;

//     public Task<Result<Updated>> Handle(
//         RemoveManageExternalLoginCommand request,
//         CancellationToken cancellationToken
//     )
//     {
//         if (!_currentUser.IsAuthenticated)
//             return Task.FromResult<Result<Updated>>(
//                 Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
//             );

//         return _identityService.RemoveExternalLoginAsync(
//             _currentUser.Id,
//             request.LoginProvider,
//             request.ProviderKey,
//             cancellationToken
//         );
//     }
// }

// public sealed record AddManageExternalLoginCommand : IRequest<Result<Updated>>;

// public sealed class AddManageExternalLoginCommandHandler(
//     IIdentityService identityService,
//     ICurrentUser currentUser
// ) : IRequestHandler<AddManageExternalLoginCommand, Result<Updated>>
// {
//     private readonly IIdentityService _identityService = identityService;
//     private readonly ICurrentUser _currentUser = currentUser;

//     public Task<Result<Updated>> Handle(
//         AddManageExternalLoginCommand request,
//         CancellationToken cancellationToken
//     )
//     {
//         if (!_currentUser.IsAuthenticated)
//             return Task.FromResult<Result<Updated>>(
//                 Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
//             );

//         return _identityService.AddExternalLoginAsync(_currentUser.Id, cancellationToken);
//     }
// }
