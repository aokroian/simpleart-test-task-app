using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace ReactiveGameObject
{
    public abstract class GameObjectToggler : MonoBehaviour, IDisposable
    {
        private readonly ReactiveProperty<GameObjectTogglerState> _state = new(GameObjectTogglerState.None);

        public void Dispose()
        {
            InterruptIfTransitioning();
        }

        public abstract float GetToggleDurationSeconds();

        public bool ShouldToggle(bool targetState)
        {
            return (targetState && _state.Value != GameObjectTogglerState.Shown)
                   || (!targetState && _state.Value != GameObjectTogglerState.Hidden);
        }

        public async UniTask Toggle(bool isOn, CancellationToken ct = default)
        {
            if (ct == default)
            {
                ct = destroyCancellationToken;
            }

            if (!ShouldToggle(isOn))
            {
                Debug.LogWarning("GameObjectToggler is already toggled to requested state", this);
                return;
            }

            InterruptIfTransitioning();

            _state.Value = isOn
                ? GameObjectTogglerState.Showing
                : GameObjectTogglerState.Hiding;

            await ToggleInner(isOn, ct);

            _state.Value = isOn
                ? GameObjectTogglerState.Shown
                : GameObjectTogglerState.Hidden;
        }

        protected abstract void InterruptIfTransitioning();

        protected abstract UniTask ToggleInner(bool isOn, CancellationToken ct);
    }
}