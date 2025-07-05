namespace VacApp_Bovinova_Platform.IAM.Interfaces.REST.Resources.UserResources;

/* public record UserInfoResource(
    string Name,
    int TotalBovines,
    //int TotalCampaigns,
    int TotalVaccinations,
    int TotalStables
    ); */

public record UserInfoResource(
   int id,
   string name,
   int totalAnimals,
   int totalCampaigns,
   int totalStables,
   int totalVaccinations,
   CampaignInfoResource[] nextCampaigns
);

public record CampaignInfoResource(
    int id,
    string name,
    DateTime startDate
);