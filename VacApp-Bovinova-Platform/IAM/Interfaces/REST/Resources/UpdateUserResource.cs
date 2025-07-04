namespace VacApp_Bovinova_Platform.IAM.Interfaces.REST.Resources;

public record UpdateUserResource(
    string Username,
    string Email
);