using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace StepStopper
{
    public class SingleStep<T> : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public bool NeedBreakpoint { get; set; }
        private StepStatus _status = StepStatus.NotStarted;
        public StepStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                NotifyPropertyChanged();
            }
        }
        public StepContext Context { get; set; } = StepContext.Default;
        public T? Value { get; set; }

        public async Task<T?> StepValue(CancellationToken token)
        {
            if (Context.NeedNextPause || NeedBreakpoint)
            {
                Status = StepStatus.Waiting;
                await Context.WaitThisAsync(token).ConfigureAwait(false);
            }
            Status = StepStatus.Processing;
            return Value;
        }

        public void SetEnd()
        {
            Status = StepStatus.End;
        }
    }
}
