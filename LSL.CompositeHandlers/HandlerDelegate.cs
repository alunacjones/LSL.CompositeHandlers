using System;

namespace LSL.CompositeHandlers 
{
    public delegate TResult HandlerDelegate<in TContext, TResult>(TContext context, Func<TResult> next);
}
