using VacApp_Bovinova_Platform.RanchManagement.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Model.Queries;
using VacApp_Bovinova_Platform.Shared.Domain.Repositories;

namespace VacApp_Bovinova_Platform.RanchManagement.Domain.Repositories;

public interface IBovineRepository : IBaseRepository<Bovine>
{
    Task<Bovine?> FindByNameAsync(string name);
    
    Task<IEnumerable<Bovine>> FindByStableIdAsync(int? stableId);
    
    Task<int> CountBovinesByStableIdAsync(int stableId);
    
    /*
    Task UpdateAsync(Bovine bovine);
    
    Task DeleteAsync(Bovine bovine);*/
}