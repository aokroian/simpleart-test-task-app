using System;
using Cards;
using PolyAndCode.UI;
using UI.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Cards
{
    public class CardView : MonoBehaviour, IDisposable, ICell
    {
        [SerializeField] private ImageLoader imageLoader;
        [SerializeField] private Image loadedImageComponent;
        [SerializeField] private GameObject premiumGraphics;

        private CardViewModel _cardViewModel;
        private Action<Sprite> _onImageLoadSuccessAction;
        private Action _onImageLoadErrorAction;
        private bool _isInitialized;

        public void Initialize(
            CardViewModel viewModel,
            Action<Sprite> onImageLoadSuccess,
            Action onOnImageLoadError
        )
        {
            if (_isInitialized)
            {
                Dispose();
            }

            _cardViewModel = viewModel;
            premiumGraphics.SetActive(_cardViewModel.IsPremium);

            if (viewModel.Sprite)
            {
                imageLoader.StopAllCoroutines();
                loadedImageComponent.sprite = viewModel.Sprite;
                loadedImageComponent.enabled = true;
            }
            else
            {
                _onImageLoadErrorAction = onOnImageLoadError;
                _onImageLoadSuccessAction = onImageLoadSuccess;
                imageLoader.OnImageLoaded += OnImageLoaded;
                imageLoader.OnImageLoadFailed += OnImageLoadFailed;
                loadedImageComponent.enabled = false;
                imageLoader.Load(_cardViewModel.ImageUrl);
            }

            _isInitialized = true;
        }

        public void Dispose()
        {
            _cardViewModel = null;
            _onImageLoadErrorAction = null;
            _onImageLoadSuccessAction = null;
            imageLoader.OnImageLoaded -= OnImageLoaded;
            imageLoader.OnImageLoadFailed -= OnImageLoadFailed;

            imageLoader.StopAllCoroutines();
            _isInitialized = false;
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