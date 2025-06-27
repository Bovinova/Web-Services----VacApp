using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacApp_Bovinova_Platform.IAM.Application.OutBoundServices;
using VacApp_Bovinova_Platform.IAM.Infrastructure.Tokens.Google.Services;
using VacApp_Bovinova_Platform.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace VacApp_Bovinova_Platform.IAM.Interfaces.REST;

[Route("api/v1/[controller]")]
[ApiController]
public class UserGoogleController(IGoogleAuthorization googleAuthorization,
    AppDbContext context, IConfiguration configuration) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Authorize() => 
        Ok(googleAuthorization.GetAuthorizationUrl());
    
    [HttpGet("callback")]
    [AllowAnonymous]
    public async Task<IActionResult> SignInGoogle(string code)
    {
        var userCredential = await googleAuthorization.ExchangeCodeForToken(code);
        var _credential = await context.Credentials.
            FirstOrDefaultAsync(c=>c.AccessToken == userCredential.Token.AccessToken);
        //Return to the web frontend with the access token
        return Redirect($"https://googleauthhandlerapp.azurewebsites.net/connect-google/{_credential.UserId}");
        //return Redirect($"http://localhost:5173/connect-google/{_credential.UserId}");

    }


    [HttpGet("sign-in/{userId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAccessToken(string userId)
    {
        Guid _userId;
        try
        {
            _userId = Guid.Parse(userId);
        }
        catch (FormatException)
        {
            return Unauthorized();
        }

        var credential = await context.Credentials
            .FirstOrDefaultAsync(c => c.UserId == _userId);

        if (credential == null)
            return Unauthorized();

        // Usa la misma clave secreta que en la configuración
        var secret = configuration["TokenSettings:Secret"];
        var jwt = JwtGenerator.GenerateJwt(credential.UserId.ToString(), secret);

        var result = new
        {
            accessTokenForGoogleServices = credential.AccessToken,
            userId = credential.UserId.ToString(),
            token = jwt
        };

        return Ok(result);
    }
}