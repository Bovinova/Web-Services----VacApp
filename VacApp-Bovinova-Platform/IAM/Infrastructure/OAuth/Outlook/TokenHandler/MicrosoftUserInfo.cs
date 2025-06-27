using System.Text.Json.Serialization;

namespace VacApp_Bovinova_Platform.IAM.Infrastructure.OAuth.Outlook.TokenHandler;

public class MicrosoftUserInfo
{
    [JsonPropertyName("mail")]
    public string Mail { get; set; }

    [JsonPropertyName("userPrincipalName")]
    public string UserPrincipalName { get; set; }
}