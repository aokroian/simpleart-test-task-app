using Cards;
using Cysharp.Threading.Tasks;
using Initialization.EntryPoint.Listeners;
using R3;
using Reflex.Attributes;
using TMPro;
using UnityEngine;

namespace UI.Cards
{
    public class CardsFilterButtonView : MonoEntryPointDoneListener
    {
        [SerializeField] private TextMeshProUGUI[] labelTextComponents;
        [SerializeField] private GameObject selectedGraphics;
        [SerializeField] private string filterOptionVal;

        [Inject] private readonly CardsViewModel _cardsViewModel;

        protected override UniTask OnEntryPointDone()
        {
            _cardsViewModel.SelectedFilterVal
                .Subscribe(OnSelectedFilterChanged)
                .AddTo(this);
            return UniTask.CompletedTask;
        }

        private void OnSelectedFilterChanged(string val)
        {
            selectedGraphics.SetActive(val == filterOptionVal);
        }
    }
}