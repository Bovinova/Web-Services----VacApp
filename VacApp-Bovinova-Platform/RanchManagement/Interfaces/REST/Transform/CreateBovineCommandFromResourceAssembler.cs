using VacApp_Bovinova_Platform.RanchManagement.Domain.Model.Commands;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Model.ValueObjects;
using VacApp_Bovinova_Platform.RanchManagement.Interfaces.REST.Resources;

namespace VacApp_Bovinova_Platform.RanchManagement.Interfaces.REST.Transform;

public class CreateBovineCommandFromResourceAssembler
{
    public static CreateBovineCommand ToCommandFromResource(CreateBovineResource resource, int userId)
    {
        return new CreateBovineCommand(
            resource.Name,
            resource.Gender,
            resource.BirthDate,
            resource.Breed,
            resource.Location,
            resource.BovineImg,
            resource.StableId,
            new UserId(userId)
        );
    }
}