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

        private readonly ReactiveProperty<CardViewModel> _selectedCard = new();
        private readonly List<CardViewModel> _allCardsData = new();
        private readonly ReactiveProperty<List<CardViewModel>> _filteredCardsData = new(null);

        public void Initialize()
        {
            InitializeCardsData();
            SetCardsFilter(Const.Cards.Filters.All);
        }

        public void SetCardsFilter(string filterVal)
        {
            var result = new List<CardViewModel>();
            if (filterVal == Const.Cards.Filters.All)
            {
                result.AddRange(_allCardsData);
            }
            else if (filterVal == Const.Cards.Filters.Even)
            {
                result.AddRange(
                    _allCardsData.AsValueEnumerable()
                        .Where((_, index) => index % 2 == 0)
                        .ToList());
            }
            else if (filterVal == Const.Cards.Filters.Odd)
            {
                result.AddRange(
                    _allCardsData.AsValueEnumerable()
                        .Where((_, index) => index % 2 != 0)
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
            for (var i = Const.Cards.ImagesUrlsIndicesFirst; i <= Const.Cards.ImagesUrlsIndicesLast; i++)
            {
                var cardImageUrl = Const.Cards.ImagesBaseUrl + i + Const.Cards.ImagesUrlEnding;
                var cardVm = new CardViewModel(
                    cardImageUrl,
                    Const.Cards.IsPremiumCard(i)
                );
                _allCardsData.Add(cardVm);
            }
        }
    }
}