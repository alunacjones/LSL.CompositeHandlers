using System;
using System.Threading.Tasks;

namespace LSL.CompositeHandlers
{
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

    /// <inheritdoc/>
    public sealed class AsyncConfigurationBuilder<TContext, TResult> : IAsyncConfigurationBuilder<TContext, TResult>
    {
        /// <inheritdoc/>
        public Func<TContext, Task<TResult>> DefaultHandler { get; private set; }

        /// <inheritdoc/>
        public IAsyncConfigurationBuilder<TContext, TResult> WithDefaultHandler(Func<TContext, Task<TResult>> defaultHandler)
        {
            DefaultHandler = defaultHandler;
            return this;
        }
    }
}