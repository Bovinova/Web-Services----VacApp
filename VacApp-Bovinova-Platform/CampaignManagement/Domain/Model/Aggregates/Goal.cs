namespace VacApp_Bovinova_Platform.CampaignManagement.Domain.Model.Aggregates;

public class Goal
{
    public int Id { get; private set;}
    public string Description {get; private set;}
    public string Metric {get; private set;}
    public int TargetValue {get; private set;}
    public int CurrentValue { get; private set; }
    
    protected Goal()
    {
        this.Description = string.Empty;
        this.Metric = string.Empty;
        this.TargetValue = 0;
        this.CurrentValue = 0;
    }
}