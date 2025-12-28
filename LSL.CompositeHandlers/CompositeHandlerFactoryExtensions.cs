using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            Action<IConfigurationBuilder<TContext, TResult>> configurator = null) =>
            source.CreateCompositeHandler(handlers, configurator).Handler;

        /// <summary>
        /// Builds a contextual composite handler
        /// </summary>
        /// <param name="source"></param>
        /// <param name="handlers"></param>
        /// <param name="configurator"></param>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns>The <see cref="BuildCompositeHandlerResult{TContext, TResult}"/> that encapsulates result of the build operation</returns>
        public static BuildCompositeHandlerResult<TContext, TResult> CreateCompositeHandler<TContext, TResult>(
                this ICompositeHandlerFactory source,
                IEnumerable<ContextualHandlerDelegate<TContext, TResult>> handlers,
                Action<IConfigurationBuilder<TContext, TResult>> configurator = null)
        {
            var configuration = new ConfigurationBuilder<TContext, TResult>();

            configurator?.Invoke(configuration);

            return new BuildCompositeHandlerResult<TContext, TResult>(
                handlers
                    .Reverse()
                    .Aggregate(
                        configuration.DefaultHandler ?? (_ => default),
                        (compositeFunction, currentFn) => context => currentFn(context, c => compositeFunction(c))
                    )
            );
        }

        /// <summary>
        /// Builds an async contextual composite handler
        /// </summary>
        /// <param name="source"></param>
        /// <param name="handlers"></param>
        /// <param name="configurator"></param>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns>The <see cref="BuildCompositeHandlerResult{TContext, TResult}"/> that encapsulates result of the build operation</returns>
        public static BuildAsyncCompositeHandlerResult<TContext, TResult> CreateAsyncCompositeHandler<TContext, TResult>(
            this ICompositeHandlerFactory source,
            IEnumerable<AsyncContextualHandlerDelegate<TContext, TResult>> handlers,
            Action<IAsyncConfigurationBuilder<TContext, TResult>> configurator = null)
            where TResult : Task<TResult>
        {
            var configuration = new AsyncConfigurationBuilder<TContext, TResult>();

            configurator?.Invoke(configuration);

            return new BuildAsyncCompositeHandlerResult<TContext, TResult>(
                handlers
                    .Reverse()
                    .Aggregate(
                        configuration.DefaultHandler ?? (_ => Task.FromResult(default(TResult))),
                        (compositeFunction, currentFn) => async context => await currentFn(
                            context, 
                            async c => await compositeFunction(c).ConfigureAwait(false))
                            .ConfigureAwait(false)
                    )
            );
        }
    }
}