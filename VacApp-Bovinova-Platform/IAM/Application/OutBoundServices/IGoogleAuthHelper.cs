using Google.Apis.Auth.OAuth2;

namespace VacApp_Bovinova_Platform.IAM.Application.OutBoundServices;

public interface IGoogleAuthHelper
{
    string[] GetScopes();
    string ScopeToString();
    ClientSecrets GetClientSecrets();
}