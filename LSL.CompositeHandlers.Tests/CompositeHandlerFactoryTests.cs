using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LSL.ExecuteIf;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LSL.CompositeHandlers.Tests
{
    public class CompositeHandlerFactoryTests
    {
        private static ICompositeHandlerFactory BuildSut() => new CompositeHandlerFactory();
        private static ICompositeHandlerFactory BuildSutFromServiceProvider() =>
            new ServiceCollection()
                .AddCompositeHandlerServices()
                .BuildServiceProvider()
                .GetRequiredService<ICompositeHandlerFactory>();
                
        [TestCase("", "execute callNext | callNext | donothing", "1 (executed) => 2 => 3")]
        [TestCase("", "execute | callNext | donothing", "1 (executed)")]
        [TestCase("", "callNext | callNext | execute callNext", "1 => 2 => 3 (executed)")]
        [TestCase("", "donothing | callNext | execute callNext", "1")]
        [TestCase("", "callNext | execute | execute", "1 => 2 (executed)")]
        [TestCase("", "callNext | execute callNext | donothing", "1 => 2 (executed) => 3")]
        public void GivenASetOfHandlersAndADefaultHandlerThatReturnsTrue_ItShouldFollowTheExpectedPipeline(string dummy, string pipelineDefinition, string expectedExecution)
        {
            var recorder = new List<string>();

            BuildSut()
                .Create(
                    BuildHandlers(pipelineDefinition, recorder),
                    cfg => cfg.WithDefaultHandler(_ => true)
                )("context")
                .Should()
                .Be(true);

            MapRecording(recorder)
                .Should()
                .Be(expectedExecution);
        }

        [TestCase(false, "execute callNext | callNext | donothing", "1 (executed) => 2 => 3", true)]
        [TestCase(false, "execute | callNext | donothing", "1 (executed)", true)]
        [TestCase(false, "callNext | callNext | execute callNext", "1 => 2 => 3 (executed)", false)]
        [TestCase(false, "donothing | callNext | execute callNext", "1", true)]
        [TestCase(false, "callNext | execute | execute", "1 => 2 (executed)", true)]
        [TestCase(false, "callNext | execute callNext | donothing", "1 => 2 (executed) => 3", true)]

        [TestCase(true, "execute callNext | callNext | donothing", "1 (executed) => 2 => 3", true)]
        [TestCase(true, "execute | callNext | donothing", "1 (executed)", true)]
        [TestCase(true, "callNext | callNext | execute callNext", "1 => 2 => 3 (executed)", true)]
        [TestCase(true, "donothing | callNext | execute callNext", "1", true)]
        [TestCase(true, "callNext | execute | execute", "1 => 2 (executed)", true)]
        [TestCase(true, "callNext | execute callNext | donothing", "1 => 2 (executed) => 3", true)]
        public void Contextual_GivenASetOfHandlersAndADefaultHandlerThatReturnsTrue_ItShouldFollowTheExpectedPipeline(
            bool useDefaultHandler,
            string pipelineDefinition,
            string expectedExecution,
            bool expectedResult)
        {
            var recorder = new List<string>();

            BuildSutFromServiceProvider()
                .CreateContextualCompositeHandler(
                    BuildContextualHandlers(pipelineDefinition, recorder),
                    cfg => cfg.ExecuteIf(useDefaultHandler, c => c.WithDefaultHandler(_ => true))
                ).Handler("context")
                .Should()
                .Be(expectedResult);

            MapRecording(recorder)
                .Should()
                .Be(expectedExecution);
        }        

        [TestCase("", "callNext | callNext | execute callNext", "1 => 2 => 3 (executed)")]
        public void GivenASetOfHandlersAndNoDefaultHandlerThatReturnsTrue_ItShouldFollowTheExpectedPipeline(string dummy, string pipelineDefinition, string expectedExecution)
        {
            var recorder = new List<string>();

            BuildSut()
                .Create(
                    BuildHandlers(pipelineDefinition, recorder)
                )("context")
                .Should()
                .Be(false);

            MapRecording(recorder)
                .Should()
                .Be(expectedExecution);
        }

        [TestCase("first", 12)]
        [TestCase("second", 24)]
        [TestCase("other", 0)]
        public void GivenASetOfGenericHandlers_ItShouldReturnThExpectedResult(string input, int expectedResult)
        {
            BuildSut().Create(
                For.GenericHandlers([new GenericHandler1(), new GenericHandler2()])
            )(input)
            .Should()
            .Be(expectedResult);
        }

        private string MapRecording(List<string> recorder)
        {
            return string.Join(" => ", recorder);
        }

        private static IEnumerable<HandlerDelegate<string, bool>> BuildHandlers(string pipelineDefinitions, IList<string> recorder) => 
            [.. BuildPipeline(pipelineDefinitions)
                .Select((h, i) => new HandlerDelegate<string, bool>((s, next) =>
                {
                    var stageRecording = (i + 1).ToString();
                    if (h.Execute)
                    {
                        stageRecording += " (executed)";
                    }

                    recorder.Add(stageRecording);

                    return !h.CallNext || next();
                }))];

        private static IEnumerable<PipelineDefinition> BuildPipeline(string pipelineDefinitions) => 
            pipelineDefinitions
                .Split('|')
                .Select(pd => pd.Trim())
                .Where(pd => !string.IsNullOrEmpty(pd))
                .Select(pd => new PipelineDefinition(
                    pd.IndexOf("callNext", StringComparison.InvariantCultureIgnoreCase) > -1,
                    pd.IndexOf("execute", StringComparison.InvariantCultureIgnoreCase) > -1
                ));

        public record PipelineDefinition(bool CallNext, bool Execute);

        private static IEnumerable<ContextualHandlerDelegate<string, bool>> BuildContextualHandlers(string pipelineDefinitions, IList<string> recorder) =>
            [.. BuildPipeline(pipelineDefinitions)
                .Select((h, i) => new ContextualHandlerDelegate<string, bool>((s, next) =>
                {
                    var stageRecording = (i + 1).ToString();
                    if (h.Execute)
                    {
                        stageRecording += " (executed)";
                    }

                    recorder.Add(stageRecording);

                    return !h.CallNext || next(s);
                }))];

        private class GenericHandler1 : IGenericHandler<string, int>
        {
            public int Handle(string context, Func<int> next)
            {
                if (context == "first") 
                {
                    return 12;
                }

                return next();
            }
        }

        private class GenericHandler2 : IGenericHandler<string, int>
        {
            public int Handle(string context, Func<int> next)
            {
                if (context == "second")
                {
                    return 24;
                }

                return next();
            }
        }
    }
}