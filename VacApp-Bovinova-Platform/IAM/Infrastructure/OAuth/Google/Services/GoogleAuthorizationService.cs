using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.EntityFrameworkCore;
using VacApp_Bovinova_Platform.IAM.Application.OutBoundServices;
using VacApp_Bovinova_Platform.IAM.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace VacApp_Bovinova_Platform.IAM.Infrastructure.OAuth.Google.Services;

public class GoogleAuthorizationService(
    AppDbContext context, 
    IGoogleAuthHelper googleAuthHelper,
    IConfiguration configuration) : IGoogleAuthorization
{
    private string RedirectUrl = configuration["Google:RedirectUrl"]!;
    
    public async Task<UserCredential> ExchangeCodeForToken(string code)
    {
        var flow = new GoogleAuthorizationCodeFlow(
            new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = googleAuthHelper.GetClientSecrets(),
                Scopes = googleAuthHelper.GetScopes()
            });
        var token = await flow.ExchangeCodeForTokenAsync(
            "user",
            code,
            RedirectUrl,
            CancellationToken.None);
        context.Add(new Credential
        {
            AccessToken = token.AccessToken,
            RefreshToken = token.RefreshToken,
            ExpiresInSeconds = token.ExpiresInSeconds,
            IdToken = token.IdToken,
            UserId = Guid.NewGuid(),
            IssuedUtc = token.IssuedUtc,
        });
        await context.SaveChangesAsync();
        return new UserCredential(flow, "user", token);
    }
    
    public string GetAuthorizationUrl() =>
        new GoogleAuthorizationCodeFlow(
            new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = googleAuthHelper.GetClientSecrets(),
                Scopes = googleAuthHelper.GetScopes(),
                Prompt = "consent"
            }).CreateAuthorizationCodeRequest(RedirectUrl).Build().ToString();
    

    public async Task<UserCredential> ValidateToken(string accessToken)
    {
        var credential = await context.Credentials.FirstOrDefaultAsync(c => c.AccessToken == accessToken) ?? 
                          throw new UnauthorizedAccessException("Invalid access token. Please login again.");
        var flow = new GoogleAuthorizationCodeFlow(
            new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = googleAuthHelper.GetClientSecrets(),
                Scopes = googleAuthHelper.GetScopes()
            });
        var tokenResponse = new TokenResponse
        {
            AccessToken = credential.AccessToken,
            RefreshToken = credential.RefreshToken,
            ExpiresInSeconds = credential.ExpiresInSeconds,
            IdToken = credential.IdToken,
            IssuedUtc = credential.IssuedUtc
        };
        return new UserCredential(flow, "user", tokenResponse);
    }
}