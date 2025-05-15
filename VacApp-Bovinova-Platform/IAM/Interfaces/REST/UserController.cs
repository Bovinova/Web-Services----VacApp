using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
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
    public class UserController(IUserCommandService commandService) : ControllerBase
    {
        [HttpPost("sign-up")]
        [AllowAnonymous]
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
        public async Task<ActionResult> SignIn([FromBody] SignInResource resource)
        {
            var command = SignInCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await commandService.Handle(command);

            if (result is null) return BadRequest("Invalid credentials.");

            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(result);

            return Ok(userResource);
        }
    }
}