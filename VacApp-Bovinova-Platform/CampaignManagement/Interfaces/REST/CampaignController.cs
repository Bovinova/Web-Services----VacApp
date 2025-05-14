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

    [HttpPatch("{id}/update-status")]
    public async Task<ActionResult> UpdateCampaignStatus([FromRoute] int id, [FromBody] UpdateCampaignStatusResource resource)
    {
        var updateCampaignStatusCommand = UpdateCampaignStatusFromResourceAssembler.ToCommandFromResource(resource, id);
        var result = await campaignCommandService.Handle(updateCampaignStatusCommand);
        if (result is null) return BadRequest();
        var resourceFromEntity = CampaignResourceFromEntityAssembler.ToResourceFromEntity(result);
        return CreatedAtAction(nameof(GetCampaignById), new { id = result.Id }, resourceFromEntity);
    }
    
    [HttpPatch("{id}/add-goal")]
    public async Task<ActionResult> AddGoalToCampaign([FromRoute] int id, [FromBody] AddGoalToCampaignResource resource)
    {
        var addGoalToCampaignCommand = AddGoalToCampaignFromResourceAssembler.ToCommandFromResource(resource, id);
        var result = await campaignCommandService.Handle(addGoalToCampaignCommand);
        if (result is null) return BadRequest();
        var resourceFromEntity = CampaignResourceFromEntityAssembler.ToResourceFromEntity(result);
        return CreatedAtAction(nameof(GetCampaignById), new { id = result.Id }, resourceFromEntity);
    }
    
    [HttpPatch("{id}/add-channel")]
    public async Task<ActionResult> AddChannelToCampaign([FromRoute] int id, [FromBody] AddChannelToCampaignResource resource)
    {
        var addChannelToCampaignCommand = AddChannelToCampaignFromResourceAssembler.ToCommandFromResource(resource, id);
        var result = await campaignCommandService.Handle(addChannelToCampaignCommand);
        if (result is null) return BadRequest();
        var resourceFromEntity = CampaignResourceFromEntityAssembler.ToResourceFromEntity(result);
        return CreatedAtAction(nameof(GetCampaignById), new { id = result.Id }, resourceFromEntity);
    }
    
    [HttpGet("{id}/goals")]
    public async Task<ActionResult> GetGoalsFromCampaign([FromRoute] int id)
    {
        var getGoalsFromCampaignIdQuery = new GetGoalsFromCampaignIdQuery(id);
        var result = await campaignQueryService.Handle(getGoalsFromCampaignIdQuery);
        if (result is null) return NotFound();
        var resources = result.Select(GoalResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    
    [HttpGet("{id}/channels")]
    public async Task<ActionResult> GetChannelsFromCampaign([FromRoute] int id)
    {
        var getChannelsFromCampaignIdQuery = new GetChannelsFromCampaignIdQuery(id);
        var result = await campaignQueryService.Handle(getChannelsFromCampaignIdQuery);
        if (result is null) return NotFound();
        var resources = result.Select(ChannelResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
}