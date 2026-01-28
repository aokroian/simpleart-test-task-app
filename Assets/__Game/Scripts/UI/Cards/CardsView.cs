using Cards;
using Cysharp.Threading.Tasks;
using Initialization.EntryPoint.Listeners;
using PolyAndCode.UI;
using R3;
using Reflex.Attributes;
using UnityEngine;
using Utils;

namespace UI.Cards
{
    public class CardsView : MonoEntryPointDoneListener, IRecyclableScrollRectDataSource
    {
        [SerializeField] RecyclableScrollRect recyclableScrollRect;

        [Inject] private readonly CardsViewModel _cardsViewModel;

        private bool _isRecyclableScrollRectInitialized;

        public int GetItemCount()
        {
            return _cardsViewModel.FilteredCardsData == null
                ? 0
                : _cardsViewModel.FilteredCardsData.CurrentValue.Count;
        }

        public void SetCell(ICell cell, int index)
        {
            var cardView = cell as CardView;
            var cardViewModel = _cardsViewModel.FilteredCardsData
                .CurrentValue[index];

            cardView!.Initialize(
                cardViewModel,
                cardViewModel.SetSprite,
                null
            );
        }

        protected override UniTask OnEntryPointDone()
        {
            var gameObjectActiveAsObservable = gameObject.AddComponent<GoActiveAsObservable>();
            gameObjectActiveAsObservable.Initialize();

            gameObjectActiveAsObservable.IsActiveInHierarchy.CombineLatest(
                    _cardsViewModel.FilteredCardsData.Where(v => v != null),
                    (isEnabled, d) => (isEnabled, d)
                )
                .Subscribe(
                    (v =>
                    {
                        var isEnabled = v.isEnabled;
                        if (!isEnabled)
                        {
                            recyclableScrollRect.StopAllCoroutines();
                        }
                        else
                        {
                            if (!_isRecyclableScrollRectInitialized)
                            {
                                CardsGridLayoutConfigurator.Configure(recyclableScrollRect);
                                recyclableScrollRect.Initialize(this);
                                _isRecyclableScrollRectInitialized = true;
                            }
                            else
                            {
                                recyclableScrollRect.ReloadData();
                            }
                        }
                    }))
                .AddTo(this);

            return UniTask.CompletedTask;
        }
    }
}