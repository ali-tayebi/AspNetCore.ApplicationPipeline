using ApplicationPipeline.Abstractions;
using FluentAssertions;
using Moq;
using Xunit;

namespace ApplicationPipeline.Tests.Pipeline
{
    public class PipelineTests
    {
        [Fact]
        public void EmptyPipelineExecute()
        {
            var step = new Mock<IPipelineStep<int>>();
            step
                .Setup(s => s.Invoke(It.IsAny<int>()))
                .Returns<int>(x => x + 1);

            var pipeline = new Pipeline<IPipelineStep<int>, int>();

            var result = pipeline.Execute(5);
            result.Should().Be(5);
        }

        [Fact]
        public void PipelineExecute()
        {
            var step = new Mock<IPipelineStep<int>>();
            step
                .Setup(s => s.Invoke(It.IsAny<int>()))
                .Returns<int>(x => x + 1);

            var pipeline = new Pipeline<IPipelineStep<int>, int>()
                .Add(step.Object);

            var result = pipeline.Invoke(5);

            step.Verify(s=>s.Invoke(It.Is<int>(a=>a == 5)));
            result.Should().Be(6);
        }

        [Fact]
        public void PipelineExecute2()
        {
            var step = new Mock<IPipelineStep<int>>();

            step
                .Setup(s=>s.Key)
                .Returns("step1");
            step
                .Setup(s => s.Invoke(It.IsAny<int>()))
                .Returns<int>(x => x + 1);

            var step2 = new Mock<IPipelineStep<int>>();

            step2
                .Setup(s=>s.Key)
                .Returns("step2");
            step2
                .Setup(s => s.Invoke(It.IsAny<int>()))
                .Returns<int>(x => x + 2);

            var pipeline = new Pipeline<IPipelineStep<int>, int>();
            pipeline.Add(step.Object);
            pipeline.Add(step2.Object);

            var result = pipeline.Execute(5);

            step.Verify(s=>s.Invoke(It.Is<int>(a=>a == 5)));
            step2.Verify(s=>s.Invoke(It.Is<int>(a=>a == 6)));
            result.Should().Be(8);
        }

        [Fact]
        public void EmptyPipelineExecute222()
        {
            var step = new Mock<IPipelineStep<int>>();
            step
                .Setup(s => s.Invoke(It.IsAny<int>()))
                .Returns<int>(x => x + 1);

            var pipeline = new Pipeline<IPipelineStep<int>, int>();

            var result = pipeline.Execute(5);
            result.Should().Be(5);
        }
    }
}
