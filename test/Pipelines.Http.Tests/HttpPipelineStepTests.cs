using System;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Pipelines.Http.Tests
{
    public class HttpPipelineStepTests
    {
        [Fact]
        public void ShouldInvokeGivenAction()
        {
            var act = new Mock<Action<IApplicationBuilder>>();
            var mockedApp = new Mock<IApplicationBuilder>();
            
            var sut = new HttpPipelineStep("s1", act.Object);
            sut.Invoke(mockedApp.Object);
            
            act.Verify(x=>x.Invoke(It.Is<IApplicationBuilder>(x=> x == mockedApp.Object)));
        }
        
        [Fact]
        public void ShouldThrowExceptionIfGivenActionIsNull()
        {
            Action act = () => new HttpPipelineStep("s1", null);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("appBuilder");
        }
        
        
        [Theory]
        [InlineData(null)]
        [InlineData("  ")]
        [InlineData("")]
        public void ShouldThrowExceptionIfGivenKeyIsNullEmptyOrWhitespace(string key)
        {
            Action act = () => new HttpPipelineStep(key, app => {});

            act.Should().Throw<ArgumentException>().And.ParamName.Should().Be("key");
        }
        
        [Fact]
        public void InheritedStepsFromHttpPipelineStepBaseGenericShouldUseTypeFullNameAsKey()
        {
            var step = new SampleHttpPipelineStep();

            step.Key.Should().Be(typeof(SampleHttpPipelineStep).FullName);
        }
        
        [Fact]
        public void InheritedStepsFromHttpPipelineStepBaseGenericShouldHaveStaticKeyWithTypeFullName()
        {
            SampleHttpPipelineStep.StaticKey.Should().Be(typeof(SampleHttpPipelineStep).FullName);
        }
    }
}