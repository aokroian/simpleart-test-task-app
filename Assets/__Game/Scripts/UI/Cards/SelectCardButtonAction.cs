using System;
using System.Threading;
using Cards;
using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using UI.ReactiveButton;

namespace UI.Cards
{
    public class SelectCardButtonAction : MonoButtonAction
    {
        [Inject] private readonly CardsViewModel _cardsViewModel;

        private CardViewModel _vm;

        public void Initialize(CardViewModel viewModel)
        {
            _vm = viewModel;
        }

        protected override UniTask<bool> ExecuteInner(CancellationToken ct)
        {
            _cardsViewModel.SelectCard(_vm);
            return UniTask.FromResult(true);
        }
    }
}