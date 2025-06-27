namespace VacApp_Bovinova_Platform.RanchManagement.Domain.Model.ValueObjects;

public record UserId(int UserIdentifier)
{
    public UserId() : this(0) { }
}