namespace VacApp_Bovinova_Platform.IAM.Interfaces.REST.Resources;

public record UserProfileResource(
    string Username,
    string Email,
    bool EmailConfirmed
);