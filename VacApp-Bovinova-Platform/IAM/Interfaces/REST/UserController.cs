using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VacApp_Bovinova_Platform.CampaignManagement.Domain.Model.Queries;
using VacApp_Bovinova_Platform.CampaignManagement.Domain.Services;
using VacApp_Bovinova_Platform.IAM.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.IAM.Domain.Model.Queries;
using VacApp_Bovinova_Platform.IAM.Domain.Services;
using VacApp_Bovinova_Platform.IAM.Interfaces.REST.Resources;
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
    [Tags("User")]
    public class UserController(
        IUserCommandService commandService,
        IUserQueryService queryService,
        IBovineQueryService bovineQueryService,
        ICampaignQueryService campaignQueryService,
        IVaccineQueryService vaccineQueryService,
        IStableQueryService stableQueryService
        ) : ControllerBase
    {
        
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
        
        [HttpGet("get-info")]
        [SwaggerResponse(StatusCodes.Status200OK, "User info", typeof(UserInfoResource))]
        public async Task<ActionResult> GetInfo()
        {
            // Get user ID from JWT claims
            var userIdClaim = User.FindFirst(ClaimTypes.Sid)?.Value;


            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user ID");

            // Use the query handler to get the user by ID
            var user = await queryService.Handle(new GetUserByIdQuery(userId));
            if (user is null)
                return NotFound("User not found");

            // Get bovine count
            var totalBovines = await bovineQueryService.CountBovinesByUserIdAsync(new RanchUserId(userId));
            
            // Get campaign count
            //var totalCampaigns = await campaignQueryService.CountCampaignsByUserIdAsync(new CampaignUserId(userId));
            
            // Get vaccine count
            var totalVaccinations = await vaccineQueryService.CountVaccinesByUserIdAsync(new RanchUserId(userId));
            
            // Get vaccine count
            var totalStables = await stableQueryService.CountStablesByUserIdAsync(new RanchUserId(userId));


            // Build and return the response
            var resource = new UserInfoResource(user.Username, totalBovines, totalVaccinations, totalStables);
            return Ok(resource);
        }
    }
}