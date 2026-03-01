namespace IdentityService.Api.Endpoints;

public interface IEndpoint
{
    /// <summary>
/// Registers this endpoint's routes on the provided route builder.
/// </summary>
/// <param name="app">The route builder to which this endpoint should add its route mappings.</param>
void MapEndpoint(IEndpointRouteBuilder app);
}
