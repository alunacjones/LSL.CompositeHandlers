using System;
using System.Collections.Generic;
using System.Linq;

namespace LSL.CompositeHandlers
{
    public class CompositeHandlerFactory : ICompositeHandlerFactory
    {
        public Func<TContext, TResult> Create<TContext, TResult>(
            IEnumerable<HandlerDelegate<TContext, TResult>> handlers,
            Action<IConfigurationBuilder<TContext, TResult>> configurator = null)
        {
            var configuration = new ConfigurationBuilder<TContext, TResult>();

            configurator?.Invoke(configuration);

            return handlers
                .Reverse()
                .Aggregate(
                    configuration.DefaultHandler ?? (_ => default(TResult)),
                    (compositeFunction, currentFn) => context => currentFn(context, () => compositeFunction(context))
                );
        }
    }
}