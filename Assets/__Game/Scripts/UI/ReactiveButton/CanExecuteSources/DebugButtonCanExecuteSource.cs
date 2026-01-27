using R3;

namespace UI.ReactiveButton.CanExecuteSources
{
    public class DebugButtonCanExecuteSource : MonoButtonCanExecuteSource
    {
        public bool canExecute;

        protected override ReactiveProperty<bool> CanExecuteInner { get; } = new(false);

        private void OnValidate()
        {
            CanExecuteInner.Value = canExecute;
        }
    }
}