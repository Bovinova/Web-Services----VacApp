using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
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
    private static readonly Regex ImageUrlRegex = new(@"^(https?:\/\/.*\.(?:png|jpg|jpeg|gif|svg))$", RegexOptions.IgnoreCase);
    
    // Default constructor for EF Core
    private Bovine() { }
    
    public Bovine(string name, string gender, DateTime? birthDate, string? breed, string? location, string? bovineImg, int? stableId)
    {
        Name = name;
        Gender = gender;
        BirthDate = birthDate;
        Breed = breed;
        Location = location;
        BovineImg = ValidateImageUrl(bovineImg);
        StableId = stableId;
    }

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
        BovineImg = ValidateImageUrl(command.BovineImg);
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
    
    private static string ValidateImageUrl(string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl) || !ImageUrlRegex.IsMatch(imageUrl))
        {
            throw new ArgumentException("The image URL must be a valid link to an image file.");
        }
        return imageUrl;
    }
}