namespace VacApp_Bovinova_Platform.CampaignManagement.Domain.Model.Aggregates;

public class Channel
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Details { get; set; }
    
    protected Channel()
    {
        this.Type = string.Empty;
        this.Details = string.Empty;
    }
}