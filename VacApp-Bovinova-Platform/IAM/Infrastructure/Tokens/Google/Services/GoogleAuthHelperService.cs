using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using VacApp_Bovinova_Platform.IAM.Application.OutBoundServices;

namespace VacApp_Bovinova_Platform.IAM.Infrastructure.Tokens.Google.Services;

public class GoogleAuthHelperService(IConfiguration configuration) : IGoogleAuthHelper
{
    public string[] GetScopes()
    {
        var scopes = new[]
        {
            Oauth2Service.Scope.Openid,
            Oauth2Service.Scope.UserinfoEmail,
            Oauth2Service.Scope.UserinfoProfile
        };
        return scopes;
    }

    public string ScopeToString() => string.Join(", ", GetScopes());

    public ClientSecrets GetClientSecrets()
    {
        string clientId = configuration["Google:ClientId"]!;
        string clientSecret = configuration["Google:ClientSecret"]!;
        return new() { ClientId = clientId, ClientSecret = clientSecret };
        
    }
}