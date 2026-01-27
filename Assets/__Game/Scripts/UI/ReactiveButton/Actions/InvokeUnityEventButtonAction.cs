using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace UI.ReactiveButton.Actions
{
    public class InvokeUnityEventButtonAction : MonoButtonAction
    {
        [SerializeField] private UnityEvent unityEvent;

        protected override UniTask<bool> ExecuteInner(CancellationToken ct)
        {
            unityEvent.Invoke();
            return UniTask.FromResult(true);
        }
    }
}