using System;

namespace LSL.CompositeHandlers 
{
    public delegate TResult HandlerDelegate<out TContext, in TResult>(TContext context, Func<TResult> next);
}
