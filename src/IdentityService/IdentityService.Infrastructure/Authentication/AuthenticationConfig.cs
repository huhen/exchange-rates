using System.Text;
using IdentityService.UseCases.Abstractions.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    private const string JwksEndpoin = "/.well-known/jwks";
    private const string OpenIdEndpoin = "/.well-known/openid-configuration";

    internal static IServiceCollection AddAuthenticationConfig(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddJwtAuthentication(configuration).AddAuthorizationInternal();
    }

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
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtTokenOptions.Issuer,
                    ValidAudience = jwtTokenOptions.Audience,
                    ClockSkew = TimeSpan.Zero
                };
                o.ConfigurationManager =
                    new ConfigurationManager<OpenIdConnectConfiguration>(
                        jwtTokenOptions.JwksEndpoint,
                        new OpenIdConnectConfigurationRetriever(),
                        new HttpDocumentRetriever { RequireHttps = false });
            });

        services.AddHttpContextAccessor();
        // services.AddScoped<IUserContext, UserContext>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ITokenProvider, TokenProvider>();

        services.AddSingleton<OpenIdConfigurationResponse>(provider => new OpenIdConfigurationResponse
        {
            Issuer = jwtTokenOptions.Issuer,
            JwksUri = jwtTokenOptions.Issuer + JwksEndpoin,
            SubjectTypesSupported = ["public"],
            ResponseTypesSupported = ["token"],
            ClaimsSupported = [], // ["sub", "aud", "exp", "nbf", "iat", "iss", "act"],
            IdTokenSigningAlgValuesSupported = ["ES256"],
            ScopesSupported = [] // ["openid"]
        });
        return services;
    }

    private static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        services.AddAuthorization();

        //     services.AddScoped<PermissionProvider>();
        //
        //     services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        //
        //     services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }

    public static WebApplication MapAuthentications(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGet(OpenIdEndpoin, OpenIdConfigurationEndpoint);

        return app;
    }

    private static Ok<OpenIdConfigurationResponse> OpenIdConfigurationEndpoint(OpenIdConfigurationResponse metadata)
    {
        return TypedResults.Ok(metadata);
    }

//{"issuer":"https://github.com","jwks_uri":"https://github.com/login/oauth/.well-known/jwks","subject_types_supported":["public"],"response_types_supported":["code","id_token"],"claims_supported":["sub","aud","exp","nbf","iat","iss","act"],"id_token_signing_alg_values_supported":["RS256"],"scopes_supported":["openid"]}

    //app.MapGet("/.well-known/jwks.json", (IConfiguration config) =>
    // {
    //     var cert = new X509Certificate2(
    //         config["Jwt:CertificatePath"],
    //         config["Jwt:CertificatePassword"]);
    // 
    //     var ecdsa = cert.GetECDsaPublicKey();
    //     var parameters = ecdsa.ExportParameters(false);
    // 
    //     var jwk = new
    //     {
    //         kty = "EC",
    //         use = "sig",
    //         crv = "P-256",
    //         kid = cert.Thumbprint,
    //         x = Base64UrlEncoder.Encode(parameters.Q.X),
    //         y = Base64UrlEncoder.Encode(parameters.Q.Y),
    //         alg = "ES256"
    //     };
    // 
    //     return Results.Json(new { keys = new[] { jwk } });
    // });

    //     var metadata = new
    // {
    //     issuer = issuer,
    //     jwks_uri = issuer + jwksPath,

    //     // минимально допустимые поля для совместимости
    //     id_token_signing_alg_values_supported = new[] { "ES256" },
    //     subject_types_supported = new[] { "public" },
    //     response_types_supported = new[] { "token" }
    // };
}
