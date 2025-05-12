using VacApp_Bovinova_Platform.CampaignManagement.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.Shared.Domain.Repositories;

namespace VacApp_Bovinova_Platform.CampaignManagement.Domain.Repositories;

public interface ICampaignRepository : IBaseRepository<Campaign>
{
    Task<Campaign?> FindByNameAsync(string name);
    Task<IEnumerable<Goal>> FindByCampaignId(int campaignId);
    
    Task<IEnumerable<Channel>> FindChannelsByCampaignId(int campaignId);
}