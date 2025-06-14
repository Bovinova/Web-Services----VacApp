using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VacApp_Bovinova_Platform.IAM.Application.OutBoundServices;
using VacApp_Bovinova_Platform.IAM.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace VacApp_Bovinova_Platform.IAM.Infrastructure.OAuth.Google.TokenHandler;

public class GoogleAccessTokenAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> 
        options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        TimeProvider timeProvider,
        IGoogleAuthorization googleAuthorization,
        AppDbContext context) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private readonly TimeProvider _timeProvider = timeProvider;
    

    protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("Missing Authorization Header");

        string authHeader = Request.Headers.Authorization!;
        if(!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return AuthenticateResult.Fail("Invalid Authorization Header");

        string accessToken = authHeader["Bearer ".Length..].Trim();
        var userCredential = await googleAuthorization.ValidateToken(accessToken);
        Credential? user = await GetUserCredential(userCredential.Token.AccessToken);
        if(user == null) AuthenticateResult.Fail("Invalid Access Token Provided");
        
        List<Claim> claims = [new Claim(ClaimTypes.NameIdentifier, user!.UserId.ToString())];
        var identity = new ClaimsIdentity(claims, Constant.Scheme);
        return AuthenticateResult.Success(
            new AuthenticationTicket(
                new ClaimsPrincipal(identity), Constant.Scheme));
    }

    private async Task<Credential?> GetUserCredential(string tokenAccessToken) =>
    await context.Credentials.FirstOrDefaultAsync(c=>c.AccessToken == tokenAccessToken);
}