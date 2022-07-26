﻿using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
using ApiScope = Volo.Abp.IdentityServer.ApiScopes.ApiScope;
using Client = Volo.Abp.IdentityServer.Clients.Client;

namespace ECAS.Shared.DbMigrator;

public class IdentityServerDataSeeder : ITransientDependency
{
    private readonly IApiResourceRepository _apiResourceRepository;
    private readonly IApiScopeRepository _apiScopeRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IConfiguration _configuration;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IIdentityResourceDataSeeder _identityResourceDataSeeder;
    private readonly IPermissionDataSeeder _permissionDataSeeder;

    public IdentityServerDataSeeder(IClientRepository clientRepository, IApiResourceRepository apiResourceRepository, IApiScopeRepository apiScopeRepository, IIdentityResourceDataSeeder identityResourceDataSeeder,
        IGuidGenerator guidGenerator, IPermissionDataSeeder permissionDataSeeder, IConfiguration configuration)
    {
        _clientRepository = clientRepository;
        _apiResourceRepository = apiResourceRepository;
        _apiScopeRepository = apiScopeRepository;
        _identityResourceDataSeeder = identityResourceDataSeeder;
        _guidGenerator = guidGenerator;
        _permissionDataSeeder = permissionDataSeeder;
        _configuration = configuration;
    }

    [UnitOfWork]
    public virtual async Task SeedAsync()
    {
        await _identityResourceDataSeeder.CreateStandardResourcesAsync();
        await CreateApiResourcesAsync();
        await CreateApiScopesAsync();
        await CreateClientsAsync();
    }

    private async Task CreateClientsAsync()
    {
        List<ServiceClient>? clients = _configuration.GetSection("Clients").Get<List<ServiceClient>>();
        string[]? commonScopes = new[] { "email", "openid", "profile", "role", "phone", "address" };
        foreach (ServiceClient? client in clients)
        {
            _ = await CreateClientAsync(client.ClientId, commonScopes.Union(client.Scopes), client.GrantTypes, client.ClientSecret.Sha256(), requireClientSecret: false, redirectUris: client.RedirectUris,
                postLogoutRedirectUris: client.PostLogoutRedirectUris, corsOrigins: client.AllowedCorsOrigins);
        }
    }

    private async Task CreateApiResourcesAsync()
    {
        string[]? commonApiUserClaims = new[] { "email", "email_verified", "name", "phone_number", "phone_number_verified", "role" };
        string[]? apiResources = _configuration.GetSection("ApiResource").Get<string[]>();
        foreach (string? item in apiResources)
        {
            _ = await CreateApiResourceAsync(item, commonApiUserClaims);
        }
    }

    private async Task CreateApiScopesAsync()
    {
        string[]? apiScopes = _configuration.GetSection("ApiScope").Get<string[]>();
        foreach (string? item in apiScopes)
        {
            _ = await CreateApiScopeAsync(item);
        }
    }

    private async Task<ApiResource> CreateApiResourceAsync(string name, IEnumerable<string> claims)
    {
        ApiResource? apiResource = await _apiResourceRepository.FindByNameAsync(name);
        if (apiResource == null)
        {
            apiResource = await _apiResourceRepository.InsertAsync(new ApiResource(_guidGenerator.Create(), name, name + " API"), true);
        }
        foreach (string? claim in claims)
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
        ApiScope? apiScope = await _apiScopeRepository.FindByNameAsync(name);
        if (apiScope == null)
        {
            apiScope = await _apiScopeRepository.InsertAsync(new ApiScope(_guidGenerator.Create(), name, name + " API"), true);
        }
        return apiScope;
    }

    private async Task<Client> CreateClientAsync(string name, IEnumerable<string> scopes, IEnumerable<string> grantTypes, string? secret = null, IEnumerable<string>? redirectUris = null,
        IEnumerable<string>? postLogoutRedirectUris = null, string? frontChannelLogoutUri = null, bool requireClientSecret = true, bool requirePkce = false, IEnumerable<string>? permissions = null,
        IEnumerable<string>? corsOrigins = null)
    {
        Client? client = await _clientRepository.FindByClientIdAsync(name);
        if (client == null)
        {
            client = await _clientRepository.InsertAsync(new Client(_guidGenerator.Create(), name)
            {
                ClientName = name,
                ProtocolType = "oidc",
                Description = name,
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowOfflineAccess = true,
                AbsoluteRefreshTokenLifetime = 31536000, // 365 days
                AccessTokenLifetime = 31536000, // 365 days
                AuthorizationCodeLifetime = 300,
                IdentityTokenLifetime = 300,
                RequireConsent = false,
                FrontChannelLogoutUri = frontChannelLogoutUri,
                RequireClientSecret = requireClientSecret,
                RequirePkce = requirePkce
            }, true);
        }
        foreach (string? scope in scopes)
        {
            if (client.FindScope(scope) == null)
            {
                client.AddScope(scope);
            }
        }
        foreach (string? grantType in grantTypes)
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
        foreach (string? redirectUrl in redirectUris ?? Array.Empty<string>())
        {
            if (client.FindRedirectUri(redirectUrl) == null)
            {
                client.AddRedirectUri(redirectUrl);
            }
        }
        foreach (string? postLogoutRedirectUri in postLogoutRedirectUris ?? Array.Empty<string>())
        {
            if (client.FindPostLogoutRedirectUri(postLogoutRedirectUri) == null)
            {
                client.AddPostLogoutRedirectUri(postLogoutRedirectUri);
            }
        }
        if (permissions != null)
        {
            await _permissionDataSeeder.SeedAsync(ClientPermissionValueProvider.ProviderName, name, permissions);
        }
        if (corsOrigins != null)
        {
            foreach (string? origin in corsOrigins)
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

