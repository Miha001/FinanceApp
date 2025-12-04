using Application.Abstractions;
using Application.Abstractions.Services;
using Application.Abstractions.Validators;
using Domain.Settings;
using Infrastructure.Implementations.Services;
using MediatR;
using Microsoft.Extensions.Options;
using Moq;
using Serilog;

namespace Tests.Configurations.Fixture;
public class AuthServiceFixture : IDisposable
{
    public Mock<IMediator> MediatorMock { get; }
    public Mock<ITokenService> TokenServiceMock { get; }

    public Mock<IUnitOfWork> UnitOfWorkMock { get; }

    public Mock<IAuthValidator> AuthValidatorMock { get; }

    public Mock<ICacheService> CacheServiceMock { get; }

    public Mock<ILogger> LoggerMock { get; }

    public JwtSettings JwtSettings { get; }

    public IAuthService AuthService { get; }


    public AuthServiceFixture()
    {
        MediatorMock = new Mock<IMediator>();
        TokenServiceMock = new Mock<ITokenService>();
        UnitOfWorkMock = new Mock<IUnitOfWork>();
        AuthValidatorMock = new Mock<IAuthValidator>();
        CacheServiceMock = new Mock<ICacheService>();
        LoggerMock = new Mock<ILogger>();

        JwtSettings = new JwtSettings
        {
            Issuer = "test",
            Audience = "test",
            Authority = "test",
            Key = "test",
            RefreshTokenValidityInDays = 11
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