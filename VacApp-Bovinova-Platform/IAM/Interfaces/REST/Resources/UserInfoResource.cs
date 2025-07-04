namespace VacApp_Bovinova_Platform.IAM.Interfaces.REST.Resources;

public record UserInfoResource(
    string Name,
    int TotalBovines,
    //int TotalCampaigns,
    int TotalVaccinations,
    int TotalStables
    );