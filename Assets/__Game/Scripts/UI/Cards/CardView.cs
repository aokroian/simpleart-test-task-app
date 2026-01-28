using System;
using Cards;
using PolyAndCode.UI;
using UI.Utils;
using UnityEngine;

namespace UI.Cards
{
    public class CardView : MonoBehaviour, IDisposable, ICell
    {
        [SerializeField] private ImageLoader imageLoader;
        [SerializeField] private GameObject premiumGraphics;

        private CardViewModel _cardViewModel;
        private Action<Sprite> _onImageLoadSuccessAction;
        private Action _onImageLoadErrorAction;

        public void Initialize(
            CardViewModel viewModel,
            Action<Sprite> onImageLoadSuccess,
            Action onOnImageLoadError
        )
        {
            _cardViewModel = viewModel;
            premiumGraphics.SetActive(_cardViewModel.IsPremium);

            _onImageLoadErrorAction = onOnImageLoadError;
            _onImageLoadSuccessAction = onImageLoadSuccess;

            imageLoader.OnImageLoaded += OnImageLoaded;
            imageLoader.OnImageLoadFailed += OnImageLoadFailed;
            imageLoader.Load(_cardViewModel.ImageUrl);
        }

        public void Dispose()
        {
            _cardViewModel = null;
            _onImageLoadErrorAction = null;
            _onImageLoadSuccessAction = null;
            imageLoader.OnImageLoaded -= OnImageLoaded;
            imageLoader.OnImageLoadFailed -= OnImageLoadFailed;

            imageLoader.StopAllCoroutines();
            imageLoader.ClearImage();
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void OnImageLoaded(Sprite sprite)
        {
            _onImageLoadSuccessAction?.Invoke(sprite);
            _onImageLoadSuccessAction = null;
        }

        private void OnImageLoadFailed()
        {
            _onImageLoadErrorAction?.Invoke();
            _onImageLoadErrorAction = null;
        }
    }
}