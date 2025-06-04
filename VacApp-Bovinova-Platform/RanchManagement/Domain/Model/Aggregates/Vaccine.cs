using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Model.Commands;

namespace VacApp_Bovinova_Platform.RanchManagement.Domain.Model.Aggregates;

public class Vaccine
{
    /// <summary>
    /// Entity Identifier
    /// </summary>
    [Required]
    public int Id { get; private set; }

    /// <summary>
    /// Name of the Vaccine
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; private set; }

    /// <summary>
    /// Vaccine Type
    /// </summary>
    [Required]
    [StringLength(100)]
    public string? VaccineType { get; private set; }

    /// <summary>
    /// Date of the Vaccination
    /// </summary>
    [Required]
    public DateTime? VaccineDate { get; private set; }

    /// <summary>
    /// Vaccine Image
    /// </summary>
    [Required]
    [StringLength(300)]
    public string? VaccineImg { get; private set; }
    private static readonly Regex ImageUrlRegex = new(@"^(https?:\/\/.*\.(?:png|jpg|jpeg|gif|svg))$", RegexOptions.IgnoreCase);


    /// <summary>
    /// Bovine Identifier As Foreign Key
    /// </summary>
    [Required]
    public int BovineId { get; private set; }
    /// <summary>
    /// Instancing the Bovine Entity for the Foreign Key
    /// </summary>
    [ForeignKey(nameof(BovineId))]
    public Bovine? Bovine { get; private set; }

    // Default constructor for EF Core
    private Vaccine() { }
    public Vaccine(int id, string name, string? vaccineType, DateTime? vaccineDate, string? vaccineImg, int bovineId, Bovine? bovine)
    {
        Id = id;
        Name = name;
        VaccineType = vaccineType;
        VaccineDate = vaccineDate;
        VaccineImg = ValidateImageUrl(vaccineImg);
        BovineId = bovineId;
        Bovine = bovine;
    }

    // Constructor with parameters
    public Vaccine(CreateVaccineCommand command)
    {
        Name = command.Name;
        VaccineType = command.VaccineType;
        VaccineDate = command.VaccineDate;
        VaccineImg = ValidateImageUrl(command.VaccineImg);
        BovineId = command.BovineId;
    }

    //Update
    public void Update(UpdateVaccineCommand command)
    {
        Name = command.Name;
        VaccineType = command.VaccineType;
        VaccineDate = command.VaccineDate;
        BovineId = command.BovineId;
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