using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Reflex.Attributes;
using UnityEngine;

namespace ReactiveGameObject.Togglers
{
    public class FadeGameObjectToggler : GameObjectToggler
    {
        [SerializeField] private CanvasGroup canvasGroup;

        private const float Duration = .18f;

        private Tween _tween;

        [Inject]
        private void Inject()
        {
            gameObject.SetActive(false);
            canvasGroup.alpha = 0f;
        }

        public override float GetToggleDurationSeconds()
        {
            return Duration;
        }

        protected override void InterruptIfTransitioning()
        {
            _tween?.Kill();
            _tween = null;
        }

        protected override async UniTask ToggleInner(bool isOn, CancellationToken ct)
        {
            if (isOn && gameObject.activeInHierarchy && Mathf.Approximately(canvasGroup.alpha, 1))
            {
                return;
            }

            if (!isOn && !gameObject.activeInHierarchy && canvasGroup.alpha == 0)
            {
                return;
            }

            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }

            var alphaTarget = isOn
                ? 1f
                : 0f;
            var tween = canvasGroup.DOFade(alphaTarget, Duration);
            tween.Play();
            await tween.AsyncWaitForCompletion();
            if (!isOn)
            {
                gameObject.SetActive(false);
            }
        }
    }
}