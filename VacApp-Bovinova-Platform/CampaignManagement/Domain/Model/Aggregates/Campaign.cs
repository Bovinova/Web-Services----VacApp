using VacApp_Bovinova_Platform.CampaignManagement.Domain.Model.Commands;

namespace VacApp_Bovinova_Platform.CampaignManagement.Domain.Model.Aggregates;

public class Campaign
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public string Status { get; private set; }
    
    public Goal Goal { get; private set; }

    protected Campaign()
    {
        this.Name = string.Empty;
        this.Description = string.Empty;
        this.StartDate = DateTime.Now;
        this.EndDate = DateTime.Now;
        this.Status = string.Empty;
        this.Goal = new Goal();
    }

    public Campaign(CreateCampaignCommand command)
    {
        this.Name = command.Name;
        this.Description = command.Description;
        this.StartDate = command.StartDate;
        this.EndDate = command.EndDate;
        this.Status = command.Status;
        this.Goal = command.Goal;
    }
    
    public void UpdateStatus(string status)
    {
        this.Status = status;
    }

    public void AddGoal(Goal goal)
    {
        this.Goal = goal;    
    }
    
}