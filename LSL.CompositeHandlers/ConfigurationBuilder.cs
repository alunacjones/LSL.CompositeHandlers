using System;

namespace LSL.CompositeHandlers
{
    public class ConfigurationBuilder<TContext, TResult> : IConfigurationBuilder<TContext, TResult>
    {
        public IConfigurationBuilder<TContext, TResult> WithDefaultHandler(Func<TContext, TResult> defaultHandler)
        {
            DefaultHandler = defaultHandler;
            return this;
        }

        public Func<TContext, TResult> DefaultHandler { get; set; }
    }
}