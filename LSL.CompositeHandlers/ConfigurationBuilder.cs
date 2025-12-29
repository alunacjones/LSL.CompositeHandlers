using System;

namespace LSL.CompositeHandlers;

/// <inheritdoc/>
public sealed class ConfigurationBuilder<TContext, TResult> : IConfigurationBuilder<TContext, TResult>
{
    /// <inheritdoc/>
    public IConfigurationBuilder<TContext, TResult> WithDefaultHandler(Func<TContext, TResult> defaultHandler)
    {
        DefaultHandler = defaultHandler;
        return this;
    }

    /// <inheritdoc/>
    public Func<TContext, TResult> DefaultHandler { get; set; }
}