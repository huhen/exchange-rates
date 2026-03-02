using IdentityService.Core.UserAggregate;
using Vogen;

namespace IdentityService.Infrastructure.Data.Config;

[EfCoreConverter<UserId>]
[EfCoreConverter<UserName>]
[EfCoreConverter<UserPasswordHash>]
internal partial class VogenEfCoreConverters;
