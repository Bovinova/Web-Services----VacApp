using System.Numerics;
using VacApp_Bovinova_Platform.IAM.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.IAM.Interfaces.REST.Resources;

namespace VacApp_Bovinova_Platform.IAM.Interfaces.REST.Transform
{
    public class UserInfoResourceFromEntityAssembler
    {
        public static UserInfoResource ToResourceFromEntity(User user, int totalAnimals, int totalCampaigns,
        int totalStables)
        {
            return new UserInfoResource(
                user.Username,
                totalAnimals,
                totalCampaigns,
                totalStables
            );
        }
    }
}