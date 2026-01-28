using System.Threading;
using Cards;
using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using UI.ReactiveButton;
using UnityEngine;

namespace UI.Cards
{
    public class SetCardFilterButtonAction : MonoButtonAction
    {
        [SerializeField] private string filterOptionVal;

        [Inject] private readonly CardsViewModel _cardsViewModel;

        protected override UniTask<bool> ExecuteInner(CancellationToken ct)
        {
            _cardsViewModel.SetCardsFilter(filterOptionVal);
            return UniTask.FromResult(true);
        }
    }
}