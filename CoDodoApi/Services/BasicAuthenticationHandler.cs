using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace CoDodoApi.Services;

public class BasicAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder) 
    : 
    AuthenticationHandler<AuthenticationSchemeOptions>(
        options: options, 
        logger: logger, 
        encoder: encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authHeader = Request.Headers.Authorization.ToString();

        if (!authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            return Fail();

        var credentials = Credentials(authHeader);

        // todo: make this configurable
        if (credentials[0] != "admin" || credentials[1] != "password")
            return Fail();

        return Success(credentials);
    }

    private static string[] Credentials(string authHeader)
    {
        string token = authHeader["Basic ".Length..].Trim();

        return Encoding.UTF8.GetString(Convert.FromBase64String(token))
            .Split(':');
    }

    private Task<AuthenticateResult> Success(string[] credentials)
    {
        Claim name = new ("name", credentials[0]);
        Claim role = new (ClaimTypes.Role, "Admin");

        Claim[] claims = [name, role];

        ClaimsIdentity identity = new(claims, "Basic");
        ClaimsPrincipal claimsPrincipal = new(identity);

        AuthenticationTicket ticket = new(claimsPrincipal, Scheme.Name);
        AuthenticateResult success = AuthenticateResult.Success(ticket);

        return Task.FromResult(success);
    }

    private Task<AuthenticateResult> Fail()
    {
        Response.StatusCode = 401;
        Response.Headers.Append("WWW-Authenticate", 
            """
            Basic realm="CoDodoApiRealm"
            """);

        return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
    }
}