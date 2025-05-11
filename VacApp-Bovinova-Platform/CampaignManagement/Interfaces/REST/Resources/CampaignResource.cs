namespace VacApp_Bovinova_Platform.CampaignManagement.Interfaces.REST.Resources;

public record CampaignResource(
    int Id,
    string Name,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    string Status
    );