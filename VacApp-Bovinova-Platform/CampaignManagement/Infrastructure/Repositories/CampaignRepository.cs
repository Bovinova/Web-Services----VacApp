using Microsoft.EntityFrameworkCore;
using VacApp_Bovinova_Platform.CampaignManagement.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.CampaignManagement.Domain.Repositories;
using VacApp_Bovinova_Platform.Shared.Infrastructure.Persistence.EFC.Configuration;
using VacApp_Bovinova_Platform.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace VacApp_Bovinova_Platform.CampaignManagement.Infrastructure.Repositories;

public class CampaignRepository : BaseRepository<Campaign>, ICampaignRepository
{
    public CampaignRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Campaign?> FindByNameAsync(string name)
    {
        return await Context.Set<Campaign>().FirstOrDefaultAsync(c => c.Name == name);
    }
}