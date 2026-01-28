namespace Cards
{
    public class CardViewModel
    {
        public readonly string ImageUrl;
        public readonly bool IsPremium;

        public CardViewModel(string imageUrl, bool isPremium)
        {
            ImageUrl = imageUrl;
            IsPremium = isPremium;
        }
    }
}