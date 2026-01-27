using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using UI.ReactiveButton;
using UnityEngine;
using ZLinq;

namespace UI.Routing.ButtonActions
{
    public class ConditionalSetUIRouteButtonAction : MonoButtonAction
    {
        [SerializeField] private ConditionalSetUIRouteButtonActionBinding[] conditionBindings;

        [Inject] private readonly UIRoutingService _uiRoutingService;

        protected override UniTask<bool> ExecuteInner(CancellationToken ct)
        {
            var backRoute = _uiRoutingService.GetBackRoute();
            var conditionBinding = conditionBindings
                .AsValueEnumerable()
                .FirstOrDefault(b => UIRoutingService.AreRoutesEqual(backRoute, b.condition.route));

            if (conditionBinding != null)
            {
                _uiRoutingService.GoToRoute(conditionBinding.targetRoute);
            }

            return UniTask.FromResult(true);
        }
    }

    [Serializable]
    public class ConditionalSetUIRouteButtonActionBinding
    {
        public UIRouteCondition condition;
        public string[] targetRoute;
    }

    [Serializable]
    public class UIRouteCondition
    {
        public UIRouteConditionKind kind;
        public string[] route;
    }

    public enum UIRouteConditionKind
    {
        PrevRoute,
        // todo: implement more if needed
    }
}