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
        public StepContext()
        {
            Reset();
        }
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
        /// 中断<br/>
        /// 下一项取出前会暂停
        /// </summary>
        public void BreakAll()
        {
            NeedNextPause = true;
        }
        /// <summary>
        /// 逐语句<br/>
        /// 开始当前的暂停并停在下一项取出前
        /// </summary>
        public void StepOver()
        {
            CompleteOne();
            NeedNextPause = true;
        }
        /// <summary>
        /// 逐过程<br/>
        /// 开始当前的暂停
        /// </summary>
        public void Continue()
        {
            CompleteOne();
            NeedNextPause = false;
        }

    }
}
