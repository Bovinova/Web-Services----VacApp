using VacApp_Bovinova_Platform.Shared.Domain.Repositories;
using VacApp_Bovinova_Platform.StaffAdministration.Domain.Model.Aggregates;

namespace VacApp_Bovinova_Platform.StaffAdministration.Domain.Repositories;

public interface IStaffRepository : IBaseRepository<Staff>
{
    Task<Staff?> FindByNameAsync(string name);
    Task<IEnumerable<Staff>> FindByCampaignIdAsync(int campaignId);
    
    Task<IEnumerable<Staff>> FindByEmployeeStatusAsync(int employeeStatus);
}