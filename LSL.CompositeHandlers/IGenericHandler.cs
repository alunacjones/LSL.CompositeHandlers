using System;

namespace LSL.CompositeHandlers
{
    /// <summary>
    /// Interface that handlers can be based off for a given TContext and TResult
    /// </summary>
    /// <typeparam name="TContext">Type for the context the handler expects</typeparam>
    /// <typeparam name="TResult">Type that should be the result of the handler</typeparam>
    public interface IGenericHandler<TContext, TResult>
    {
        /// <summary>
        /// Handler for a given context. If it cannot handle the context it calls the next function
        /// </summary>
        /// <param name="context">The handler's context object</param>
        /// <param name="next">The next function to call if the current handler cannot process the context. The result of this function must then be ultimately returned</param>
        /// <returns></returns>
        TResult Handle(TContext context, Func<TResult> next);
    }
}