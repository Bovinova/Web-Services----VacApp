using VacApp_Bovinova_Platform.RanchManagement.Domain.Model.Commands;
using VacApp_Bovinova_Platform.RanchManagement.Interfaces.REST.Resources;

namespace VacApp_Bovinova_Platform.RanchManagement.Interfaces.REST.Transform;

public class CreateStableCommandFromResourceAssembler
{
    public static CreateStableCommand ToCommandFromResource(CreateStableResource resource)
    {
        return new CreateStableCommand(
            resource.Name,
            resource.Limit
        );
    }
}