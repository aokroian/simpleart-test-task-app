using System.Threading;
using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using UI.ReactiveButton;
using UnityEngine;

namespace UI.Routing.ButtonActions
{
    public class SetUIRouteButtonAction : MonoButtonAction
    {
        [SerializeField] private string[] route;

        [Inject] private UIRoutingService _uiRoutingService;

        protected override UniTask<bool> ExecuteInner(CancellationToken ct)
        {
            _uiRoutingService.GoToRoute(route);
            return UniTask.FromResult(true);
        }
    }
}