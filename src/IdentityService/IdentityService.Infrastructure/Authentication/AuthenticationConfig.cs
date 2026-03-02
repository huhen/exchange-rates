using System.Security.Cryptography.X509Certificates;
using System.Text;
using IdentityService.Infrastructure.Authorization;
using IdentityService.UseCases.Abstractions.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Infrastructure.Authentication;

public static class AuthenticationConfig
{
    /// <summary>
    /// Registers JWT-based authentication and the application's authorization services into the provided service collection.
    /// </summary>
    /// <returns>The same <see cref="IServiceCollection"/> with authentication and authorization services configured.</returns>
    internal static IServiceCollection AddAuthenticationConfig(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddJwtAuthentication(configuration).AddAuthorizationInternal();
    }

    /// <summary>
    /// Configures JWT bearer authentication, OpenID Connect metadata, and related services and providers in the dependency injection container.
    /// </summary>
    /// <returns>The updated <see cref="IServiceCollection"/> with JWT authentication, OpenID configuration, and related service registrations applied.</returns>
    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtTokenOptions>(configuration.GetSection(JwtTokenOptions.SectionName));

        var jwtTokenOptions = new JwtTokenOptions();
        configuration
            .GetSection(JwtTokenOptions.SectionName)
            .Bind(jwtTokenOptions);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
            {
                o.RequireHttpsMetadata = jwtTokenOptions.RequireHttpsMetadata;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtTokenOptions.Issuer,
                    ValidAudience = jwtTokenOptions.Audience,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true
                };
                o.ConfigurationManager =
                    new ConfigurationManager<OpenIdConnectConfiguration>(
                        jwtTokenOptions.OpenIdConfigurationUri,
                        new OpenIdConnectConfigurationRetriever(),
                        new HttpDocumentRetriever { RequireHttps = jwtTokenOptions.RequireHttpsMetadata });
            });

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ITokenProvider, TokenProvider>();

        services.AddSingleton<OpenIdConfigurationResponse>(provider => new OpenIdConfigurationResponse
        {
            Issuer = jwtTokenOptions.Issuer,
            JwksUri = jwtTokenOptions.JwksUri,
            SubjectTypesSupported = ["public"],
            ResponseTypesSupported = ["token"],
            ClaimsSupported = ["sub", "aud", "exp", "nbf", "iat", "iss", "act"],
            IdTokenSigningAlgValuesSupported = ["ES256"],
            ScopesSupported = ["openid"]
        });

        services.AddJwksResponse(jwtTokenOptions);

        return services;
    }

    /// <summary>
    /// Registers authorization infrastructure and permission-based authorization services.
    /// </summary>
    /// <returns>The original <see cref="IServiceCollection"/> with authorization services and related handlers registered.</returns>
    private static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }

    /// <summary>
    /// Registers a JWKS (JSON Web Key Set) response singleton derived from the EC public key of the configured certificate.
    /// </summary>
    /// <param name="services">The service collection to register the JWKS response into.</param>
    /// <param name="jwtTokenOptions">JWT token options containing the certificate path and password used to build the JWKS entry.</param>
    /// <returns>The same <see cref="IServiceCollection"/> to allow fluent chaining.</returns>
    private static IServiceCollection AddJwksResponse(this IServiceCollection services, JwtTokenOptions jwtTokenOptions)
    {
        if (string.IsNullOrWhiteSpace(jwtTokenOptions.CertificatePath))
            throw new InvalidOperationException("JWT certificate path is not configured.");

        using var certificate = new X509Certificate2(
            jwtTokenOptions.CertificatePath,
            jwtTokenOptions.CertificatePassword);

        var thumbprint = certificate.Thumbprint;

        using var ecdsa = certificate.GetECDsaPublicKey()
                          ?? throw new InvalidOperationException("JWT certificate must contain an ECDSA public key.");

        var parameters = ecdsa.ExportParameters(false);

        var x = Base64UrlEncoder.Encode(parameters.Q.X);
        var y = Base64UrlEncoder.Encode(parameters.Q.Y);

        services.AddSingleton<JwksResponse>(provider => new JwksResponse
        {
            Keys =
            [
                new JwksKeyDto
                {
                    Kty = "EC",
                    Alg = "ES256",
                    Crv = "P-256",
                    Use = "sig",
                    Kid = thumbprint,
                    X = x,
                    Y = y
                }
            ]
        });
        return services;
    }

    /// <summary>
    /// Enables authentication and authorization middleware on the application and exposes OpenID Connect metadata endpoints.
    /// </summary>
    /// <returns>The same <see cref="WebApplication"/> instance with authentication/authorization enabled and OpenID configuration and JWKS GET endpoints mapped (excluded from API descriptions).</returns>
    public static WebApplication MapAuthentications(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGet(JwtTokenOptions.OpenIdConfigurationEndpoint, OpenIdConfigurationEndpoint)
            .ExcludeFromDescription();
        app.MapGet(JwtTokenOptions.JwksEndpoint, JwksEndpoint)
            .ExcludeFromDescription();

        return app;
    }

    /// <summary>
    /// Returns the provided OpenID Connect discovery metadata as an HTTP 200 result.
    /// </summary>
    /// <param name="metadata">The OpenID Connect configuration metadata to return.</param>
    /// <returns>An OK result containing the given <see cref="OpenIdConfigurationResponse"/>.</returns>
    private static Ok<OpenIdConfigurationResponse> OpenIdConfigurationEndpoint(OpenIdConfigurationResponse metadata)
    {
        return TypedResults.Ok(metadata);
    }

    /// <summary>
    /// Returns the provided JWKS (JSON Web Key Set) response as an HTTP 200 OK result.
    /// </summary>
    /// <param name="metadata">The JWKS payload to return to clients.</param>
    /// <returns>An OK result containing the given <see cref="JwksResponse"/>.</returns>
    private static Ok<JwksResponse> JwksEndpoint(JwksResponse metadata)
    {
        return TypedResults.Ok(metadata);
    }
}
