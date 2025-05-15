namespace VacApp_Bovinova_Platform.RanchManagement.Interfaces.REST.Resources;

public class UpdateVaccineResource
{
    /*
    string Name,
    string? VaccineType,
    DateTime? VaccineDate,
    string? VaccineImg, 
    int BovineId
     */

    public string Name { get; set; }
    public string? VaccineType { get; set; }
    public DateTime? VaccineDate { get; set; }
    public int BovineId { get; set; }
    public IFormFile? fileData { get; set; }
}