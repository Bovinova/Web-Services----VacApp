using System.Text.Json.Serialization;

namespace VacApp_Bovinova_Platform.IAM.Infrastructure.OAuth.Outlook.TokenHandler;

public class MicrosoftTokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string IdToken { get; set; }
    public string TokenType { get; set; }
    public int ExpiresIn { get; set; }

    [JsonPropertyName("access_token")]
    public string AccessTokenJson { set => AccessToken = value; }

    [JsonPropertyName("refresh_token")]
    public string RefreshTokenJson { set => RefreshToken = value; }
}