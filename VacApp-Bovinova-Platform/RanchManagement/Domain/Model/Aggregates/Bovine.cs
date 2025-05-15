using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Model.Commands;

namespace VacApp_Bovinova_Platform.RanchManagement.Domain.Model.Aggregates;

public class Bovine
{
    /// <summary>
    /// Entity Identifier
    /// </summary>
    [Required]
    public int Id { get; private set; }

    /// <summary>
    /// Name of the Bovine
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; private set; }

    /// <summary>
    /// Gender of the Bovine (Male or Female)
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Gender { get; private set; }

    /// <summary>
    /// Date of Birth of the Bovine
    /// </summary>
    [Required]
    public DateTime? BirthDate { get; private set; }

    /// <summary>
    /// Breed of the Bovine
    /// </summary>
    [Required]
    [StringLength(100)]
    public string? Breed { get; private set; }

    /// <summary>
    /// Actual Location of the Bovine
    /// </summary>
    [Required]
    [StringLength(100)]
    public string? Location { get; private set; }

    /// <summary>
    /// Stable Identifier As Foreign Key
    /// </summary>
    [Required]
    public int? StableId { get; private set; }
    /// <summary>
    /// Instancing the Stable Entity for the Foreign Key
    /// </summary>
    [ForeignKey(nameof(StableId))]
    public Stable? Stable { get; private set; }

    /// <summary>
    /// Bovine Image
    /// </summary>
    [Required]
    [StringLength(300)]
    public string? BovineImg { get; private set; }

    // Default constructor for EF Core
    private Bovine() { }

    // Constructor with parameters
    public Bovine(CreateBovineCommand command)
    {
        if (!command.Gender.ToLower().Equals("male") && !command.Gender.ToLower().Equals("female"))
            throw new ArgumentException("Gender must be either 'male' or 'female'");

        Name = command.Name;
        Gender = command.Gender;
        BirthDate = command.BirthDate;
        Breed = command.Breed;
        Location = command.Location;
        BovineImg = command.BovineImg;
        StableId = command.StableId;
    }

    //Update Bovine
    public void Update(UpdateBovineCommand command)
    {
        if (!command.Gender.ToLower().Equals("male") && !command.Gender.ToLower().Equals("female"))
            throw new ArgumentException("Gender must be either 'male' or 'female'");

        Name = command.Name;
        Gender = command.Gender;
        BirthDate = command.BirthDate;
        Breed = command.Breed;
        Location = command.Location;
        StableId = command.StableId;
    }
}