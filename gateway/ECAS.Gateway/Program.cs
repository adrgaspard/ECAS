namespace ECAS.Gateway;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
        _ = builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
        WebApplication? app = builder.Build();
        _ = app.MapReverseProxy();
        app.Run();
    }
}