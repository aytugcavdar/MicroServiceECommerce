using BuildingBlocks.CrossCutting.Exceptions.types;
using Identity.Application.Services;
using MediatR;

namespace Identity.Application.Features.Auth.ConfirmEmail.Commands;

public class ConfirmEmailCommandHandler
    : IRequestHandler<ConfirmEmailCommand, ConfirmEmailCommandResponse>
{
    private readonly IUserRepository _userRepository;

    public ConfirmEmailCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ConfirmEmailCommandResponse> Handle(
        ConfirmEmailCommand request,
        CancellationToken cancellationToken)
    {
        // Kullanıcıyı bul
        var user = await _userRepository.GetAsync(
            u => u.Email == request.Email,
            cancellationToken: cancellationToken);

        if (user == null)
            throw new BusinessException("User not found");

        // Zaten doğrulanmış mı?
        if (user.IsEmailConfirmed)
            throw new BusinessException("Email already confirmed");

        // Token doğru mu?
        if (user.EmailConfirmationToken != request.Token)
            throw new BusinessException("Invalid confirmation token");

        // Token süresi dolmuş mu? (Eğer expiration eklediyseniz)
        // if (user.EmailConfirmationTokenExpiration.HasValue && 
        //     user.EmailConfirmationTokenExpiration < DateTime.UtcNow)
        //     throw new BusinessException("Confirmation token has expired. Please request a new one.");

        // Email'i onayla
        user.IsEmailConfirmed = true;
        user.EmailConfirmationToken = null;
        // user.EmailConfirmationTokenExpiration = null;

        await _userRepository.UpdateAsync(user);

        return new ConfirmEmailCommandResponse
        {
            Success = true,
            Message = "Email confirmed successfully. You can now log in."
        };
    }
}
