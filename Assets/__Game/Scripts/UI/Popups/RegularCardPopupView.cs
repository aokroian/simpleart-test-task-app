using System;
using Cards;
using Cysharp.Threading.Tasks;
using Initialization.EntryPoint.Listeners;
using R3;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Popups
{
    public class RegularCardPopupView : MonoEntryPointDoneListener
    {
        [SerializeField] private GameObject loadingGraphics;
        [SerializeField] private GameObject loadedGraphics;
        [SerializeField] private Image loadedImageComponent;

        [Inject] private readonly CardsViewModel _cardsViewModel;

        private IDisposable _spriteLoadSub;

        protected override UniTask OnEntryPointDone()
        {
            var gameObjectActiveObservable = gameObject.AddComponent<GoActiveAsObservable>();

            gameObjectActiveObservable.IsActiveInHierarchy.CombineLatest(
                    _cardsViewModel.SelectedCard,
                    (isEnabled, selectedCard) => (isEnabled, selectedCard)
                )
                .Subscribe(v =>
                {
                    _spriteLoadSub?.Dispose();

                    var isEnabled = v.isEnabled;
                    var selectedCard = v.selectedCard;
                    if (!isEnabled || selectedCard == null)
                    {
                        return;
                    }

                    RenderSprite();
                })
                .AddTo(this);

            return UniTask.CompletedTask;
        }

        private void RenderSprite()
        {
            var spriteAsObservable = _cardsViewModel.SelectedCard.CurrentValue.Sprite;
            if (!spriteAsObservable.CurrentValue)
            {
                // assuming it is still loading
                loadingGraphics.SetActive(true);
                loadedGraphics.SetActive(false);
                _spriteLoadSub = spriteAsObservable
                    .Where(v => v)
                    .Subscribe(_ =>
                    {
                        _spriteLoadSub?.Dispose();
                        RenderSprite();
                    });
            }
            else
            {
                loadingGraphics.SetActive(false);
                loadedGraphics.SetActive(true);
                loadedImageComponent.sprite = spriteAsObservable.CurrentValue;
            }
        }
    }
}