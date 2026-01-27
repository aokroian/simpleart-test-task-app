using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.ReactiveButton.Actions
{
    public class DebugButtonAction : MonoButtonAction
    {
        [SerializeField, Range(0, 10)] private float duration;
        [SerializeField] private bool result;
        [SerializeField] private string message = "";

        protected override async UniTask<bool> ExecuteInner(CancellationToken ct)
        {
            if (duration == 0)
            {
                Debug.Log($"debug action result {result}. {message}", this);
                return result;
            }

            await UniTask.WaitForSeconds(duration, cancellationToken: ct);
            Debug.Log($"debug action result {result}. {message}", this);
            return result;
        }
    }
}