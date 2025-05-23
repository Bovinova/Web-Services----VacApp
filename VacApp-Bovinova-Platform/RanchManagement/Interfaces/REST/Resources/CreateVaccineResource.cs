namespace VacApp_Bovinova_Platform.RanchManagement.Interfaces.REST.Resources;

public record CreateVaccineResource(string Name,
    string? VaccineType,
    DateTime? VaccineDate,
    int BovineId,
    IFormFile? fileData);