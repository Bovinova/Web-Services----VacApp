using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using VacApp_Bovinova_Platform.IAM.Domain.Model.Queries;
using VacApp_Bovinova_Platform.IAM.Domain.Services;
using VacApp_Bovinova_Platform.IAM.Interfaces.REST.Resources;
using VacApp_Bovinova_Platform.IAM.Interfaces.REST.Transform;
using VacApp_Bovinova_Platform.IAM.Infrastructure.Pipeline.Middleware.Attributes;


namespace VacApp_Bovinova_Platform.IAM.Interfaces.REST
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Tags("User")]
    public class UserController(IUserCommandService commandService,
        IUserQueryService queryService) : ControllerBase
    {
        [HttpPost("sign-up")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpResource resource)
        {
            var command = SignUpCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await commandService.Handle(command);

            if (result is null) return BadRequest("User already exists");

            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(result, resource.Username, resource.Email);

            return CreatedAtAction(nameof(SignUp), userResource);
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
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
            /*
            var email = !string.IsNullOrEmpty(resource.Email)
                ? resource.Email
                : await queryService.GetEmailByUserName(resource.UserName!);*/

            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(result, userName,  resource.Email ?? string.Empty);

            return Ok(userResource);
        }
    }
}