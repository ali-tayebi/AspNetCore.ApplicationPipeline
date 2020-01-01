using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Pipelines.Abstractions;
using Pipelines.Exceptions;

namespace Pipelines
{
    public class Pipeline<TStep, TInput> : 
        IExecute<TInput>, 
        IPipeline<TStep> where TStep : IPipelineStep<TInput>
    {
        private readonly LinkedList<TStep> _steps;
        public IReadOnlyCollection<TStep> Steps => new ReadOnlyCollection<TStep>(_steps.ToArray());

        public Pipeline()
        {
            _steps = new LinkedList<TStep>();
        }

        public TInput Execute(TInput input) => 
            _steps.Aggregate(input, (current, step) => step.Invoke(current));

        public IEnumerable<string> KeysInOrder => _steps.Select(x => x.Key).ToArray();

        public TStep Add(TStep step)
        {
            return Add(step, null, (steps, s, k) => _steps.AddLast(s).Value);
        }

        private TStep Add(TStep step, string key, Func<LinkedList<TStep>, TStep, string, TStep> addAction)
        {
            EnsureStepIsNotNull(step);
            return Contains(step)
                ? throw new DuplicatedPipelineStepException(step)
                : addAction.Invoke(_steps, step, key);
        }
        
        public bool Contains(TStep step)
        {
            if (step is null) return false;
            return Find(step) != null;
        }

        public bool ContainsKey(string key) =>
            !string.IsNullOrWhiteSpace(key) && Find(s=>s.Key == key) != null;

        public TStep AddBefore(string key, TStep step) =>
            Add(step, key, 
                (steps, s, k) =>
                {
                    var found = FindNode(k);
                    return found is null 
                        ? throw new NotFoundPipelineStepException(k) 
                        : steps.AddBefore(node: FindNode(k), s).Value;
                });

        public TStep AddAfter(string key, TStep step) =>
            Add(step, key, 
                (steps, s, k) =>
                {
                    var found = FindNode(k);
                    return found is null 
                        ? throw new NotFoundPipelineStepException(k) 
                        : steps.AddAfter(node: FindNode(k), s).Value;
                });


        public TStep AddAfter(Func<TStep, bool> selector, TStep step) =>
            Add(step, null, (steps, s, k) => steps.AddAfter(FindNode(selector), s).Value);

        private LinkedListNode<TStep> FindNode(string key)
        {
            return FindNode(s => s.Key == key);
        }

        private LinkedListNode<TStep> FindNode(Func<TStep, bool> selector)
        {
            return selector is null || _steps.FirstOrDefault(selector.Invoke) is null
                ? null 
                : _steps.Find(_steps.FirstOrDefault(selector.Invoke));
        }

        public TStep FindWithKey(string key) =>
            Find(_steps.SingleOrDefault(s => s.Key == key));
        
        public TStep Find(Func<TStep, bool> selector) =>
            selector != null
                ? Find(_steps.SingleOrDefault(selector.Invoke)) 
                : default;

        public TStep Find(TStep step)
        {
            if (step is null) return step;
            
            var found = _steps.Find(step);
            
            return found is null 
                ? default
                : found.Value;
        }
        
        public void Remove(TStep step)
        {
            EnsureStepIsNotNull(step);

            if (_steps.Remove(Find(step))) return;
            
            throw new NotFoundPipelineStepException(step);
        }

        public void RemoveWithKey(string key)
        {
            if(string.IsNullOrWhiteSpace(key)) 
                throw new ArgumentException("Pipeline step keys cannot be null, empty or whitespace", nameof(key));
            
            var found = FindWithKey(key);
            if(found is null) throw new NotFoundPipelineStepException(key);
            
            Remove(found);
        }
        
        private void EnsureStepIsNotNull(TStep step)
        {
            if (step is null) throw new ArgumentNullException(nameof(step));
        }
    }
}