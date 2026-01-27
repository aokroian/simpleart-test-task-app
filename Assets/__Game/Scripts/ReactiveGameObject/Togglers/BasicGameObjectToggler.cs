using System.Threading;
using Cysharp.Threading.Tasks;

namespace ReactiveGameObject.Togglers
{
    public class BasicGameObjectToggler : GameObjectToggler
    {
        public override float GetToggleDurationSeconds()
        {
            return 0;
        }

        protected override void InterruptIfTransitioning()
        {
        }

        protected override UniTask ToggleInner(bool isOn, CancellationToken ct)
        {
            gameObject.SetActive(isOn);
            return UniTask.CompletedTask;
        }
    }
}