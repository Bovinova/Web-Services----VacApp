using VacApp_Bovinova_Platform.IAM.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.IAM.Domain.Model.Queries;

namespace VacApp_Bovinova_Platform.IAM.Domain.Services
{
    public interface IUserQueryService
    {
        Task<User?> Handle(GetUserByIdQuery query);
    }
}