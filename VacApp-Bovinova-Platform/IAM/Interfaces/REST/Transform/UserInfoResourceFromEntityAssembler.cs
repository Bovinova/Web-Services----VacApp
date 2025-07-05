using VacApp_Bovinova_Platform.IAM.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.IAM.Interfaces.REST.Resources.UserResources;

namespace VacApp_Bovinova_Platform.IAM.Interfaces.REST.Transform;

public static class UserInfoResourceFromEntityAssembler
{
    public static UserInfoResource ToResourceFromEntity(
        User user,
        int totalBovines,
        int totalCampaigns,
        int totalVaccinations,
        int totalStables,
        CampaignInfoResource[] nextCampaigns)
    {
        return new UserInfoResource(
            user.Id,
            user.Username,
            totalBovines,
            totalCampaigns,
            totalStables,
            totalVaccinations,
            nextCampaigns
        );
    }
}