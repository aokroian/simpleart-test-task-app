using UnityEngine;
using PolyAndCode.UI;

namespace UI.Cards
{
    public static class CardsGridLayoutConfigurator
    {
        private const int TabletSegments = 3;
        private const int DefaultSegments = 2;

        private const float TabletAspectRatioThreshold = 1.6f;

        public static void Configure(RecyclableScrollRect scrollRect)
        {
            if (scrollRect == null)
            {
                Debug.LogError("CardsGridLayoutConfigurator: scrollRect is null");
                return;
            }

            scrollRect.Segments = IsTabletLike(GetAspectRatio())
                ? TabletSegments
                : DefaultSegments;
        }

        private static float GetAspectRatio()
        {
            float width = Screen.width;
            float height = Screen.height;

            // Always normalize so ratio >= 1
            return Mathf.Max(width, height) / Mathf.Min(width, height);
        }

        private static bool IsTabletLike(float aspectRatio)
        {
            return aspectRatio <= TabletAspectRatioThreshold;
        }
    }
}