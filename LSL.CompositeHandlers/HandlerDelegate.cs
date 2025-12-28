using System;
using System.Threading.Tasks;

namespace LSL.CompositeHandlers 
{
    /// <summary>
    /// Handler delegate definition
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public delegate TResult HandlerDelegate<in TContext, TResult>(TContext context, Func<TResult> next);

    /// <summary>
    /// Contextual handler delegate definition
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public delegate TResult ContextualHandlerDelegate<TContext, TResult>(TContext context, Func<TContext, TResult> next);

    /// <summary>
    /// Async Handler delegate definition
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public delegate Task<TResult> AsyncHandlerDelegate<in TContext, TResult>(TContext context, Func<Task<TResult>> next);

    /// <summary>
    /// Async contextual handler delegate definition
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public delegate TResult AsyncContextualHandlerDelegate<TContext, TResult>(TContext context, Func<TContext, Task<TResult>> next);    
}
