using System.Collections.Generic;

namespace ApplicationPipeline.Abstractions
{
    public interface IExecute<TInput>
    {
        TInput Execute(TInput input);
        IEnumerable<string> KeysInOrder { get; }
    }
}
