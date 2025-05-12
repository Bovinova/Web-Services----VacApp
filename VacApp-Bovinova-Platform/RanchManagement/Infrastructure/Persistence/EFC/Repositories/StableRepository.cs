using Microsoft.EntityFrameworkCore;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Repositories;
using VacApp_Bovinova_Platform.Shared.Infrastructure.Persistence.EFC.Configuration;
using VacApp_Bovinova_Platform.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace VacApp_Bovinova_Platform.RanchManagement.Infrastructure.Persistence.EFC.Repositories;

public class StableRepository(AppDbContext ctx)
    : BaseRepository<Stable>(ctx), IStableRepository
{
    public async Task<Stable?> FindByNameAsync(string name)
    {
        return await Context.Set<Stable>().FirstOrDefaultAsync(f=>f.Name == name);
    }
}