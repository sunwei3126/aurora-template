using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.IdentityResources;
using IdentityResource = IdentityServer4.Models.IdentityResource;

namespace Aurora.IdentityServer
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IIdentityResourceDataSeeder))]
    public class AuroraIdentityResourceDataSeeder : IdentityResourceDataSeeder
    {
        public AuroraIdentityResourceDataSeeder(IIdentityResourceRepository identityResourceRepository, IGuidGenerator guidGenerator, IIdentityClaimTypeRepository claimTypeRepository) : base(identityResourceRepository, guidGenerator, claimTypeRepository)
        {
        }

        public override async Task CreateStandardResourcesAsync()
        {
            var resources = new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile
                {
                    UserClaims = new[]
                    {
                        JwtClaimTypes.Name
                    }
                }
            };

            foreach (var resource in resources)
            {
                foreach (var claimType in resource.UserClaims)
                {
                    await AddClaimTypeIfNotExistsAsync(claimType);
                }

                await AddIdentityResourceIfNotExistsAsync(resource);
            }
        }
    }
}