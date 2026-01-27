using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.ReactiveButton.Actions
{
    public class SimulateClickButtonAction : MonoButtonAction
    {
        [SerializeField] private ReactiveButton otherButton;

        protected override UniTask<bool> ExecuteInner(CancellationToken ct)
        {
            otherButton.SimulateClick();
            return UniTask.FromResult(true);
        }
    }
}