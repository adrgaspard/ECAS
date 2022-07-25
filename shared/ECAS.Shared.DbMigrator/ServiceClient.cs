namespace ECAS.Shared.DbMigrator;

public class ServiceClient
{
    public ServiceClient()
    {
        ClientId = "";
        ClientSecret = "";
        RootUrls = Array.Empty<string>();
        Scopes = Array.Empty<string>();
        GrantTypes = Array.Empty<string>();
        RedirectUris = Array.Empty<string>();
        PostLogoutRedirectUris = Array.Empty<string>();
        AllowedCorsOrigins = Array.Empty<string>();
    }

    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string[] RootUrls { get; set; }
    public string[] Scopes { get; set; }
    public string[] GrantTypes { get; set; }
    public string[] RedirectUris { get; set; }
    public string[] PostLogoutRedirectUris { get; set; }
    public string[] AllowedCorsOrigins { get; set; }
}

