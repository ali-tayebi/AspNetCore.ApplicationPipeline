using System;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Pipelines.Abstractions;
using Xunit;

namespace Pipelines.Http.Tests
{
    public class HttpPipelineConfigurarTests
    {
        private readonly HttpPipelineConfigurar _sut;
        private readonly IHttpPipeline _pipeline;
        
        private readonly IHttpPipelineStep _step1;
        private readonly IHttpPipelineStep _step2;
        private readonly ApplicationBuilder _appBuilder;

        public HttpPipelineConfigurarTests()
        {
            var services = new ServiceCollection()
                .AddSingleton(new Mock<IWebHostEnvironment>().Object)
                .AddSingleton(new Mock<IDummyService>().Object);
            
            _sut = new HttpPipelineConfigurar(services);
            _pipeline = new HttpPipeline();
            _step1 = new HttpPipelineStep("step-1", app => { });
            _step2 = new HttpPipelineStep("step-2", app => { });
            _appBuilder = new ApplicationBuilder(services.BuildServiceProvider());
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
                .Add<SampleHttpPipelineStep>()
                .Add(_step2);

            _sut.Configure(_pipeline);

            _pipeline.KeysInOrder.Should().ContainInOrder(new[]
            {
                _step1.Key,
                SampleHttpPipelineStep.StaticKey,
                _step2.Key
            });
        }
        
        
        [Fact]
        public void AppendGenericShouldUseDependencyInjectionToAddStepsToPipeline()
        {
            _sut
                .Add(_step1)
                .Add<SampleHttpPipelineStepWithDI>()
                .Add(_step2);

            _sut.Configure(_pipeline);

            _pipeline.KeysInOrder.Should().ContainInOrder(new[]
            {
                _step1.Key,
                SampleHttpPipelineStepWithDI.StaticKey,
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
            var step = new SampleHttpPipelineStep();
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
                .AddAfter<SampleHttpPipelineStepWithDI>(_step1.Key);

            _sut.Configure(_pipeline);
            
            _pipeline.KeysInOrder.Should().ContainInOrder(new[]
            {
                _step1.Key,
                SampleHttpPipelineStepWithDI.StaticKey,
                _step2.Key
            });
        }
        
        [Fact]
        public void AddAfterWith2GenericsShouldAddStepsToPipeline()
        {
            _sut
                .Add(_step1)
                .Add<SampleHttpPipelineStep>()
                .Add(_step2)
                .AddAfter<SampleHttpPipelineStep, SampleHttpPipelineStepWithDI>();

            _sut.Configure(_pipeline);
            
            _pipeline.KeysInOrder.Should().ContainInOrder(new[]
            {
                _step1.Key,
                SampleHttpPipelineStep.StaticKey,
                SampleHttpPipelineStepWithDI.StaticKey,
                _step2.Key
            });
        }
        
        [Fact]
        public void AddBaforeWith2GenericsShouldAddStepsToPipeline()
        {
            _sut
                .Add(_step1)
                .Add<SampleHttpPipelineStep>()
                .Add(_step2)
                .AddBefore<SampleHttpPipelineStep, SampleHttpPipelineStepWithDI>();

            _sut.Configure(_pipeline);
            
            _pipeline.KeysInOrder.Should().ContainInOrder(new[]
            {
                _step1.Key,
                SampleHttpPipelineStepWithDI.StaticKey,
                SampleHttpPipelineStep.StaticKey,
                _step2.Key
            });
        }
        
        [Fact]
        public void AddBeforeShouldAddStepBeforeStepWithGivenKey()
        {
            var step = new SampleHttpPipelineStep();
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
                .AddBefore<SampleHttpPipelineStepWithDI>(_step2.Key);

            _sut.Configure(_pipeline);
            
            _pipeline.KeysInOrder.Should().ContainInOrder(new[]
            {
                _step1.Key,
                SampleHttpPipelineStepWithDI.StaticKey,
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
                .Add<SampleHttpPipelineStep>()
                .Remove<SampleHttpPipelineStep>();
            
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