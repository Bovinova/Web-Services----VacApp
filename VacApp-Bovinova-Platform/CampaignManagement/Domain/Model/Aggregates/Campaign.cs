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
    
    public ICollection<Goal> Goals { get; private set; }
    
    public int GoalId { get; private set; }
    public ICollection<Channel> Channels { get; private set; }

    protected Campaign()
    {
        this.Name = string.Empty;
        this.Description = string.Empty;
        this.StartDate = DateTime.Now;
        this.EndDate = DateTime.Now;
        this.Status = string.Empty;
        this.Goals = new List<Goal>();
        this.Channels = new List<Channel>();
        
        //this.Channel = new Channel();      
    }

    public Campaign(CreateCampaignCommand command)
    {
        this.Name = command.Name;
        this.Description = command.Description;
        this.StartDate = command.StartDate;
        this.EndDate = command.EndDate;
        this.Status = command.Status;
        this.Goals = command.Goals;
        this.Channels = command.Channel;   
    }
    
    public void UpdateStatus(string status)
    {
        this.Status = status;
    }

    public void AddGoal(Goal goal)
    {
        //Goal _goal = new Goal(goal.Description, goal.Metric, goal.TargetValue, goal.CurrentValue);
        //this.Goal.UpdateValues(goal.Description, goal.Metric, goal.TargetValue, goal.CurrentValue);  
        //this.Goal.UpdateValues(description, metric, targetValue, currentValue);
        this.Goals.Add(goal);
    }
    
    public void AddChannel(Channel channel)
    {
        this.Channels.Add(channel);
    }
    
}