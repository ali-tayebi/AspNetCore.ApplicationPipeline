using System.Collections.Generic;

namespace Pipelines.Abstractions
{
    public interface IExecute<TInput>
    {
        TInput Execute(TInput input);
        IEnumerable<string> KeysInOrder { get; }
    }
}