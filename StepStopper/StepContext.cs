using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace StepStopper
{
    public class StepContext
    {
        public static readonly StepContext Default = new();

        public void Reset()
        {
            NeedNextPause = false;
        }

        internal bool NeedNextPause { get; set; } = false;

        private readonly ConcurrentBag<TaskCompletionSource<bool>> _currentWait = [];

        private void CompleteOne()
        {
            if (_currentWait.TryTake(out var currentItem))
            {
                currentItem.TrySetResult(true);
            }
        }

        internal Task WaitThisAsync(CancellationToken token = default)
        {
            var newTcs = new TaskCompletionSource<bool>(token);
            token.Register(() =>
            {
                newTcs.TrySetCanceled(token);
            });
            _currentWait.Add(newTcs);
            return newTcs.Task;
        }
        /// <summary>
        /// 中断
        /// </summary>
        public void Pause()
        {
            NeedNextPause = true;
        }
        /// <summary>
        /// 逐语句
        /// </summary>
        public void NextSingle()
        {
            CompleteOne();
            NeedNextPause = true;
        }
        /// <summary>
        /// 逐过程
        /// </summary>
        public void NextBreakingPoint()
        {
            CompleteOne();
            NeedNextPause = false;
        }

    }
}
