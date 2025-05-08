using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Model.Queries;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Services;
using VacApp_Bovinova_Platform.RanchManagement.Interfaces.REST.Resources;
using VacApp_Bovinova_Platform.RanchManagement.Interfaces.REST.Transform;

namespace VacApp_Bovinova_Platform.RanchManagement.Interfaces.REST;

/// <summary>
/// API controller for managing bovines
/// </summary>
[ApiController]
[Route("/api/v1/bovines")]
[Produces(MediaTypeNames.Application.Json)]
[Tags ("Bovines")]
public class BovineController(IBovineCommandService commandService, 
    IBovineQueryService queryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateBovines([FromBody] CreateBovineResource resource)
    {
        var command = CreateBovineCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await commandService.Handle(command);
        if (result is null) return BadRequest();
    
        return CreatedAtAction(nameof(GetBovineById), new { id = result.Id },
            BovineResourceFromEntityAssembler.ToResourceFromEntity(result));
    }

    
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all bovines",
        Description = "Get all bovines",
        OperationId = "GetAllBovine")]
    [SwaggerResponse(StatusCodes.Status200OK, "The list of bovines were found", typeof(IEnumerable<BovineResource>))]
    public async Task<IActionResult> GetAllBovine()
    {
        var bovines = await queryService.Handle(new GetAllBovinesQuery());
        var bovineResources = bovines.Select(BovineResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(bovineResources);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult> GetBovineById(int id)
    {
        var getBovineById = new GetBovinesByIdQuery(id);
        var result = await queryService.Handle(getBovineById);
        if (result is null) return NotFound();
        var resources = BovineResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resources);
    }
    
    [HttpGet("stable/{stableId}")]
    [SwaggerOperation(
        Summary = "Get all bovines by stable ID",
        Description = "Get all bovines by stable ID",
        OperationId = "GetBovinesByStableId")]
    public async Task<ActionResult> GetBovinesByStableId(int? stableId)
    {
        var getBovinesByStableIdQuery = new GetBovinesByStableIdQuery(stableId);
        var bovines = await queryService.Handle(getBovinesByStableIdQuery);

        if (bovines == null || !bovines.Any()) 
            return NotFound();

        var bovineResources = bovines.Select(BovineResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(bovineResources);
    }
}

/// <summary>
/// API controller for managing vaccines
/// </summary>
[ApiController]
[Route("/api/v1/vaccines")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Vaccines")]
public class VaccineController(
    IVaccineCommandService commandService,
    IVaccineQueryService queryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateVaccines([FromBody] CreateVaccineResource resource)
    {
        var command = CreateVaccineCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await commandService.Handle(command);
        if (result is null) return BadRequest();

        return CreatedAtAction(nameof(GetVaccineById), new { id = result.Id },
            VaccineResourceFromEntityAssembler.ToResourceFromEntity(result));
    }


    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all vaccines",
        Description = "Get all vaccines",
        OperationId = "GetAllVaccine")]
    [SwaggerResponse(StatusCodes.Status200OK, "The list of vaccines were found", typeof(IEnumerable<VaccineResource>))]
    public async Task<IActionResult> GetAllVaccine()
    {
        var vaccines = await queryService.Handle(new GetAllVaccinesQuery());
        var vaccineResources = vaccines.Select(VaccineResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(vaccineResources);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetVaccineById(int id)
    {
        var getVaccineById = new GetVaccinesByIdQuery(id);
        var result = await queryService.Handle(getVaccineById);
        if (result is null) return NotFound();
        var resources = VaccineResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resources);
    }
}

/// <summary>
/// API controller for managing stables
/// </summary>
[ApiController]
[Route("/api/v1/stables")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Stables")]
public class StableController(
    IStableCommandService commandService,
    IStableQueryService queryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateStables([FromBody] CreateStableResource resource)
    {
        var command = CreateStableCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await commandService.Handle(command);
        if (result is null) return BadRequest();

        return CreatedAtAction(nameof(GetStableById), new { id = result.Id },
            StableResourceFromEntityAssembler.ToResourceFromEntity(result));
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all stables",
        Description = "Get all stables",
        OperationId = "GetAllStable")]
    [SwaggerResponse(StatusCodes.Status200OK, "The list of stables were found", typeof(IEnumerable<StableResource>))]
    public async Task<IActionResult> GetAllStable()
    {
        var stables = await queryService.Handle(new GetAllStablesQuery());
        var stableResources = stables.Select(StableResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(stableResources);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetStableById(int id)
    {
        var getStableById = new GetStablesByIdQuery(id);
        var result = await queryService.Handle(getStableById);
        if (result is null) return NotFound();
        var resources = StableResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resources);
    }
} 