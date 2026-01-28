using System;
using DanielLochner.Assets.SimpleScrollSnap;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Utils
{
    public class CarouselAutoSwiper : MonoBehaviour
    {
        [SerializeField] private float intervalSeconds = 5f;
        [SerializeField] private SimpleScrollSnap simpleScrollSnap;

        private readonly ReactiveProperty<bool> _isMoving = new();
        private IDisposable _swipeSub;

        private void Start()
        {
            _isMoving.DistinctUntilChanged()
                .Subscribe(isMoving =>
                {
                    _swipeSub?.Dispose();
                    if (!isMoving)
                        ScheduleAutoSwipe();
                })
                .AddTo(this);
            simpleScrollSnap.GetComponent<ScrollRect>()
                .OnBeginDragAsObservable()
                .Subscribe(_ => { _isMoving.Value = true; });
            simpleScrollSnap.GetComponent<ScrollRect>()
                .OnEndDragAsObservable()
                .Subscribe(_ => { _isMoving.Value = false; });
        }

        private void ScheduleAutoSwipe()
        {
            _swipeSub = Observable.Timer(TimeSpan.FromSeconds(intervalSeconds))
                .Subscribe(_ =>
                {
                    if (_isMoving.Value)
                    {
                        return;
                    }
                    simpleScrollSnap.GoToNextPanel();
                    ScheduleAutoSwipe();
                });
        }
    }
}