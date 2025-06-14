using System.Reflection.Metadata;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacApp_Bovinova_Platform.IAM.Application.OutBoundServices;
using VacApp_Bovinova_Platform.IAM.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.IAM.Infrastructure.Tokens.Google.Configuration;
using VacApp_Bovinova_Platform.Shared.Infrastructure.Persistence.EFC.Configuration;
using Constant = VacApp_Bovinova_Platform.IAM.Infrastructure.OAuth.Google.TokenHandler.Constant;

namespace VacApp_Bovinova_Platform.IAM.Interfaces.REST;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthorizeController(IGoogleAuthorization googleAuthorization,
    AppDbContext context,
    ITokenService tokenService) : ControllerBase
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
        return Redirect($"https://localhost:7272/connect/{_credential.UserId}");
    }

    [HttpGet("token/{userId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAccessToken(string userId)
    {
        Guid _userId = Guid.Empty;
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
        return Ok(JsonSerializer.Serialize
            (new Token(credential!.AccessToken, credential.UserId.ToString())));
    }
}