using System.Threading;
using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using UI.ReactiveButton;

namespace UI.Routing.ButtonActions
{
    public class GoBackUIRouteButtonAction : MonoButtonAction
    {
        [Inject] private UIRoutingService _uiRoutingService;

        protected override UniTask<bool> ExecuteInner(CancellationToken ct)
        {
            _uiRoutingService.GoBack();
            return UniTask.FromResult(true);
        }
    }
}