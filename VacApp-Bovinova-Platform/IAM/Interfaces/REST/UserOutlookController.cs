using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacApp_Bovinova_Platform.IAM.Application.OutBoundServices;
using VacApp_Bovinova_Platform.IAM.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.IAM.Infrastructure.Tokens.Google.Services;
using VacApp_Bovinova_Platform.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace VacApp_Bovinova_Platform.IAM.Interfaces.REST;

[Route("api/v1/[controller]")]
[ApiController]
public class UserOutlookController(IMicrosoftAuthorization microsoftAuthorization,
    AppDbContext context, IConfiguration configuration) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Authorize() =>
        Ok(microsoftAuthorization.GetAuthorizationUrl());

    [HttpGet("callback")]
    [AllowAnonymous]
    public async Task<IActionResult> SignInOutlook(string code)
    {
        var userCredential = await microsoftAuthorization.ExchangeCodeForToken(code);
        
        var existingCredential = await context.MicrosoftCredentials
            .FirstOrDefaultAsync(c => c.AccessToken == userCredential.AccessToken);

        if (existingCredential == null)
        {
            var newCredential = new MicrosoftCredential
            {
                UserId = Guid.NewGuid(),
                AccessToken = userCredential.AccessToken,
                RefreshToken = userCredential.RefreshToken,
                Email = userCredential.Email
            };

            context.MicrosoftCredentials.Add(newCredential);
            await context.SaveChangesAsync();

            existingCredential = newCredential;
        }

        // Redirige al frontend con el ID del usuario
        //return Redirect($"http://localhost:5173/connect-microsoft/{existingCredential.UserId}");
        return Redirect($"https://googleauthhandlerapp.azurewebsites.net/connect-microsoft/{existingCredential.UserId}");
    }

    [HttpGet("sign-in/{userId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAccessToken(string userId)
    {
        if (!Guid.TryParse(userId, out Guid _userId))
            return Unauthorized();

        var credential = await context.MicrosoftCredentials
            .FirstOrDefaultAsync(c => c.UserId == _userId);

        if (credential == null)
            return Unauthorized();

        var secret = configuration["TokenSettings:Secret"];
        var jwt = JwtGenerator.GenerateJwt(credential.UserId.ToString(), secret);

        var result = new
        {
            accessTokenForMicrosoftServices = credential.AccessToken,
            userId = credential.UserId.ToString(),
            token = jwt
        };

        return Ok(result);
    }
}
