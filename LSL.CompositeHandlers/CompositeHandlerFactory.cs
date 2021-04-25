using System;
using System.Collections.Generic;
using System.Linq;

namespace LSL.CompositeHandlers
{
    /// <inheritdoc/>
    public class CompositeHandlerFactory : ICompositeHandlerFactory
    {
        /// <inheritdoc/>
        public BuildCompositeHandlerResult<TContext, TResult> CreateCompositeHandler<TContext, TResult>(
            IEnumerable<HandlerDelegate<TContext, TResult>> handlers,
            Action<IConfigurationBuilder<TContext, TResult>> configurator = null)
        {
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