using R3;

namespace Cards
{
    public class CardsViewModel
    {
        private readonly ReactiveProperty<CardViewModel> _selectedCard = new();

        public void Initialize()
        {
        }
    }
}