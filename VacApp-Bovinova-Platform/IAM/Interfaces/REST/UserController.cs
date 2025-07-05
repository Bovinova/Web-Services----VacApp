using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VacApp_Bovinova_Platform.CampaignManagement.Domain.Model.Queries;
using VacApp_Bovinova_Platform.CampaignManagement.Domain.Services;
using VacApp_Bovinova_Platform.IAM.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.IAM.Domain.Model.Commands;
using VacApp_Bovinova_Platform.IAM.Domain.Model.Queries;
using VacApp_Bovinova_Platform.IAM.Domain.Services;
using VacApp_Bovinova_Platform.IAM.Interfaces.REST.Resources;
using VacApp_Bovinova_Platform.IAM.Interfaces.REST.Resources.UserResources;
using VacApp_Bovinova_Platform.IAM.Interfaces.REST.Transform;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Model.Queries;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Model.ValueObjects;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Services;

namespace VacApp_Bovinova_Platform.IAM.Interfaces.REST
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Tags("Users")]
    public class UserController(
        IUserCommandService commandService,
        IUserQueryService queryService,
        IBovineQueryService bovineQueryService,
        ICampaignQueryService campaignQueryService,
        IVaccineQueryService vaccineQueryService,
        IStableQueryService stableQueryService
        ) : ControllerBase
    {
        /*
         * 
         */
        [HttpPost("sign-up")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpResource resource)
        {
            var command = SignUpCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await commandService.Handle(command);

            if (result is null) return BadRequest("User already exists");

            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(result, resource.Username, resource.Email);

            return CreatedAtAction(nameof(SignUp), userResource);
        }

        /*
         * 
         */
        [HttpPost("sign-in")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<ActionResult> SignIn([FromBody] SignInResource resource)
        {
            if (string.IsNullOrEmpty(resource.Email) && string.IsNullOrEmpty(resource.UserName))
            {
                return BadRequest("Either Email or UserName must be provided.");
            }

            var command = SignInCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await commandService.Handle(command);

            if (result is null) return BadRequest("Invalid credentials.");

            var userName = !string.IsNullOrEmpty(resource.UserName)
                ? resource.UserName
                : await queryService.GetUserNameByEmail(resource.Email!);

            var email = !string.IsNullOrEmpty(resource.Email)
                ? resource.Email
                : await queryService.GetEmailByUserName(resource.UserName!);

            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(result, userName, email);

            return Ok(userResource);
        }

        /*
         * 
         */
        [HttpGet("get-info")]
        [SwaggerResponse(StatusCodes.Status200OK, "User info", typeof(Resources.UserInfoResource))]
        public ActionResult GetInfo()
        {
            var user = (User?)HttpContext.Items["User"];

            if (user is null) return Unauthorized("User not found.");

            // Total de bovinos
            var totalBovines = bovineQueryService.Handle(new GetAllBovinesQuery(user.Id)).Result.Count();

            // Total de establos
            var totalStables = stableQueryService.Handle(new GetAllStablesQuery(user.Id)).Result.Count();

            // Total de campañas
            var totalCampaigns = campaignQueryService.Handle(new GetAllCampaignsQuery(user.Id)).Result.Count();

            // Próximas campañas
            var nextCampaigns = campaignQueryService
                .Handle(new GetAllCampaignsQuery(user.Id))
                .Result
                .Where(c => c.StartDate >= DateTime.Now)
                .Select(c => new CampaignInfoResource(c.Id, c.Name, c.StartDate))
                .ToArray();

            // Total de vacunas
            var totalVaccinations = vaccineQueryService.CountVaccinesByUserIdAsync(new RanchUserId(user.Id)).Result;

            var userInfoResource = new Resources.UserInfoResource(
                user.Id,
                user.Username,
                totalBovines,
                totalCampaigns,
                totalStables,
                totalVaccinations,
                nextCampaigns
            );

            return Ok(userInfoResource);
        }

    }
}