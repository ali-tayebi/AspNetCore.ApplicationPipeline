using System;
using ApplicationPipeline;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Moq;
using Xunit;

namespace ApplicationPipeline.Tests
{
    public class ApplicationPipelineStepTests
    {
        [Fact]
        public void ShouldInvokeGivenAction()
        {
            var act = new Mock<Action<IApplicationBuilder>>();
            var mockedApp = new Mock<IApplicationBuilder>();

            var sut = new ApplicationPipelineStep("s1", act.Object);
            sut.Invoke(mockedApp.Object);

            act.Verify(x=>x.Invoke(It.Is<IApplicationBuilder>(x=> x == mockedApp.Object)));
        }

        [Fact]
        public void ShouldThrowExceptionIfGivenActionIsNull()
        {
            Action act = () => new ApplicationPipelineStep("s1", null);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("appBuilder");
        }


        [Theory]
        [InlineData(null)]
        [InlineData("  ")]
        [InlineData("")]
        public void ShouldThrowExceptionIfGivenKeyIsNullEmptyOrWhitespace(string key)
        {
            Action act = () => new ApplicationPipelineStep(key, app => {});

            act.Should().Throw<ArgumentException>().And.ParamName.Should().Be("key");
        }

        [Fact]
        public void InheritedStepsFromHttpPipelineStepBaseGenericShouldUseTypeFullNameAsKey()
        {
            var step = new SampleApplicationPipelineStep();

            step.Key.Should().Be(typeof(SampleApplicationPipelineStep).FullName);
        }

        [Fact]
        public void InheritedStepsFromHttpPipelineStepBaseGenericShouldHaveStaticKeyWithTypeFullName()
        {
            SampleApplicationPipelineStep.StaticKey.Should().Be(typeof(SampleApplicationPipelineStep).FullName);
        }
    }
}
