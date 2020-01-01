using System;
using System.Collections.Generic;

namespace ApplicationPipeline.Abstractions
{
    public interface IPipeline<TStep>
    {
        IReadOnlyCollection<TStep> Steps { get; }
        TStep Add(TStep step);
        TStep AddBefore(string key, TStep step);
        TStep AddAfter(string key, TStep step);
        void Remove(TStep step);
        void RemoveWithKey(string key);
        TStep FindWithKey(string key);
        TStep Find(TStep step);
        TStep Find(Func<TStep, bool> selector);
        bool Contains(TStep step);
        bool ContainsKey(string key);
    }
}
