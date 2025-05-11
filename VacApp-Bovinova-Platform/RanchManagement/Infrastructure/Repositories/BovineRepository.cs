using Microsoft.EntityFrameworkCore;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Repositories;
using VacApp_Bovinova_Platform.Shared.Infrastructure.Persistence.EFC.Configuration;
using VacApp_Bovinova_Platform.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace VacApp_Bovinova_Platform.RanchManagement.Infrastructure.Repositories;

public class BovineRepository(AppDbContext ctx)
    : BaseRepository<Bovine>(ctx), IBovineRepository
{
    public async Task<Bovine?> FindByNameAsync(string name)
    {
        return await Context.Set<Bovine>().FirstOrDefaultAsync(f=>f.Name == name);
    }
    
    public async Task<IEnumerable<Bovine>> FindByStableIdAsync(int? stableId)
    {
        return await Context.Set<Bovine>().Where(f => f.StableId == stableId).ToListAsync();
    }
    
    public async Task<int> CountBovinesByStableIdAsync(int stableId)
    {
        return await Context.Set<Bovine>().CountAsync(b => b.StableId == stableId);
    }
    
    /*
    public async Task UpdateAsync(Bovine bovine)
    {
        Context.Entry(bovine).State = EntityState.Modified;
        await Context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(Bovine bovine)
    {
        Context.Entry(bovine).State = EntityState.Deleted;
        await Context.SaveChangesAsync();
    }*/
}