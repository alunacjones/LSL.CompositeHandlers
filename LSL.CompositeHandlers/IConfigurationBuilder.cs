using System;

namespace LSL.CompositeHandlers;

/// <summary>
/// A builder to configure the ICompositeHandlerFactory
/// </summary>
/// <typeparam name="TContext"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IConfigurationBuilder<TContext, in TResult>
{
    /// <summary>
    /// The default handler if nothing can handle the current context
    /// </summary>
    /// <param name="defaultHandler"></param>
    /// <returns>The IConfigurationBuilder instance that is currently being used to build the settings</returns>
    IConfigurationBuilder<TContext, TResult> WithDefaultHandler(Func<TContext, TResult> defaultHandler);
}