namespace VacApp_Bovinova_Platform.IAM.Interfaces.REST.Resources;

public record UserInfoResource(
    String Name,
    int TotalBovines/*,
    int TotalCampaigns,
    int TotalVaccinations,
    int TotalStables*/
    );