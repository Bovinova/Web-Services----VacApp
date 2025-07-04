using VacApp_Bovinova_Platform.IAM.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.IAM.Interfaces.REST.Resources;

namespace VacApp_Bovinova_Platform.IAM.Interfaces.REST.Transform
{
    public class UserResourceFromEntityAssembler
    {
        public static UserResource ToResourceFromEntity(string token, string? userName, string? email)
        {
            return new UserResource(token, userName, email);
        }
    }
}