using System;
using FluentAssertions;
using Xunit;

namespace Pipelines.Tests
{
    public class AddingStepTests
    {
        private readonly  SamplePipelineStep Step1;
        private readonly  SamplePipelineStep Step2;
        private readonly  SamplePipelineStep Step3;
        private readonly  SamplePipelineStep Step4;
        private readonly  SamplePipelineStep Step5;

        private readonly Pipeline<SamplePipelineStep, int> _sut;

        public AddingStepTests()
        { 
            Step1 = new SamplePipelineStep(nameof(Step1)); 
            Step2 = new SamplePipelineStep(nameof(Step2));
            Step3 = new SamplePipelineStep(nameof(Step3));
            Step4 = new SamplePipelineStep(nameof(Step4));
            Step5 = new SamplePipelineStep(nameof(Step5));
            _sut = new Pipeline<SamplePipelineStep, int>(); 
        }

        [Fact]
        public void AddShouldAddStepsToThePipeline()
        {
            _sut.Add(Step1);
            _sut.Add(Step2);
            _sut.Add(Step3);

            _sut.Steps.Should().ContainInOrder(new[]
            {
                Step1, Step2, Step3
            });
        }
        
        [Fact]
        public void AddShouldThrowDuplicatedStepExceptionForDuplicatedSteps()
        {
            _sut.Add(Step1);
            _sut.Add(Step2);
            
            Action act = () =>
            {
                _sut.Add(Step2);
            };

            act.Should().Throw<DuplicatedPipelineStepException>().And.Step.Should().Be(Step2);
        }

        [Fact]
        public void AddShouldAddRemovedSteps()
        {
            _sut.Add(Step1);
            _sut.Add(Step2);
            _sut.Remove(Step1);
            _sut.Add(Step1);
            
            _sut.Steps.Should().ContainInOrder(new[]
            {
                Step2, Step1
            });
        }

        [Fact]
        public void AddAfterShouldAddStepAfterTheStepWithGivenKey()
        {
            _sut.Add(Step1);
            _sut.Add(Step2);
            _sut.Add(Step3);
            _sut.AddAfter(nameof(Step2), Step4);
            _sut.AddAfter(nameof(Step2), Step5);
            
            _sut.Steps.Should().ContainInOrder(new[]
            {
                Step1, Step2, Step5, Step4, Step3
            });
        }
        
        [Fact]
        public void AddAfterShouldAddStepAfterTheStepWithGivenKeyAtTheEnd()
        {
            _sut.Add(Step1);
            _sut.Add(Step2);
            _sut.Add(Step3);
            _sut.AddAfter(nameof(Step3), Step4);
            _sut.AddAfter(nameof(Step3), Step5);
            
            _sut.Steps.Should().ContainInOrder(new[]
            {
                Step1, Step2, Step3, Step5, Step4
            });
        }

        [Fact]
        public void AddAfterShouldThrowNotFoundPipelineStepExceptionWhenGivenKeyIsNotFound()
        {
            _sut.Add(Step1);
            _sut.Add(Step2);

            Action act = () => { _sut.AddAfter("StepX", Step3); };

            act.Should().Throw<NotFoundPipelineStepException>().And.Key.Should().Be("StepX");
        }
        
        [Fact]
        public void AddBeforeShouldThrowNotFoundPipelineStepExceptionWhenGivenKeyIsNotFound()
        {
            _sut.Add(Step1);
            _sut.Add(Step2);

            Action act = () => { _sut.AddBefore("StepX", Step3); };

            act.Should().Throw<NotFoundPipelineStepException>().And.Key.Should().Be("StepX");
        }

        [Fact]
        public void AddBeforeShouldAddStepBeforeTheStepWithGivenKey()
        {
            _sut.Add(Step1);
            _sut.Add(Step2);
            _sut.AddBefore(nameof(Step2), Step3);
            _sut.AddBefore(nameof(Step2), Step4);
            _sut.AddBefore(nameof(Step2), Step5);

            _sut.Steps.Should().ContainInOrder(new[]
            {
                Step1, Step3, Step4, Step5, Step2
            });
        }
        
        [Fact]
        public void AddBeforeShouldAddStepBeforeTheStepWithGivenKeyAtFist()
        {
            _sut.Add(Step1);
            _sut.Add(Step2);
            
            _sut.AddBefore(nameof(Step1), Step3);
            _sut.AddBefore(nameof(Step1), Step4);
            _sut.AddBefore(nameof(Step1), Step5);
            
            _sut.Steps.Should().ContainInOrder(new[]
            {
                Step3, Step4, Step5, Step1, Step2
            });
        }
        
        [Fact]
        public void AddAfterShouldAddStepBeforeTheStepWithGivenKeyAtEnd()
        {
            _sut.Add(Step1);
            _sut.Add(Step2);
            _sut.Add(Step3);
            
            _sut.AddBefore(nameof(Step3), Step4);
            _sut.AddBefore(nameof(Step3), Step5);
            
            _sut.Steps.Should().ContainInOrder(new[]
            {
                Step1, Step2, Step4, Step5, Step3
            });
        }
    }
}