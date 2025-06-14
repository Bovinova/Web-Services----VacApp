using Google.Apis.Auth.OAuth2;

namespace VacApp_Bovinova_Platform.IAM.Application.OutBoundServices;

public interface IGoogleAuthorization
{
    string GetAuthorizationUrl();
    Task<UserCredential> ExchangeCodeForToken(string code);
    Task<UserCredential> ValidateToken(string accessToken);
}