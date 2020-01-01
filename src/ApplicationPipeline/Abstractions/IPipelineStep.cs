using System;

namespace ApplicationPipeline.Abstractions
{
    public interface IPipelineStep<TInput> : IPipelineStep
    {
        TInput Invoke(TInput input);
    }

    public interface IPipelineStep : IEquatable<IPipelineStep>
    {
        string Key { get; }
    }
}
