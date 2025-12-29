using System;

namespace LSL.CompositeHandlers;

/// <summary>
/// Result of the composite handler factory Build method
/// </summary>
/// <typeparam name="TContext">The Type of the context object that will be passed into the generated handler</typeparam>
/// <typeparam name="TResult">The Type of the result that will be returned by the composite handler</typeparam>
public sealed class BuildCompositeHandlerResult<TContext, TResult>
{
    internal BuildCompositeHandlerResult(Func<TContext, TResult> handler)
    {
        Handler = handler;
    }

    /// <summary>
    /// The composite handler delegate
    /// </summary>
    /// <value></value>
    public Func<TContext, TResult> Handler { get; }
}