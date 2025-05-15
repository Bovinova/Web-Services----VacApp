using VacApp_Bovinova_Platform.StaffAdministration.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.StaffAdministration.Domain.Model.Commands;
using VacApp_Bovinova_Platform.StaffAdministration.Interfaces.REST.Resources;

namespace VacApp_Bovinova_Platform.StaffAdministration.Interfaces.REST.Transform;

public static class CreateStaffCommandFromResourceAssembler
{
    public static CreateStaffCommand ToCommandFromResource(CreateStaffResource resource)
    {
        return new CreateStaffCommand(
            resource.Name,
            resource.EmployeeStatus,
            resource.CampaignId
        );
    }
}