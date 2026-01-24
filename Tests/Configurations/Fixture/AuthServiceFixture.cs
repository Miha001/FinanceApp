using Finances.Application.Abstractions.Services;
using Finances.Application.Abstractions.Shared;
using Finances.Application.Abstractions.Validators;
using Finances.DAL.Implementations.Users;
using Finances.Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using Moq;
using Serilog;

namespace Tests.Configurations.Fixture;
public class AuthServiceFixture : IDisposable
{
    public Mock<IMediator> MediatorMock { get; }
    public Mock<ITokenService> TokenServiceMock { get; }

    public Mock<IStateSaveChanges> UnitOfWorkMock { get; }

    public Mock<IAuthValidator> AuthValidatorMock { get; }

    public Mock<ICacheService> CacheServiceMock { get; }

    public Mock<ILogger> LoggerMock { get; }

    public JwtSettings JwtSettings { get; }

    public IAuthService AuthService { get; }

    public AuthServiceFixture()
    {
        MediatorMock = new Mock<IMediator>();
        TokenServiceMock = new Mock<ITokenService>();
        UnitOfWorkMock = new Mock<IStateSaveChanges>();
        AuthValidatorMock = new Mock<IAuthValidator>();
        CacheServiceMock = new Mock<ICacheService>();
        LoggerMock = new Mock<ILogger>();

        JwtSettings = new JwtSettings
        {
            Issuer = "test",
            Audience = "test",
            Authority = "test",
            Key = "test",
            ExpirationInMinutes = 60
        };

        var jwtOptions = Options.Create(JwtSettings);

        AuthService = new AuthService(
            MediatorMock.Object,
            TokenServiceMock.Object,
            UnitOfWorkMock.Object,
            AuthValidatorMock.Object,
            CacheServiceMock.Object,
            LoggerMock.Object,
            jwtOptions);
    }

    public void Dispose() { }
}