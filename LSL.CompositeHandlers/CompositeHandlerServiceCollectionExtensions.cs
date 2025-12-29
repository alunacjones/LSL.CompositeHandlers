using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LSL.CompositeHandlers;

/// <summary>
/// Composite handler service collection extensions
/// </summary>
public static class CompositeHandlerServiceCollectionExtensions
{
    /// <summary>
    /// Adds the composite handler services
    /// </summary>
    /// <remarks>
    /// <para>
    /// The following services are registered:
    /// </para>
    /// <list type="bullet">
    ///     <item><see cref="ICompositeHandlerFactory"/></item>
    /// </list>
    /// </remarks>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IServiceCollection AddCompositeHandlerServices(this IServiceCollection source)
    {
        source.TryAddSingleton<ICompositeHandlerFactory, CompositeHandlerFactory>();
        return source;
    }
}