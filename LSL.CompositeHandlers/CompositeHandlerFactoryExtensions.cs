using System;
using System.Collections.Generic;

namespace LSL.CompositeHandlers
{
    /// <summary>
    /// Extensions to the ICompositeHandlerFactory interface
    /// </summary>
    public static class CompositeHandlerFactoryExtensions
    {
        /// <summary>
        /// Generate a compound handler and return the function from the BuildCompositeHandlerResult
        /// </summary>
        /// <param name="source">The <see cref="ICompositeHandlerFactory"/> instance</param>
        /// <param name="handlers">An enumerable of handlers that the resultant composite handler will call</param>
        /// <param name="configurator">A delegate that configures the <see cref="ICompositeHandlerFactory" /> that creates the composite handler</param>
        /// <typeparam name="TContext">The Type of the context that the composite handler will receive as its input</typeparam>
        /// <typeparam name="TResult">The Type of the handler's result</typeparam>
        /// <returns>The composite delegate</returns>
        public static Func<TContext, TResult> Create<TContext, TResult>(
            this ICompositeHandlerFactory source,
            IEnumerable<HandlerDelegate<TContext, TResult>> handlers,
            Action<IConfigurationBuilder<TContext, TResult>> configurator = null) 
            {
                return source.CreateCompositeHandler<TContext, TResult>(handlers, configurator).Handler;
            }  
    }
}