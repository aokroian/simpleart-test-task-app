using R3;
using UnityEngine;

namespace Cards
{
    public class CardViewModel
    {
        public readonly string ImageUrl;
        public readonly bool IsPremium;
        public ReadOnlyReactiveProperty<Sprite> Sprite => _sprite;

        private readonly ReactiveProperty<Sprite> _sprite = new();

        public CardViewModel(string imageUrl, bool isPremium)
        {
            ImageUrl = imageUrl;
            IsPremium = isPremium;
        }

        public void SetSprite(Sprite sprite)
        {
            _sprite.Value = sprite;
        }
    }
}