using FluentAssertions;
using Xunit;

namespace ApplicationPipeline.Tests.Pipeline
{
    public class FindingStepTests
    {
	    private const string NotFoundKey = nameof(NotFoundKey);

        private readonly  SamplePipelineStep Step1;
        private readonly  SamplePipelineStep Step2;
        private readonly  SamplePipelineStep Step3;
        private readonly  SamplePipelineStep Step4;
        private readonly  SamplePipelineStep Step5;

        private readonly Pipeline<SamplePipelineStep, int> _sut;

        public FindingStepTests()
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
        public void FindShouldFindStep()
        {
            var found = _sut.Find(Step2);

            found.Should().Be(Step2);
        }

        [Fact]
        public void FindWithKeyShouldFindStepWithGivenKey()
        {
            var found = _sut.FindWithKey(Step2.Key);

            found.Should().Be(Step2);
        }

        [Fact]
        public void FindWithKeyShouldFindStepWithGivenFunc()
        {
            var found = _sut.Find(step=> step.Key == Step3.Key);

            found.Should().Be(Step3);
        }

        [Fact]
        public void FindShouldReturnNullForNotExistingStep()
        {
            var step = new SamplePipelineStep(NotFoundKey);
            var found = _sut.Find(step);

            found.Should().BeNull();
        }

        [Fact]
        public void FindWithKeyShouldReturnNullForNotExistingKey()
        {
            var found = _sut.FindWithKey(NotFoundKey);

            found.Should().BeNull();
        }

        [Fact]
        public void FindShouldReturnNullForNotFoundStep()
        {
            var found = _sut.Find(step=> step.Key == NotFoundKey);

            found.Should().BeNull();
        }
    }
}
