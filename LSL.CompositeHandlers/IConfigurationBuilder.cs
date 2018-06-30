
using System;

namespace LSL.CompositeHandlers
{
    public interface IConfigurationBuilder<TContext, in TResult>
    {
        IConfigurationBuilder<TContext, TResult> WithDefaultHandler(Func<TContext, TResult> defaultHandler);
    }
}