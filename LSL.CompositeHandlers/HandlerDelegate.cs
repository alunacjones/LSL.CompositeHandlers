using System;

namespace LSL.CompositeHandlers 
{
    public delegate TResult HandlerDelegate<TContext, TResult>(TContext context, Func<TResult> next);
}
