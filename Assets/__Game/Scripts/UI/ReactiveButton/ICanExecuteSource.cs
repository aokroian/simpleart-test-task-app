using R3;

namespace UI.ReactiveButton
{
    public interface ICanExecuteSource
    {
        public ReadOnlyReactiveProperty<bool> CanExecute { get; }
    }
}