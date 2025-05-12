using System.ComponentModel.DataAnnotations;
using VacApp_Bovinova_Platform.StaffAdministration.Domain.Model.Commands;
using VacApp_Bovinova_Platform.StaffAdministration.Domain.Model.ValueObjects;

namespace VacApp_Bovinova_Platform.StaffAdministration.Domain.Model.Aggregates;

public class Staff
{
    [Required]
    public int Id { get; private set; }

    [Required]
    [StringLength(100)]
    public string Name { get; private set; }
    
    public EmployeeStatus EmployeeStatus { get; private set; }
    
    public CampaignId CampaignId { get; private set; }

    public Staff()
    {
        Name = "";
        EmployeeStatus = new EmployeeStatus();
        CampaignId = new CampaignId();
    }
    
    public Staff(string name, int employeeStatus, int campaignId)
    {
        Name = name;
        EmployeeStatus = new EmployeeStatus(employeeStatus);
        CampaignId = new CampaignId(campaignId);
    }

    // Constructor for creating a new Staff
    public Staff(CreateStaffCommand command)
    {
        Name = command.Name;
        EmployeeStatus = new EmployeeStatus(command.EmployeeStatus);
        CampaignId = new CampaignId(command.CampaignId);
    }

    // Update method for modifying an existing Staff
    public void Update(UpdateStaffCommand command)
    {
        Name = command.Name;
        EmployeeStatus = new EmployeeStatus(command.EmployeeStatus);
        CampaignId = new CampaignId(command.CampaignId);
    }
}