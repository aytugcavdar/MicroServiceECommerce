using BuildingBlocks.Core.Security.Constants;
using BuildingBlocks.CrossCutting.Exceptions.types;
using BuildingBlocks.Security.Encryption;
using Identity.Application.Features.Auth.Register.Rules;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Domain.Events;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Auth.Register.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterCommandResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly RegisterBusinessRules _registerBusinessRules;
    private readonly ILogger _logger;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        RegisterBusinessRules authBusinessRules)
    {
        _userRepository = userRepository;
        _registerBusinessRules = authBusinessRules;
        // Loglama için context belirtiyoruz, böylece loglarda hangi sınıftan geldiği belli olur.
        _logger = Log.ForContext<RegisterCommandHandler>();
    }

    public async Task<RegisterCommandResponse> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        _logger.Information("📝 Yeni üyelik talebi: {Email}", request.Email);

        // ============================================
        // 1. İŞ KURALLARI (Validasyon)
        // ============================================
        await _registerBusinessRules.EmailShouldNotExistWhenRegistering(request.Email);
        await _registerBusinessRules.UserNameShouldNotExistWhenRegistering(request.UserName);

        var defaultRole = await _registerBusinessRules.DefaultUserRoleShouldExist(GeneralOperationClaims.User);

        // ============================================
        // 2. ŞİFRE HASHLEME
        // ============================================
        HashingHelper.CreatePasswordHash(
            request.Password,
            out byte[] passwordHash,
            out byte[] passwordSalt);

        // ============================================
        // 3. KULLANICI OLUŞTURMA (Critical Point!)
        // ============================================
        // NOT: User.Create metodu içinde "UserCreatedDomainEvent" oluşturulur.
        // Bu event, mail gönderme emrini taşıyan gizli tetikleyicidir.
        User user = User.Create(
            firstName: request.FirstName,
            lastName: request.LastName,
            email: request.Email,
            userName: request.UserName,
            passwordSalt: passwordSalt,
            passwordHash: passwordHash,
            registrationIp: request.IpAddress);

        // ============================================
        // 4. VARSAYILAN ROL ATAMA
        // ============================================
        user.UserOperationClaims.Add(new UserOperationClaim(user.Id, defaultRole.Id));

        _logger.Debug("👤 Kullanıcıya varsayılan rol atandı: {RoleName}", GeneralOperationClaims.User);

        // ============================================
        // 5. VERİTABANINA KAYIT (Outbox Tetiklenir)
        // ============================================
        // AddAsync çağrıldığında, User entity'si içindeki Domain Event de 
        // "OutboxMessages" tablosuna kaydedilir. 
        // Program.cs'deki işlemci bu mesajı okuyup maili o zaman gönderir.
        await _userRepository.AddAsync(user);

        _logger.Information("✅ Kullanıcı başarıyla oluşturuldu: {UserId}", user.Id);

        // ============================================
        // 6. CEVAP DÖNME
        // ============================================
        return new RegisterCommandResponse
        {
            UserId = user.Id,
            Email = user.Email,
            Message = "Kayıt başarılı. Lütfen hesabınızı doğrulamak için e-postanızı kontrol edin."
        };
    }
}