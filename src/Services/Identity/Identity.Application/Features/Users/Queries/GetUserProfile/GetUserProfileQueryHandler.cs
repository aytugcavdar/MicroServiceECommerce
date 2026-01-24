using AutoMapper;
using BuildingBlocks.CrossCutting.Exceptions.types;
using Identity.Application.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Identity.Application.Features.Users.Queries.GetUserProfile;

public class GetUserProfileQueryHandler
    : IRequestHandler<GetUserProfileQuery, GetUserProfileResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetUserProfileQueryHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _mapper = mapper;
        _logger = Log.ForContext<GetUserProfileQueryHandler>();
    }

    public async Task<GetUserProfileResponse> Handle(
        GetUserProfileQuery request,
        CancellationToken cancellationToken)
    {
        _logger.Information(
            "👤 Getting profile for user: {UserId}",
            request.UserId);

        // 1. Kullanıcıyı roller ile birlikte getir
        var user = await _userRepository.Query()
            .Include(u => u.UserOperationClaims)
                .ThenInclude(uoc => uoc.OperationClaim)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            _logger.Warning("⚠️ User not found: {UserId}", request.UserId);
            throw new NotFoundException("User", request.UserId);
        }
        var response = _mapper.Map<GetUserProfileResponse>(user);

        response.Roles = user.UserOperationClaims
            .Select(uoc => uoc.OperationClaim.Name)
            .ToList();

        var activeSessions = await _refreshTokenRepository
            .GetActiveTokensByUserIdAsync(user.Id, cancellationToken);
        response.ActiveSessionCount = activeSessions.Count;

        response.LastLoginDate = user.RefreshTokens
            .OrderByDescending(rt => rt.CreatedDate)
            .FirstOrDefault()?.CreatedDate;

        _logger.Information(
            "✅ Profile retrieved successfully for user: {UserId}",
            request.UserId);

        return response;
    }
}