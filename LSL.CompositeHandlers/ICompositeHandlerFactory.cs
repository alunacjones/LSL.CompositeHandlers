using System;
using System.Collections.Generic;

namespace LSL.CompositeHandlers
{
    public interface ICompositeHandlerFactory
    {
        Func<TContext, TResult> Create<TContext, TResult>(
            IEnumerable<HandlerDelegate<TContext, TResult>> handlers,
            Action<IConfigurationBuilder<TContext, TResult>> configurator = null);
    }    
}