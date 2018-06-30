using System;
using System.Collections.Generic;

namespace LSL.CompositeHandlers
{
    public interface ICompositeHandlerFactory
    {
        Func<TContext, TResult> Create<TContext, TResult>(
            IEnumerable<Func<TContext, Func<TResult>, TResult>> handlers,
            Action<IConfigurationBuilder<TContext, TResult>> configurator = null);
    }
}