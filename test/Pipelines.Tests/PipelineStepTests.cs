using System;
using FluentAssertions;
using Xunit;

namespace Pipelines.Tests
{
    public class PipelineStepTests
    {
        [Fact]
        public void PipelineStepsNameShouldNotBeNull()
        {
            Action act = () => new SamplePipelineStep(null);

            act.Should().Throw<ArgumentException>().And.ParamName.Should().Be("key");
        }
        
        [Fact]
        public void PipelineStepsNameShouldNotBeEmpty()
        {
            Action act = () => new SamplePipelineStep(string.Empty);

            act.Should().Throw<ArgumentException>().And.ParamName.Should().Be("key");
        }
        
        [Fact]
        public void PipelineStepsNameShouldNotBeWhitespace()
        {
            Action act = () => new SamplePipelineStep("   ");

            act.Should().Throw<ArgumentException>().And.ParamName.Should().Be("key");
        }
        
        [Fact]
        public void PipelineStepsWithTheSameNameAreEqual()
        {
            var step1 = new SamplePipelineStep("step-1");
            var step2 = new SamplePipelineStep("step-1");

            step1.Should().Equals(step2);
            Assert.True(step1 == step2);
        }

        [Fact]
        public void PipelineStepsShouldNotBeEquivalentToNull()
        {
            SamplePipelineStep step1 = new SamplePipelineStep("step-1");
            step1.Equals(null).Should().BeFalse();
            Assert.True(step1 != null);
        }
        
        [Fact]
        public void PipelineStepsWithDifferentNamesAreNotEqual()
        {
            var step1 = new SamplePipelineStep("step-1");
            var step2 = new SamplePipelineStep("step-2");

            step1.Should().NotBeEquivalentTo(step2);
            Assert.True(step1 != step2);
        }
        
        [Fact]
        public void PipelineStepsWithNullNameAreNotEqual()
        {
            var step1 = new SamplePipelineStep("step-1");
            var step2 = new SamplePipelineStep("step-1 ");

            step1.Should().NotBeEquivalentTo(step2);
            Assert.True(step1 != step2);
        }
    }
}