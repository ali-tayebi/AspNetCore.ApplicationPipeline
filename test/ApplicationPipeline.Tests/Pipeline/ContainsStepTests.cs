using FluentAssertions;
using Xunit;

namespace ApplicationPipeline.Tests.Pipeline
{
    public class ContainsTests
    {
        private const string NotFoundKey = nameof(NotFoundKey);

        private readonly  SamplePipelineStep Step1;
        private readonly  SamplePipelineStep Step2;
        private readonly  SamplePipelineStep Step3;
        private readonly  SamplePipelineStep Step4;
        private readonly  SamplePipelineStep Step5;

        private readonly Pipeline<SamplePipelineStep, int> _sut;

        public ContainsTests()
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
        public void ContainsShouldReturnTrueForExistingStep()
        {
            var found = _sut.Contains(Step2);

            found.Should().Be(true);
        }

        [Fact]
        public void ContainsKeyShouldReturnTrueForExistingStep()
        {
            var found = _sut.ContainsKey(Step2.Key);

            found.Should().Be(true);
        }

        [Fact]
        public void ContainsKeyShouldFalseForNotExistingStep()
        {
            var step = new SamplePipelineStep(NotFoundKey);
            var found = _sut.Contains(step);

            found.Should().Be(false);
        }

        [Fact]
        public void ContainsKeyShouldFalseForNotExistingKey()
        {
            var found = _sut.ContainsKey(NotFoundKey);

            found.Should().Be(false);
        }

        [Fact]
        public void ContainsShouldReturnFalseForNull()
        {
            var found = _sut.Contains(null);

            found.Should().Be(false);
        }
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void ContainsKeyShouldFalseForNullEmptyWhitespaceKey(string key)
        {
            var found = _sut.ContainsKey(key);

            found.Should().Be(false);
        }
    }
}
