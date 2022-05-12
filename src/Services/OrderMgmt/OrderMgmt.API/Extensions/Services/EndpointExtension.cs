namespace OrderMgmt.API.Extensions.Services;

public static class EndpointExtension
{
    public static IEndpointRouteBuilder Redirect(
        this IEndpointRouteBuilder endpoints,
        string from, string to)
    {
        return Redirect(endpoints,
            new Redirective(from, to));
    }

    public static IEndpointRouteBuilder RedirectPermanent(
        this IEndpointRouteBuilder endpoints,
        string from, string to)
    {
        return Redirect(endpoints,
            new Redirective(from, to, true));
    }

    public static IEndpointRouteBuilder Redirect(
        this IEndpointRouteBuilder endpoints,
        params Redirective[] paths)
    {
        foreach (var (from, to, permanent) in paths)
        {
            endpoints.MapGet(from, http =>
            {
                http.Response.Redirect(to, permanent);
                
                return Task.CompletedTask;
            });
        }

        return endpoints;
    }
}

public record Redirective(string From, string To, bool Permanent = false);