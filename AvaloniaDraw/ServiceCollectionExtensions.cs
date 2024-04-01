using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaDraw;

public static class ServiceCollectionExtensions
{
    public static void AddDrawingServices(this IServiceCollection services)
    {
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ToolboxViewModel>();
        services.AddTransient<DrawingCanvasViewModel>();
    }
}