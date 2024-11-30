using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepStopper
{
    public static class SingleStepExtensions
    {
        public static IEnumerable<SingleStep<T>> AsSingleSteps<T>(this IEnumerable<T> source, StepContext? stepContext = default)
        {
            return source.Select(x => x.AsSingleStep(stepContext));
        }
        public static IEnumerable<SingleStepObject> AsSingleStepsObject<T>(this IEnumerable<T> source, StepContext? stepContext = default)
        {
            return source.Select(x => x.AsSingleStepObject(stepContext));
        }
        public static SingleStep<T> AsSingleStep<T>(this T source, StepContext? stepStoper = default)
        {
            return new SingleStep<T>() { Value = source, Context = stepStoper ?? StepContext.Default };
        }
        public static SingleStepObject AsSingleStepObject<T>(this T source, StepContext? stepStoper = default)
        {
            return new SingleStepObject() { Value = source, Context = stepStoper ?? StepContext.Default };
        }
    }
}
