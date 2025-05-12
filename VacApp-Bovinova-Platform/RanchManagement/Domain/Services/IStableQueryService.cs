using VacApp_Bovinova_Platform.RanchManagement.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Model.Queries;

namespace VacApp_Bovinova_Platform.RanchManagement.Domain.Services;

public interface IStableQueryService
{
    Task<IEnumerable<Stable>> Handle(GetAllStablesQuery query);
    
    Task<Stable> Handle(GetStablesByIdQuery query);
}