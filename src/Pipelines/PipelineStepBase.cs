using System;
using Pipelines.Abstractions;

namespace Pipelines
{
    public abstract class PipelineStepBase<TInput> : IPipelineStep<TInput>
    {
        public string Key { get; }

        protected PipelineStepBase(string key)
        {
            Key = !string.IsNullOrWhiteSpace(key) 
                ? key 
                : throw new ArgumentException("Key should not be null, empty for whitespace", nameof(key));
        }

        public abstract TInput Invoke(TInput input);
        
        public bool Equals(IPipelineStep other) => 
            Key.Equals(other?.Key, StringComparison.InvariantCulture);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PipelineStepBase<TInput>) obj);
        }

        public override int GetHashCode()  
        {
            return (Key != null ? Key.GetHashCode() : 0);
        } 
        
        public static bool operator ==(PipelineStepBase<TInput> obj1, IPipelineStep<TInput> obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (ReferenceEquals(obj1, null))
            {
                return false;
            }
            return !ReferenceEquals(obj2, null) && obj1.Equals(obj2);
        }

        public static bool operator !=(PipelineStepBase<TInput> obj1, IPipelineStep<TInput> obj2)
        {
            return !(obj1 == obj2);
        }
    }
}