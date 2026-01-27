using R3;
using UnityEngine;

namespace UI.ReactiveButton
{
    [RequireComponent(typeof(ReactiveButton))]
    public abstract class MonoButtonCanExecuteSource : MonoBehaviour, ICanExecuteSource
    {
        public ReadOnlyReactiveProperty<bool> CanExecute => CanExecuteInner;
        protected abstract ReactiveProperty<bool> CanExecuteInner { get; }
    }
}