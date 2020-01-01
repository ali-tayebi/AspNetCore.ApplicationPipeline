using System;

namespace Pipelines.Abstractions
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