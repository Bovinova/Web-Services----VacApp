using VacApp_Bovinova_Platform.RanchManagement.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Model.Commands;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Repositories;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Services;
using VacApp_Bovinova_Platform.Shared.Domain.Repositories;

namespace VacApp_Bovinova_Platform.RanchManagement.Application.Internal.CommandServices;

public class StableCommandService(IStableRepository stableRepository, 
    IUnitOfWork unitOfWork) : IStableCommandService
{
    public async Task<Stable?> Handle(CreateStableCommand command)
    {
        // Check if a Stable entity with the given Name already exists
        var stable = 
            await stableRepository.FindByNameAsync(command.Name);
        if (stable != null) 
            throw new Exception($"Stable entity with name '{command.Name}' already exists.");
        // Create a new Stable entity from the command data
        stable = new Stable(command);

        try
        {
            // Add the new Stable entity to the repository and complete the transaction
            await stableRepository.AddAsync(stable);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return stable;
    }
}