using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
using ApiScope = Volo.Abp.IdentityServer.ApiScopes.ApiScope;
using Client = Volo.Abp.IdentityServer.Clients.Client;

namespace Aurora.IdentityServer
{
    public class IdentityServerDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IConfiguration _configuration;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IClientRepository _clientRepository;
        private readonly IApiScopeRepository _apiScopeRepository;
        private readonly IPermissionDataSeeder _permissionDataSeeder;
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IIdentityResourceDataSeeder _identityResourceDataSeeder;

        public IdentityServerDataSeedContributor(IConfiguration configuration, ICurrentTenant currentTenant, IGuidGenerator guidGenerator, IClientRepository clientRepository, IApiScopeRepository apiScopeRepository, IPermissionDataSeeder permissionDataSeeder, IApiResourceRepository apiResourceRepository, IIdentityResourceDataSeeder identityResourceDataSeeder)
        {
            _configuration = configuration;
            _currentTenant = currentTenant;
            _guidGenerator = guidGenerator;
            _clientRepository = clientRepository;
            _apiScopeRepository = apiScopeRepository;
            _permissionDataSeeder = permissionDataSeeder;
            _apiResourceRepository = apiResourceRepository;
            _identityResourceDataSeeder = identityResourceDataSeeder;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context?.TenantId))
            {
                await _identityResourceDataSeeder.CreateStandardResourcesAsync();
                await CreateApiResourcesAsync();
                await CreateApiScopesAsync();
                await CreateClientsAsync();
            }
        }

        private async Task CreateApiScopesAsync()
        {
            await CreateApiScopeAsync("aurora-api");
            await CreateApiScopeAsync("aurora-host-api");
        }

        private async Task CreateApiResourcesAsync()
        {
            var commonApiUserClaims = new[] {"role"};
            await CreateApiResourceAsync("aurora-api", commonApiUserClaims);
            await CreateApiResourceAsync("aurora-host-api", commonApiUserClaims);
        }

        private async Task<ApiResource> CreateApiResourceAsync(string name, IEnumerable<string> claims)
        {
            var apiResource = await _apiResourceRepository.FindByNameAsync(name);
            if (apiResource == null)
            {
                apiResource = await _apiResourceRepository.InsertAsync(new ApiResource(_guidGenerator.Create(), name, name), true);
            }

            foreach (var claim in claims)
            {
                if (apiResource.FindClaim(claim) == null)
                {
                    apiResource.AddUserClaim(claim);
                }
            }

            return await _apiResourceRepository.UpdateAsync(apiResource);
        }

        private async Task<ApiScope> CreateApiScopeAsync(string name)
        {
            var apiScope = await _apiScopeRepository.GetByNameAsync(name);
            if (apiScope == null)
            {
                apiScope = await _apiScopeRepository.InsertAsync(new ApiScope(_guidGenerator.Create(), name, name), true);
            }

            return apiScope;
        }

        private async Task CreateClientsAsync()
        {
            var commonScopes = new[] {"openid", "profile"};

            var configurationSection = _configuration.GetSection("IdentityServer:Clients");

            var auroraWebClientId = configurationSection["Aurora_Web:ClientId"];
            var auroraWebClientSecret = configurationSection["Aurora_Web:ClientSecret"];
            if (!auroraWebClientId.IsNullOrWhiteSpace() && !auroraWebClientSecret.IsNullOrWhiteSpace())
            {
                var scopes = new List<string>(commonScopes) {"aurora-api"};
                await CreateClientAsync(auroraWebClientId, "aurora web", scopes, GrantTypes.ResourceOwnerPassword, auroraWebClientSecret.Sha256());
            }

            var auroraHostWebClientId = configurationSection["Aurora_Host_Web:ClientId"];
            var auroraHostWebClientSecret = configurationSection["Aurora_Host_Web:ClientSecret"];
            if (!auroraHostWebClientId.IsNullOrWhiteSpace() && !auroraHostWebClientSecret.IsNullOrWhiteSpace())
            {
                var scopes = new List<string>(commonScopes) {"aurora-host-api"};
                await CreateClientAsync(auroraHostWebClientId, "aurora host web", scopes, GrantTypes.ResourceOwnerPassword, auroraHostWebClientSecret.Sha256());
            }
        }

        private async Task<Client> CreateClientAsync(string id, string name, IEnumerable<string> scopes, IEnumerable<string> grantTypes, string secret = null, string redirectUri = null, string postLogoutRedirectUri = null, string frontChannelLogoutUri = null, bool requireClientSecret = true, bool requirePkce = false, IEnumerable<string> permissions = null, IEnumerable<string> corsOrigins = null)
        {
            var client = await _clientRepository.FindByClientIdAsync(id);
            if (client == null)
            {
                client = await _clientRepository.InsertAsync(new Client(_guidGenerator.Create(), id)
                {
                    ClientName = name,
                    ProtocolType = "oidc",
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true,
                    AbsoluteRefreshTokenLifetime = 86400 * 3,
                    AccessTokenLifetime = 86400,
                    AuthorizationCodeLifetime = 300,
                    IdentityTokenLifetime = 300,
                    RequireConsent = false,
                    FrontChannelLogoutUri = frontChannelLogoutUri,
                    RequireClientSecret = requireClientSecret,
                    RequirePkce = requirePkce
                }, true);
            }

            foreach (var scope in scopes)
            {
                if (client.FindScope(scope) == null)
                {
                    client.AddScope(scope);
                }
            }

            foreach (var grantType in grantTypes)
            {
                if (client.FindGrantType(grantType) == null)
                {
                    client.AddGrantType(grantType);
                }
            }

            if (!secret.IsNullOrEmpty())
            {
                if (client.FindSecret(secret) == null)
                {
                    client.AddSecret(secret);
                }
            }

            if (redirectUri != null)
            {
                if (client.FindRedirectUri(redirectUri) == null)
                {
                    client.AddRedirectUri(redirectUri);
                }
            }

            if (postLogoutRedirectUri != null)
            {
                if (client.FindPostLogoutRedirectUri(postLogoutRedirectUri) == null)
                {
                    client.AddPostLogoutRedirectUri(postLogoutRedirectUri);
                }
            }

            if (permissions != null)
            {
                await _permissionDataSeeder.SeedAsync(ClientPermissionValueProvider.ProviderName, id, permissions);
            }

            if (corsOrigins != null)
            {
                foreach (var origin in corsOrigins)
                {
                    if (!origin.IsNullOrWhiteSpace() && client.FindCorsOrigin(origin) == null)
                    {
                        client.AddCorsOrigin(origin);
                    }
                }
            }

            return await _clientRepository.UpdateAsync(client);
        }
    }
}