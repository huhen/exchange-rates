using CSharpFunctionalExtensions;
// using IdentityService.Core.Services;

namespace IdentityService.UnitTests.Core.Services;

// public class UpdateUserPaswwordSevice_UpdateUserPassword
// {
//     private readonly IRepository<User> _repository = Substitute.For<IRepository<User>>();
//     private readonly IMediator _mediator = Substitute.For<IMediator>();
//     private readonly ILogger<UpdateUserPasswordService> _logger = Substitute.For<ILogger<UpdateUserPasswordService>>();
//
//     private readonly UpdateUserPasswordService _service;
//
//     public UpdateUserPaswwordSevice_UpdateUserPassword()
//     {
//         _service = new UpdateUserPasswordService();
//     }
//
//     [Fact]
//     public async Task ReturnsNotFoundGivenCantFindContributor()
//     {
//         var missingId = Guid.NewGuid();
//         var result = await _service.UpdateUserPassword(UserId.From(missingId));
//
//         result.ShouldBe(Result.Failure("NotFound"));
//     }
// }
