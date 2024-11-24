using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace StepStopper.WPFDemo
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        public List<SingleStepObject> items;

        private readonly StepContext _stepStoper;
        private CancellationTokenSource? _cts;
        public MainWindowViewModel()
        {
            _stepStoper = StepContext.Default;
            Items = Enumerable.Range(0, 100).AsSingleStepsObject().ToList();
        }
        [RelayCommand]
        public async Task Run()
        {
            Items = Enumerable.Range(0, 100).AsSingleStepsObject(_stepStoper).ToList();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            try
            {
                foreach (var item in Items)
                {
                    token.ThrowIfCancellationRequested();
                    var cur = await item.StepValue(_cts.Token);
                    await Task.Delay(1000);
                    item.SetEnd();
                }
                _cts.Dispose();
            }
            catch (OperationCanceledException cancel)
            {

            }
            finally
            {
                _cts.Dispose();
                _cts = null;
                _stepStoper.Reset();
            }
        }
        [RelayCommand]
        public void Stop()
        {
            _cts?.Cancel();
        }
        [RelayCommand]
        public void Pause()
        {
            _stepStoper.Pause();
        }
        [RelayCommand]
        public void NextSingle()
        {
            _stepStoper.NextSingle();
        }
        [RelayCommand]
        public void NextBreakingPoint()
        {
            _stepStoper.NextBreakingPoint();
        }
    }
}
