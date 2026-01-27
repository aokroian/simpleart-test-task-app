using ReactiveGameObject;
using Reflex.Attributes;
using UnityEngine;

namespace UI.Routing
{
    public class UIRoutingGameObjectStateSource : MonoGameObjectStateSource
    {
        [field: SerializeField]
        public string PointId { get; private set; }

        [SerializeField]
        private GameObjectToggler toggler;

        [Inject] private readonly UIRoutingService _routingService;

        [Inject]
        private void Inject()
        {
            _routingService.RegisterPoint(this);
        }

        private void OnValidate()
        {
            if (!toggler)
            {
                toggler = GetComponent<GameObjectToggler>();
            }
        }

        public void SetIsOn(bool isOn)
        {
            IsOnInner.Value = isOn;
        }

        public float GetToggleDurationSeconds()
        {
            return toggler.GetToggleDurationSeconds();
        }

        public bool ShouldToggle(bool targetState)
        {
            return toggler.ShouldToggle(targetState);
        }
    }
}