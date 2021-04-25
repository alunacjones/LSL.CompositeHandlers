using System.Collections.Generic;
using System.Linq;

namespace LSL.CompositeHandlers
{
    /// <summary>
    /// Static helper class to for simple conversions to <see cref="IEnumerable{T}" /> where T is a <see cref="LSL.CompositeHandlers.HandlerDelegate{ TContext, TResult }" />
    /// </summary>
    public static class For
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handlers">An enumerable of handlers for the specific context type and result type</param>
        /// <typeparam name="TContext">The Type of the context the generic handler uses</typeparam>
        /// <typeparam name="TResult">The result Type of the generic handler</typeparam>
        /// <returns></returns>
        public static IEnumerable<HandlerDelegate<TContext, TResult>> GenericHandlers<TContext, TResult>(IEnumerable<IGenericHandler<TContext, TResult>> handlers) =>
            handlers.Select(h => new HandlerDelegate<TContext, TResult>(h.Handle));
    }
}