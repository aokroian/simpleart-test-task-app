using UnityEngine;

namespace Cards
{
    public class CardViewModel
    {
        public readonly string ImageUrl;
        public readonly bool IsPremium;
        public Sprite Sprite { get; private set; }

        public CardViewModel(string imageUrl, bool isPremium)
        {
            ImageUrl = imageUrl;
            IsPremium = isPremium;
        }

        public void SetSprite(Sprite sprite)
        {
            Sprite = sprite;
        }
    }
}