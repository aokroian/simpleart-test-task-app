using System.Threading;
using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using UI.ReactiveButton;
using UnityEngine;

namespace UI.Routing.ButtonActions
{
    public class AppendUIRouteButtonAction : MonoButtonAction
    {
        [SerializeField] private string pointID;

        [Inject] private UIRoutingService _uiRoutingService;

        protected override UniTask<bool> ExecuteInner(CancellationToken ct)
        {
            _uiRoutingService.GoToAppendedRoute(pointID);
            return UniTask.FromResult(true);
        }
    }
}