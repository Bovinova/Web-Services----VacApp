using VacApp_Bovinova_Platform.CampaignManagement.Domain.Model.Commands;
using VacApp_Bovinova_Platform.CampaignManagement.Interfaces.REST.Resources;

namespace VacApp_Bovinova_Platform.CampaignManagement.Interfaces.REST.Transform;

public class CreateCampaignCommandFromResourceAssembler
{
    public static CreateCampaignCommand ToCommandFromResource(CreateCampaignResource resource) =>
        new CreateCampaignCommand(resource.Name, resource.Description, resource.StartDate, resource.EndDate, resource.Status);
    
}