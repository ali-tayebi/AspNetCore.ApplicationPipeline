using System;
using Pipelines.Abstractions;

namespace Pipelines.Exceptions
{
    [Serializable]
    public class DuplicatedPipelineStepException : Exception
    {
        public IPipelineStep Step { get; }

        public DuplicatedPipelineStepException()
        {
        }
    
        public DuplicatedPipelineStepException(string message)
            : base(message)
        {
        }
    
        public DuplicatedPipelineStepException(string message, Exception inner)
            : base(message, inner)
        {
        }
        
        public DuplicatedPipelineStepException(IPipelineStep step)
            : this($"Duplicated step. Step with key ({step.Key}) already exists", step)
        {
        }
        
        public DuplicatedPipelineStepException(string message, IPipelineStep step)
            :base(message)
        {
            Step = step;
        }
    }
}