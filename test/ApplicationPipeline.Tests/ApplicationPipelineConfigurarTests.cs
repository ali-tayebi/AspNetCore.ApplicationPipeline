using System;
using System.Linq.Expressions;
using ApplicationPipeline.Abstractions;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace ApplicationPipeline.Tests
{
    public class ApplicationPipelineConfigurarTests
    {
        private readonly ApplicationPipelineConfigurar _sut;
        private readonly IApplicationPipeline _pipeline;

        private readonly IApplicationPipelineStep _step1;
        private readonly IApplicationPipelineStep _step2;

        public ApplicationPipelineConfigurarTests()
        {
            var services = new ServiceCollection()
                .AddSingleton(new Mock<IWebHostEnvironment>().Object)
                .AddSingleton(new Mock<IDummyService>().Object);

            _sut = new ApplicationPipelineConfigurar(services);
            _pipeline = new AspNetApplicationPipeline();
            _step1 = new ApplicationPipelineStep("step-1", app => { });
            _step2 = new ApplicationPipelineStep("step-2", app => { });
        }

        [Fact]
        public void AppendShouldAddStepsToPipeline()
        {
            _sut
                .Add(_step1)
                .Add(_step2);

            _sut.Configure(_pipeline);

            _pipeline.Steps.Should().ContainInOrder(new[]
            {
                _step1,
                _step2
            });
        }

        [Fact]
        public void AppendGenericShouldAddStepsToPipeline()
        {
            _sut
                .Add(_step1)
                .Add<SampleApplicationPipelineStep>()
                .Add(_step2);

            _sut.Configure(_pipeline);

            _pipeline.KeysInOrder.Should().ContainInOrder(new[]
            {
                _step1.Key,
                SampleApplicationPipelineStep.StaticKey,
                _step2.Key
            });
        }


        [Fact]
        public void AppendGenericShouldUseDependencyInjectionToAddStepsToPipeline()
        {
            _sut
                .Add(_step1)
                .Add<SampleApplicationPipelineStepWithDI>()
                .Add(_step2);

            _sut.Configure(_pipeline);

            _pipeline.KeysInOrder.Should().ContainInOrder(new[]
            {
                _step1.Key,
                SampleApplicationPipelineStepWithDI.StaticKey,
                _step2.Key
            });
        }

        [Fact]
        public void AppendWithAppBuilderActionShouldAddStepsToPipeline()
        {
            var mockedAction = new Mock<Action<IApplicationBuilder>>();

            _sut
                .Add(_step1)
                .Add("step-x", mockedAction.Object)
                .Add(_step2);

            _sut.Configure(_pipeline);

            _pipeline.KeysInOrder.Should().ContainInOrder(new[]
            {
                _step1.Key,
                "step-x",
                _step2.Key
            });
        }

        [Fact]
        public void AppendWithEnvAndAppBuilderActionShouldAddStepsToPipeline()
        {
            var mockedAction = new Mock<Action<IWebHostEnvironment, IApplicationBuilder>>();

            _sut
               .Add(_step1)
               .Add("step-x", mockedAction.Object)
               .Add(_step2);

            _sut.Configure(_pipeline);

            _pipeline.KeysInOrder.Should().ContainInOrder(new[]
            {
                _step1.Key,
                "step-x",
                _step2.Key
            });
        }


        [Fact]
        public void AddAfterShouldAddStepAfterStepWithGivenKey()
        {
            var step = new SampleApplicationPipelineStep();
            _sut
                .Add(_step1)
                .Add(_step2)
                .AddAfter(_step1.Key, step);

            _sut.Configure(_pipeline);

            _pipeline.Steps.Should().ContainInOrder(new[]
            {
                _step1,
                step,
                _step2
            });
        }

        [Fact]
        public void AddAfterGenericShouldUseDependencyInjectionToAddStepsToPipeline()
        {
            _sut
                .Add(_step1)
                .Add(_step2)
                .AddAfter<SampleApplicationPipelineStepWithDI>(_step1.Key);

            _sut.Configure(_pipeline);

            _pipeline.KeysInOrder.Should().ContainInOrder(new[]
            {
                _step1.Key,
                SampleApplicationPipelineStepWithDI.StaticKey,
                _step2.Key
            });
        }

        [Fact]
        public void AddAfterWith2GenericsShouldAddStepsToPipeline()
        {
            _sut
                .Add(_step1)
                .Add<SampleApplicationPipelineStep>()
                .Add(_step2)
                .AddAfter<SampleApplicationPipelineStep, SampleApplicationPipelineStepWithDI>();

            _sut.Configure(_pipeline);

            _pipeline.KeysInOrder.Should().ContainInOrder(new[]
            {
                _step1.Key,
                SampleApplicationPipelineStep.StaticKey,
                SampleApplicationPipelineStepWithDI.StaticKey,
                _step2.Key
            });
        }

        [Fact]
        public void AddBeforeWith2GenericsShouldAddStepsToPipeline()
        {
            _sut
                .Add(_step1)
                .Add<SampleApplicationPipelineStep>()
                .Add(_step2)
                .AddBefore<SampleApplicationPipelineStep, SampleApplicationPipelineStepWithDI>();

            _sut.Configure(_pipeline);

            _pipeline.KeysInOrder.Should().ContainInOrder(new[]
            {
                _step1.Key,
                SampleApplicationPipelineStepWithDI.StaticKey,
                SampleApplicationPipelineStep.StaticKey,
                _step2.Key
            });
        }

        [Fact]
        public void AddBeforeShouldAddStepBeforeStepWithGivenKey()
        {
            var step = new SampleApplicationPipelineStep();
            _sut
                .Add(_step1)
                .Add(_step2)
                .AddBefore(_step2.Key, step);

            _sut.Configure(_pipeline);

            _pipeline.Steps.Should().ContainInOrder(new[]
            {
                _step1,
                step,
                _step2
            });
        }

        [Fact]
        public void AddBeforeGenericShouldAddStepsBeforeStepWithGivenKey()
        {
            _sut
                .Add(_step1)
                .Add(_step2)
                .AddBefore<SampleApplicationPipelineStepWithDI>(_step2.Key);

            _sut.Configure(_pipeline);

            _pipeline.KeysInOrder.Should().ContainInOrder(new[]
            {
                _step1.Key,
                SampleApplicationPipelineStepWithDI.StaticKey,
                _step2.Key
            });
        }

        [Fact]
        public void AddAfterWithExpressionShouldAddStepAfterStepWithGivenKey()
        {
            Expression<Action<IApplicationBuilder>> exp = app => app.Use(null);
            _sut
                .Add(_step1)
                .Add(_step2)
                .AddAfter(_step1.Key, exp);

            _sut.Configure(_pipeline);

            _pipeline.KeysInOrder.Should().ContainInOrder(new[]
            {
                _step1.Key,
                exp.ToString(),
                _step2.Key
            });
        }


        [Fact]
        public void AddBeforeWithExpressionShouldAddStepBeforeStepWithGivenKey()
        {
            Expression<Action<IApplicationBuilder>> exp = app => app.Use(null);
            _sut
                .Add(_step1)
                .Add(_step2)
                .AddBefore(_step2.Key, exp);

            _sut.Configure(_pipeline);

            _pipeline.KeysInOrder.Should().ContainInOrder(new[]
            {
                _step1.Key,
                exp.ToString(),
                _step2.Key
            });
        }


        #region Removing
        [Fact]
        public void RemoveWithKeyShouldDeleteStepWithGivenKey()
        {
            _sut
                .Add(_step1)
                .Add(_step2)
                .Remove(_step2.Key);

            _sut.Configure(_pipeline);

            _pipeline.Steps.Should().ContainInOrder(new[]
            {
                _step1
            });
        }

        [Fact]
        public void RemoveShouldDeleteStep()
        {
            _sut
                .Add(_step1)
                .Add(_step2)
                .Remove(_step2);

            _sut.Configure(_pipeline);

            _pipeline.Steps.Should().ContainInOrder(new[]
            {
                _step1
            });
        }


        [Fact]
        public void RemoveWithGenericShouldDeleteStep()
        {
            _sut
                .Add(_step1)
                .Add<SampleApplicationPipelineStep>()
                .Remove<SampleApplicationPipelineStep>();

            _sut.Configure(_pipeline);

            _pipeline.Steps.Should().ContainInOrder(new[]
            {
                _step1
            });
        }
        #endregion Removing
    }

    public interface IDummyService
    {
    }
}
