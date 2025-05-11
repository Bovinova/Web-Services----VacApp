using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using VacApp_Bovinova_Platform.CampaignManagement.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.CampaignManagement.Domain.Model.Commands;
using VacApp_Bovinova_Platform.CampaignManagement.Domain.Model.Queries;
using VacApp_Bovinova_Platform.CampaignManagement.Domain.Services;
using VacApp_Bovinova_Platform.CampaignManagement.Interfaces.REST.Resources;
using VacApp_Bovinova_Platform.CampaignManagement.Interfaces.REST.Transform;

namespace VacApp_Bovinova_Platform.CampaignManagement.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class CampaignController(ICampaignCommandService campaignCommandService, ICampaignQueryService campaignQueryService)
    :ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CreateCampaign([FromBody] CreateCampaignResource resource)
    {
        var createCampaignCommand = CreateCampaignCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await campaignCommandService.Handle(createCampaignCommand);
        if (result is null) return BadRequest();
        return CreatedAtAction(nameof(GetCampaignById), new { id = result.Id },
            CampaignResourceFromEntityAssembler.ToResourceFromEntity(result));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetCampaignById(int id)
    {
        var getCampaignByIdQuery = new GetCampaignByIdQuery(id);
        var result = await campaignQueryService.Handle(getCampaignByIdQuery);
        if (result is null) return NotFound();
        var resource = CampaignResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    [HttpGet("all-campaigns")]
    public async Task<ActionResult> GetAllCampaigns()
    {
        var campaigns = await campaignQueryService.Handle(new GetAllCampaignsQuery());
        var campaignResources = campaigns.Select(CampaignResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(campaignResources);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCampaign([FromRoute]int id)
    {
        var campaigns = await campaignCommandService.Handle(new DeleteCampaignCommand(id));
        var resources = campaigns.Select(CampaignResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPut("{id}/update-status")]
    public async Task<ActionResult> UpdateCampaignStatus([FromRoute] int id, [FromBody] UpdateCampaignStatusResource resource)
    {
        var updateCampaignStatusCommand = UpdateCampaignStatusFromResourceAssembler.ToCommandFromResource(resource, id);
        var result = await campaignCommandService.Handle(updateCampaignStatusCommand);
        if (result is null) return BadRequest();
        var resourceFromEntity = CampaignResourceFromEntityAssembler.ToResourceFromEntity(result);
        return CreatedAtAction(nameof(GetCampaignById), new { id = result.Id }, resourceFromEntity);
    }
}