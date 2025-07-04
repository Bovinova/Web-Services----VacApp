using Google.Apis.Auth.OAuth2;
using VacApp_Bovinova_Platform.IAM.Domain.Model.Aggregates;

namespace VacApp_Bovinova_Platform.IAM.Application.OutBoundServices;

public interface IMicrosoftAuthorization
{
    string GetAuthorizationUrl();
    Task<MicrosoftCredential> ExchangeCodeForToken(string code);
}