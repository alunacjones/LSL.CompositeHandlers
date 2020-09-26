[![Build status](https://img.shields.io/appveyor/ci/alunacjones/lsl-compositehandlers.svg)](https://ci.appveyor.com/project/alunacjones/lsl-compositehandlers)
[![codecov](https://codecov.io/gh/alunacjones/LSL.CompositeHandlers/branch/master/graph/badge.svg)](https://codecov.io/gh/alunacjones/LSL.CompositeHandlers)
[![NuGet](https://img.shields.io/nuget/v/LSL.CompositeHandlers.svg)](https://www.nuget.org/packages/LSL.CompositeHandlers/)

# LSL.CompositeHandlers

A factory for building a compound function using "next" semantics similar to node.js express middleware.

## Quick Start

```csharp
var factory = new CompositeHandlerFactory();
var handler = factory.Create(new HandlerDelegate<int, string>[] { 
    (context, next) => context == 2 ? "We stop on the number 2" : next(),
    (context, next) => context < 10 ? $"Doubled value {context * 2}": next()
},
cfg => cfg.WithDefaultHandler(context => $"Reached the end with {context}"));

Console.WriteLine(handler(1)); // Returns "Doubled value 2" 
Console.WriteLine(handler(2)); // Returns "We stop on the number 2"
Console.WriteLine(handler(12)); // Returns "Reached the end with 12"
```
