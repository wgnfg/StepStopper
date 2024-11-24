using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepStopper
{
    public static class SingleStepExtensions
    {
        public static IEnumerable<SingleStep<T>> AsSingleSteps<T>(this IEnumerable<T> source, StepContext? stepStoper = default)
        {
            return source.Select(x => new SingleStep<T>() { Value = x, Context = stepStoper ?? StepContext.Default });
        }
        public static IEnumerable<SingleStepObject> AsSingleStepsObject<T>(this IEnumerable<T> source, StepContext? stepStoper = default)
        {
            return source.Select(x => new SingleStepObject() { Value = x, Context = stepStoper ?? StepContext.Default });
        }
    }
}
