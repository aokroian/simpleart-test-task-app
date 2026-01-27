using System;
using Constants;
using Cysharp.Threading.Tasks;
using Initialization.EntryPoint.Listeners;
using R3;
using Reflex.Attributes;

namespace UI.Routing
{
    public class UIRoutingFlowManager : EntryPointDoneListener
    {
        private const float SplashScreenDurationSeconds = 2f;

        [Inject] private readonly UIRoutingService _uiRoutingService;

        protected override UniTask OnEntryPointDone()
        {
            _uiRoutingService.GoToRoute(new[] {Const.UIRoutingPointIDs.SplashScreen});
            Observable.Timer(TimeSpan.FromSeconds(SplashScreenDurationSeconds))
                .Subscribe(OnSplashScreenDone);
            return UniTask.CompletedTask;
        }

        private void OnSplashScreenDone(Unit _)
        {
            _uiRoutingService.GoToRoute(new[] {Const.UIRoutingPointIDs.Home});
        }
    }
}