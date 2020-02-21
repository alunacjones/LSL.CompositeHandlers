using System;

namespace LSL.CompositeHandlers 
{
    public delegate TResult HandlerDelegate<in TContext, out TResult>(TContext context, Func<TResult> next);
}
