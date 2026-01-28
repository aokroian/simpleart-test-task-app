using System;
using System.Collections.Generic;
using Constants;
using R3;
using ZLinq;

namespace Cards
{
    public class CardsViewModel
    {
        public ReadOnlyReactiveProperty<List<CardViewModel>> FilteredCardsData => _filteredCardsData;
        public ReadOnlyReactiveProperty<string> SelectedFilterVal => _selectedFilterVal;

        private readonly ReactiveProperty<CardViewModel> _selectedCard = new();
        private readonly ReactiveProperty<string> _selectedFilterVal = new();
        private readonly List<CardViewModel> _allCardsData = new();
        private readonly ReactiveProperty<List<CardViewModel>> _filteredCardsData = new(null);

        public void Initialize()
        {
            InitializeCardsData();
            SetCardsFilter(Const.Cards.Filters.All);
        }

        public void SetCardsFilter(string filterVal)
        {
            _selectedFilterVal.Value = filterVal;
            var result = new List<CardViewModel>();
            if (filterVal == Const.Cards.Filters.All)
            {
                result.AddRange(_allCardsData);
            }
            else if (filterVal == Const.Cards.Filters.Even)
            {
                result.AddRange(
                    _allCardsData.AsValueEnumerable()
                        .Where((_, index) => (index +1) % 2 == 0)
                        .ToList());
            }
            else if (filterVal == Const.Cards.Filters.Odd)
            {
                result.AddRange(
                    _allCardsData.AsValueEnumerable()
                        .Where((_, index) => (index + 1) % 2 != 0)
                        .ToList());
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(filterVal));
            }

            _filteredCardsData.OnNext(result);
        }

        private void InitializeCardsData()
        {
            var counter = 0;
            for (var i = Const.Cards.ImagesUrlsIndicesFirst; i <= Const.Cards.ImagesUrlsIndicesLast; i++)
            {
                var cardImageUrl = Const.Cards.ImagesBaseUrl + i + Const.Cards.ImagesUrlEnding;
                var cardVm = new CardViewModel(
                    cardImageUrl,
                    Const.Cards.IsPremiumCard(counter)
                );
                _allCardsData.Add(cardVm);
                counter++;
            }
        }
    }
}