using System.ComponentModel.DataAnnotations;

namespace VacApp_Bovinova_Platform.IAM.Domain.Model.Aggregates;

/* * Represents a user's credentials in the system from Google.
 * This class is used to store access tokens, refresh tokens, and other related information.
 */
public class Credential
{
    [Key]
    public Guid UserId { get; set; }
    
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public long? ExpiresInSeconds { get; set; }
    public string IdToken { get; set; }
    public DateTime IssuedUtc { get; set; }
}