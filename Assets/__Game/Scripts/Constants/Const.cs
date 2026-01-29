namespace Constants
{
    public static class Const
    {
        public static class UIRoutingPointIDs
        {
            public const string SplashScreen = "splash";
            public const string Home = "home";
            public const string CardPopup = "card_popup";
            public const string PremiumPopup = "premium_popup";
        }

        public static class Cards
        {
            public const string ImagesBaseUrl = "http://data.ikppbb.com/test-task-unity-data/pics/";
            public const int ImagesUrlsIndicesFirst = 1;
            public const int ImagesUrlsIndicesLast = 66;
            public const string ImagesUrlEnding = ".jpg";

            public static class Filters
            {
                public const string All = "all";
                public const string Odd = "odd";
                public const string Even = "even";
            }

            public static bool IsPremiumCard(int index)
            {
                return index != 0 && (index + 1) % 4 == 0;
            }
        }

        public static class Sounds
        {
            public const string ButtonClick = "button_click";
        }
    }
}