using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using VacApp_Bovinova_Platform.IAM.Domain.Model.Queries;
using VacApp_Bovinova_Platform.IAM.Domain.Services;
using VacApp_Bovinova_Platform.IAM.Infrastructure.Mailing.Net.Services;
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

            // Generate confirmation link
            var confirmationLink = Url.Action(
                "ConfirmEmail",
                "User",
                new { email = resource.Email },
                Request.Scheme
            );

            // Send confirmation email
            var emailService = new EmailService("smtp.gmail.com", 587, resource.Email, "ampy reif ewbd lvhg");
            await emailService.SendEmailAsync(
                resource.Email,
                "Register Confirmation",
                $"Hello {resource.Username}, please confirm your email by clicking the next link: {confirmationLink}"
            );
            
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
            
            var email = !string.IsNullOrEmpty(resource.Email)
                ? resource.Email
                : await queryService.GetEmailByUserName(resource.UserName!);

            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(result, userName, email);

            return Ok(userResource);
        }
        
        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string email)
        {
            var user = await queryService.Handle(new GetUserByEmailQuery(email));

            if (user is null)
            {
                return NotFound("User not found.");
            }

            if (user.EmailConfirmed)
            {
                return BadRequest("Email is already confirmed.");
            }

            user.EmailConfirmed = true;
            await commandService.UpdateUserAsync(user); // Method to update the user in the database

            return Ok($"Email {email} confirmed.");
        }
    }
}