using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

namespace UI.ReactiveButton
{
    public interface IButtonAction
    {
        public ReadOnlyReactiveProperty<bool> IsExecuting { get; }
        public UniTask<bool> Execute(CancellationToken ct = default);
        public int Order { get; }
    }
}