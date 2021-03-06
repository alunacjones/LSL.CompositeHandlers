﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace LSL.CompositeHandlers.Tests
{
    public class CompositeHandlerFactoryTests
    {
        private CompositeHandlerFactory BuildSut()
        {
            return new CompositeHandlerFactory();
        }

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
                For.GenericHandlers(new IGenericHandler<string, int>[] { new GenericHandler1(), new GenericHandler2() })
            )(input)
            .Should()
            .Be(expectedResult);
        }

        private string MapRecording(List<string> recorder)
        {
            return string.Join(" => ", recorder);
        }

        private IEnumerable<HandlerDelegate<string, bool>> BuildHandlers(string pipelineDefinitions, IList<string> recorder)
        {
            return pipelineDefinitions
                .Split('|')
                .Select(pd => pd.Trim())
                .Where(pd => !string.IsNullOrEmpty(pd))
                .Select(pd => new
                {
                    CallNext = pd.IndexOf("callNext", StringComparison.InvariantCultureIgnoreCase) > -1,
                    Execute = pd.IndexOf("execute", StringComparison.InvariantCultureIgnoreCase) > -1
                })
                .Select((h, i) => new HandlerDelegate<string, bool>((s, next) =>
                {
                    var stageRecording = (i + 1).ToString();
                    if (h.Execute)
                    {
                        stageRecording += " (executed)";
                    }

                    recorder.Add(stageRecording);

                    return !h.CallNext || next();
                }))
                .ToList();
        }

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