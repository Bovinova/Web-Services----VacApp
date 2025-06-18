namespace VacApp_Bovinova_Platform.IAM.Interfaces.REST.Resources
{
    public record UserInfoResource(
        String name,
        int totalAnimals,
        int totalCampaigns,
        int totalStables
    );
}