using System.ComponentModel.DataAnnotations;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Model.Commands;

namespace VacApp_Bovinova_Platform.RanchManagement.Domain.Model.Aggregates;

public class Stable
{
    /// <summary>
    /// Identifier for the Stable entity
    /// </summary>
    [Required]
    public int Id { get; private set; }

    /// <summary>
    /// Name of the Stable
    /// </summary>
    [Required]
    public string Name { get; private set; }

    /// <summary>
    /// Max. Capacity of the Stable
    /// </summary>
    [Required]
    public int Limit { get; private set; }

    public int UserId { get; private set; }

    // Default constructor for EF Core
    private Stable() { }

    // Constructor with parameters
    public Stable(CreateStableCommand command)
    {
        if (command.Limit <= 0)
        {
            throw new ArgumentException("Limit must be greater than 0");
        }

        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ArgumentException("Name cannot be empty or whitespace");

        Limit = command.Limit;
        Name = command.Name;
        UserId = command.UserId;
    }

    //Update
    public void Update(UpdateStableCommand command)
    {
        if (command.Limit <= 0)
        {
            throw new ArgumentException("Limit must be greater than 0");
        }

        Limit = command.Limit;
        Name = command.Name;
    }
}