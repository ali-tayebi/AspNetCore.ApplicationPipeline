using System;
using Pipelines.Abstractions;

namespace Pipelines
{
    [Serializable]
    public class NotFoundPipelineStepException : Exception
    {
        public string Key { get; }

        public NotFoundPipelineStepException()
        {
        }

        public NotFoundPipelineStepException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public NotFoundPipelineStepException(string message, string key)
            : base(message)
        {
            Key = key;
        }
        
        public NotFoundPipelineStepException(string key)
            : this($"No step with the given key ({key}) is found", key)
        {
        }
        
        public NotFoundPipelineStepException(IPipelineStep step)
            : this($"No step with the given key ({step.Key}) is found", step.Key)
        {
        }
    }
}