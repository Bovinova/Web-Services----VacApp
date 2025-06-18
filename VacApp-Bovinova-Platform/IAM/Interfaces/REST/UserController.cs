using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using VacApp_Bovinova_Platform.IAM.Domain.Services;
using VacApp_Bovinova_Platform.IAM.Interfaces.REST.Resources;
using VacApp_Bovinova_Platform.IAM.Interfaces.REST.Transform;
using VacApp_Bovinova_Platform.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using VacApp_Bovinova_Platform.IAM.Domain.Model.Aggregates;
using Swashbuckle.AspNetCore.Annotations;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Services;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Model.Queries;
using VacApp_Bovinova_Platform.CampaignManagement.Domain.Services;
using VacApp_Bovinova_Platform.CampaignManagement.Domain.Model.Queries;


namespace VacApp_Bovinova_Platform.IAM.Interfaces.REST
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Tags("User")]
    public class UserController(
        IUserCommandService commandService,
        IBovineQueryService bovineQueryService,
        IStableQueryService stableQueryService,
        ICampaignQueryService campaignQueryService
        ) : ControllerBase
    {
        [HttpPost("sign-up")]
        [AllowAnonymous]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(UserResource))]
        public async Task<IActionResult> SignUp([FromBody] SignUpResource resource)
        {
            var command = SignUpCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await commandService.Handle(command);

            if (result is null) return BadRequest("User already exists");

            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(result);

            return CreatedAtAction(nameof(SignUp), userResource);
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(UserResource))]
        public async Task<ActionResult> SignIn([FromBody] SignInResource resource)
        {
            var command = SignInCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await commandService.Handle(command);

            if (result is null) return BadRequest("Invalid credentials.");

            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(result);

            return Ok(userResource);
        }

        [HttpGet("get-info")]
        [SwaggerResponse(StatusCodes.Status200OK, "User info", typeof(UserInfoResource))]
        public ActionResult GetInfo()
        {
            var user = (User?)HttpContext.Items["User"];

            if (user is null) return Unauthorized("User not found.");

            var totalAnimals = bovineQueryService.Handle(new GetAllBovinesQuery(user.Id)).Result.Count();
            var totalStables = stableQueryService.Handle(new GetAllStablesQuery(user.Id)).Result.Count();
            var totalCampaigns = campaignQueryService.Handle(new GetAllCampaignsQuery(user.Id)).Result.Count();

            if (user is null) return Unauthorized("User not found.");
            var userInfoResource = UserInfoResourceFromEntityAssembler.ToResourceFromEntity(user, totalAnimals, totalCampaigns, totalStables);

            return Ok(userInfoResource);
        }
    }
}