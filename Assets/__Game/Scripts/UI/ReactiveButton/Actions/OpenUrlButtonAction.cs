using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.ReactiveButton.Actions
{
    public class OpenUrlButtonAction: MonoButtonAction
    {
        [SerializeField] private string url;
        
        protected override UniTask<bool> ExecuteInner(CancellationToken ct)
        {
            Application.OpenURL(url);
            return UniTask.FromResult(true);
        }
    }
}