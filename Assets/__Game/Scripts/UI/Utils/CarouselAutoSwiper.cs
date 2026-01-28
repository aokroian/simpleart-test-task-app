using System;
using DanielLochner.Assets.SimpleScrollSnap;
using R3;
using UnityEngine;

namespace UI.Utils
{
    public class CarouselAutoSwiper : MonoBehaviour
    {
        [SerializeField] private float intervalSeconds = 5f;
        [SerializeField] private SimpleScrollSnap simpleScrollSnap;

        private void Start()
        {
            Observable.Interval(TimeSpan.FromSeconds(intervalSeconds))
                .Subscribe(_ => simpleScrollSnap.GoToNextPanel())
                .AddTo(this);
        }
    }
}