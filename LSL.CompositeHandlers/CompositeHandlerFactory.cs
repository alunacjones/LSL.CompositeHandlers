using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSL.CompositeHandlers
{
    /// <inheritdoc/>
    public class CompositeHandlerFactory : ICompositeHandlerFactory
    {
        /// <inheritdoc/>
        public BuildAsyncCompositeHandlerResult<TContext, TResult> CreateAsyncCompositeHandler<TContext, TResult>(IEnumerable<AsyncHandlerDelegate<TContext, TResult>> handlers, Action<IAsyncConfigurationBuilder<TContext, TResult>> configurator = null)
        {
            var configuration = new AsyncConfigurationBuilder<TContext, TResult>();

            configurator?.Invoke(configuration);

            return new BuildAsyncCompositeHandlerResult<TContext, TResult>(
                handlers
                    .Reverse()
                    .Aggregate(
                        configuration.DefaultHandler ?? (_ => Task.FromResult(default(TResult))),
                        (compositeFunction, currentFn) => async context => await currentFn(context, async () => await compositeFunction(context))
                    )
            );            
        }

        /// <inheritdoc/>
        public BuildCompositeHandlerResult<TContext, TResult> CreateCompositeHandler<TContext, TResult>(
            IEnumerable<HandlerDelegate<TContext, TResult>> handlers,
            Action<IConfigurationBuilder<TContext, TResult>> configurator = null)
        {
            if (typeof(Task<>).IsAssignableFrom(typeof(TResult)))
            {
                throw new ArgumentException("TResult is a Task. Please use the CreateAsyncCompositeHandler method instead");
            }
            
            var configuration = new ConfigurationBuilder<TContext, TResult>();

            configurator?.Invoke(configuration);
            
            return new BuildCompositeHandlerResult<TContext, TResult>(
                handlers
                    .Reverse()
                    .Aggregate(
                        configuration.DefaultHandler ?? (_ => default(TResult)),
                        (compositeFunction, currentFn) => context => currentFn(context, () => compositeFunction(context))
                    )
            );
        }
    }
}