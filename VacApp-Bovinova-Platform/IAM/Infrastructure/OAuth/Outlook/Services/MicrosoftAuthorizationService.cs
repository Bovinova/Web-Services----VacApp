using System.Text.Json;
using VacApp_Bovinova_Platform.IAM.Application.OutBoundServices;
using VacApp_Bovinova_Platform.IAM.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.IAM.Infrastructure.OAuth.Outlook.TokenHandler;

namespace VacApp_Bovinova_Platform.IAM.Infrastructure.OAuth.Outlook.Services;

public class MicrosoftAuthorizationService(IConfiguration config) : IMicrosoftAuthorization
{
    private readonly string _clientId = config["MicrosoftAuth:ClientId"];
    private readonly string _clientSecret = config["MicrosoftAuth:ClientSecret"];
    private readonly string _redirectUri = config["MicrosoftAuth:RedirectUri"];

    public string GetAuthorizationUrl()
    {
        var scopes = "https://graph.microsoft.com/user.read offline_access";
        var url = $"https://login.microsoftonline.com/common/oauth2/v2.0/authorize?" +
                  $"client_id={_clientId}&response_type=code&redirect_uri={Uri.EscapeDataString(_redirectUri)}" +
                  $"&response_mode=query&scope={Uri.EscapeDataString(scopes)}";
        return url;
    }

    public async Task<MicrosoftCredential> ExchangeCodeForToken(string code)
    {
        var httpClient = new HttpClient();

        var parameters = new Dictionary<string, string>
        {
            { "client_id", _clientId },
            { "scope", "https://graph.microsoft.com/user.read offline_access" },
            { "code", code },
            { "redirect_uri", _redirectUri },
            { "grant_type", "authorization_code" },
            { "client_secret", _clientSecret }
        };

        var response = await httpClient.PostAsync("https://login.microsoftonline.com/common/oauth2/v2.0/token",
            new FormUrlEncodedContent(parameters));

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error al intercambiar el código por un token: " + content);
        }

        var tokenResult = JsonSerializer.Deserialize<MicrosoftTokenResponse>(content);

        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResult.AccessToken);
        var userInfoResponse = await httpClient.GetStringAsync("https://graph.microsoft.com/v1.0/me");
        // Loguea la respuesta para ver qué campos llegan
        Console.WriteLine(userInfoResponse);

        var userInfo = JsonSerializer.Deserialize<MicrosoftUserInfo>(userInfoResponse);

        var email = userInfo.Mail ?? userInfo.UserPrincipalName;
        if (string.IsNullOrEmpty(email))
            throw new Exception("No se pudo obtener el email del usuario de Microsoft.");
        
        return new MicrosoftCredential
        {
            Email = userInfo.Mail ?? userInfo.UserPrincipalName,
            AccessToken = tokenResult.AccessToken,
            RefreshToken = tokenResult.RefreshToken
        };
    }
}