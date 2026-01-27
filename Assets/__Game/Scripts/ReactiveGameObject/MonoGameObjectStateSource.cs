using R3;
using UnityEngine;

namespace ReactiveGameObject
{
    public abstract class MonoGameObjectStateSource : MonoBehaviour, IGameObjectStateSource
    {
        public ReadOnlyReactiveProperty<bool> IsOn => IsOnInner;
        protected readonly ReactiveProperty<bool> IsOnInner = new(false);
    }
}