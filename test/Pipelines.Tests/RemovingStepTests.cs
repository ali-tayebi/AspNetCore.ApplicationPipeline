using System;
using FluentAssertions;
using Xunit;

namespace Pipelines.Tests
{
    public class RemovingStepTests
    {
        private const string NotFoundKey = nameof(NotFoundKey);

        private readonly  SamplePipelineStep Step1;
        private readonly  SamplePipelineStep Step2;
        private readonly  SamplePipelineStep Step3;
        private readonly  SamplePipelineStep Step4;
        private readonly  SamplePipelineStep Step5;

        private readonly Pipeline<SamplePipelineStep, int> _sut;

        public RemovingStepTests()
        {
            Step1 = new SamplePipelineStep(nameof(Step1));
            Step2 = new SamplePipelineStep(nameof(Step2));
            Step3 = new SamplePipelineStep(nameof(Step3));
            Step4 = new SamplePipelineStep(nameof(Step4));
            Step5 = new SamplePipelineStep(nameof(Step5));

            _sut = new Pipeline<SamplePipelineStep, int>();
            _sut.Add(Step1);
            _sut.Add(Step2);
            _sut.Add(Step3);
            _sut.Add(Step4);
            _sut.Add(Step5);
        }

        [Fact]
        public void RemoveShouldDeleteStepsFromThePipeline()
        {
            _sut.Remove(Step2);
            _sut.Remove(Step4);

            _sut.Steps.Should().ContainInOrder(new[]
            {
                Step1, Step3, Step5
            });
        }

        [Fact]
        public void RemoveWithKeyShouldDeleteTheStepWithGivenKey()
        {
            _sut.RemoveWithKey(nameof(Step2));
            _sut.RemoveWithKey(nameof(Step4));

            _sut.Steps.Should().ContainInOrder(new[]
            {
                Step1, Step3, Step5
            });
        }

        [Fact]
        public void RemoveWithKeyShouldThrowNotFoundPipelineStepExceptionIfKeyIsNotFound()
        {
            Action act = () =>
            {
                _sut.RemoveWithKey(NotFoundKey);
            };

            act.Should().Throw<NotFoundPipelineStepException>().And.Key.Should().Be(NotFoundKey);
        }

        [Fact]
        public void RemoveShouldThrowNotFoundPipelineStepExceptionIfStepIsNotFound()
        {
            var step = new SamplePipelineStep(NotFoundKey);

            Action act = () => { _sut.Remove(step); };

            act.Should().Throw<NotFoundPipelineStepException>().And.Key.Should().Be(step.Key);
        }

        [Fact]
        public void RemoveShouldThrowNotFoundPipelineStepIfPipelineIsEmpty()
        {
            var sut = new Pipeline<SamplePipelineStep, int>();

            Action act = () =>
            {
                sut.Remove(Step5);
            };

            act.Should().Throw<NotFoundPipelineStepException>().And.Key.Should().Be(Step5.Key);
        }

        [Fact]
        public void RemoveWitKeyShouldThrowNotFoundPipelineStepIfPipelineIsEmpty()
        {
            var sut = new Pipeline<SamplePipelineStep, int>();

            Action act = () =>
            {
                sut.RemoveWithKey(Step5.Key);
            };

            act.Should().Throw<NotFoundPipelineStepException>().And.Key.Should().Be(Step5.Key);
       }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void RemoveWitKeyShouldThrowArgumentExceptionIfKeyIsNullEmptyOrWhitespace(string key)
        {
            Action act = () =>
            {
                _sut.RemoveWithKey(key);
            };

            act.Should().Throw<ArgumentException>().And.ParamName.Should().Be("key");
        }

        [Fact]
        public void RemoveShouldThrowArgumentExceptionIfStepIsNull()
        {
            Action act = () =>
            {
                _sut.Remove(null);
            };

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("step");
        }
    }
}
