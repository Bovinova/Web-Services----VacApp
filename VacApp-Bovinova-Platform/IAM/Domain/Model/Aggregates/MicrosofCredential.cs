using System.ComponentModel.DataAnnotations;

namespace VacApp_Bovinova_Platform.IAM.Domain.Model.Aggregates;

/*
 * Represents a user's credentials in the system from Microsoft (Outlook).
 */
public class MicrosoftCredential
{
    [Key]
    public Guid UserId { get; set; }

    public string AccessToken { get; set; }
    
    public string RefreshToken { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    public DateTime IssuedUtc { get; set; } = DateTime.UtcNow;
}