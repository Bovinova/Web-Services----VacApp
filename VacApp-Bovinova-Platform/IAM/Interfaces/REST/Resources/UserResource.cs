namespace VacApp_Bovinova_Platform.IAM.Interfaces.REST.Resources
{
    public record UserResource(
        string token,
        string? userName,
        string email
        );
}