using System;
using System.Collections.Generic;

namespace LSL.CompositeHandlers;

/// <summary>
/// Factory for creating a composite handler
/// </summary>
public interface ICompositeHandlerFactory
{
    /// <summary>
    /// Builds a composite handler
    /// </summary>
    /// <param name="handlers"></param>
    /// <param name="configurator"></param>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns>The <see cref="BuildCompositeHandlerResult{TContext, TResult}"/> that encapsulates result of the build operation</returns>
    BuildCompositeHandlerResult<TContext, TResult> CreateCompositeHandler<TContext, TResult>(
        IEnumerable<HandlerDelegate<TContext, TResult>> handlers,
        Action<IConfigurationBuilder<TContext, TResult>> configurator = null);
}