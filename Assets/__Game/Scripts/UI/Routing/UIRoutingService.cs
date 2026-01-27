using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using R3;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Assertions;
using ZLinq;

namespace UI.Routing
{
    public class UIRoutingService
    {
        public ReadOnlyReactiveProperty<IReadOnlyCollection<string>> CurrentRouteObservable => _currentRouteAsObservable
            .Select(arr => arr as IReadOnlyCollection<string>)
            .ToReadOnlyReactiveProperty();

        public Observable<IReadOnlyCollection<string>> OnSetRouteAsObservable =>
            _onSetRouteAsObservable.Select(arr => arr as IReadOnlyCollection<string>);

        public Observable<IReadOnlyCollection<string>> OnAppendRouteAsObservable =>
            _onAppendRouteAsObservable.Select(arr => arr as IReadOnlyCollection<string>);

        public Observable<IReadOnlyCollection<string>> OnGoBackAsObservable =>
            _onGoBackAsObservable.Select(arr => arr as IReadOnlyCollection<string>);

        public Observable<IReadOnlyCollection<string>> OnGoForwardAsObservable =>
            _onGoForwardAsObservable.Select(arr => arr as IReadOnlyCollection<string>);

        private readonly List<UIRoutingGameObjectStateSource> _allPoints = new();
        private readonly List<string[]> _history = new();

        private readonly string[] _emptyRoute = Array.Empty<string>();

        private int _historyPointer;

        private string[] CurrentRoute => _history.Count > 0
            ? _history[_historyPointer]
            : _emptyRoute;

        private readonly ReactiveProperty<string[]> _currentRouteAsObservable = new();
        private readonly Subject<string[]> _onSetRouteAsObservable = new();
        private readonly Subject<string[]> _onAppendRouteAsObservable = new();
        private readonly Subject<string[]> _onGoBackAsObservable = new();
        private readonly Subject<string[]> _onGoForwardAsObservable = new();

        [Inject] private readonly IUIConfig _uiConfig;
        private string[] _allPointsIds;

        [Inject]
        private void Inject()
        {
            _allPointsIds = _uiConfig.GetUiRoutePoints();
        }

        public async UniTask<bool> InitializeAsync()
        {
            var initializeResult = false;

            await Observable.IntervalFrame(1)
                .FirstAsync(_ =>
                {
                    var isPointsCountOk = _allPoints.Count == _allPointsIds.Length;

                    if (isPointsCountOk)
                    {
                        var isAllPointsContentOk = _allPointsIds
                            .AsValueEnumerable()
                            .All(pointId =>
                            {
                                return _allPoints
                                    .AsValueEnumerable()
                                    .FirstOrDefault(p => p.PointId == pointId) != null;
                            });
                        if (isAllPointsContentOk)
                        {
                            initializeResult = true;
                            return true; // exit from the IntervalFrame
                        }
                    }


                    if (Time.realtimeSinceStartup > 2)
                    {
                        var remainingPoints = _allPointsIds.AsValueEnumerable()
                            .Where(pointId =>
                            {
                                return !_allPoints.AsValueEnumerable()
                                    .Select(p => p.PointId)
                                    .Contains(pointId);
                            })
                            .ToArray();
                        initializeResult = false;
                        UnityEngine.Debug.LogError(
                            "Error registering UI routing points. Remaining points: " +
                            string.Join(", ", remainingPoints));
                        return true; // exit from the IntervalFrame
                    }

                    return false; // continue IntervalFrame
                });

            return initializeResult;
        }

        public void RegisterPoint(UIRoutingGameObjectStateSource point)
        {
            if (!_allPointsIds.AsValueEnumerable().Contains(point.PointId))
            {
                UnityEngine.Debug.LogError(
                    $"Trying to register UI routing point with id not specified in config: {point.PointId}");
                return;
            }

            _allPoints.Add(point);
        }

        public void GoToAppendedRoute(string appendedPoint)
        {
            Assert.IsTrue(
                _allPointsIds.AsValueEnumerable().Contains(appendedPoint),
                $"{appendedPoint} is not a valid UI routing point");
            var route = new string[CurrentRoute.Length + 1];
            CurrentRoute.CopyTo(route, 0);
            route[^1] = appendedPoint;
            GoToRoute(route, true);
            _onAppendRouteAsObservable.OnNext(route);
        }

        public void GoToRoute(string[] route, bool muteEvents = false)
        {
            Assert.IsNotNull(route, "UI route must be provided");

            if (_history.Count == 0)
            {
                _history.Add(route);
                _historyPointer = 0;
                OpenCurrentRoute().Forget();

                if (!muteEvents)
                {
                    _onSetRouteAsObservable.OnNext(route);
                }

                return;
            }

            var backRoute = GetBackRoute();
            if (AreRoutesEqual(backRoute, route))
            {
                GoBack(muteEvents);
                return;
            }

            var forwardRoute = GetForwardRoute();
            if (forwardRoute == null || !AreRoutesEqual(forwardRoute, route))
            {
                TruncateHistoryByCurrentPointer();
                _history.Add(route);
                GoForward(true);
                if (!muteEvents)
                {
                    _onSetRouteAsObservable.OnNext(route);
                }
            }
            else
            {
                GoForward(muteEvents);
            }
        }

        public void GoBack(bool muteEvents = false)
        {
            if (_historyPointer == 0)
                return;
            _historyPointer--;
            OpenCurrentRoute().Forget();
            if (!muteEvents)
            {
                _onGoBackAsObservable.OnNext(CurrentRoute);
            }
        }

        public void GoForward(bool muteEvents = false)
        {
            if (GetForwardRoute() == null)
                return;
            _historyPointer++;
            OpenCurrentRoute().Forget();
            if (!muteEvents)
            {
                _onGoForwardAsObservable.OnNext(CurrentRoute);
            }
        }

        public string[] GetBackRoute()
        {
            if (_history.Count <= 1 || _historyPointer == 0)
            {
                return _emptyRoute;
            }

            return _history[_historyPointer - 1];
        }

        [CanBeNull]
        private string[] GetForwardRoute()
        {
            return _historyPointer >= _history.Count - 1
                ? null
                : _history[_historyPointer + 1];
        }

        private void TruncateHistoryByCurrentPointer()
        {
            var removeCount = _history.Count - 1 - _historyPointer;
            if (removeCount <= 0)
                return;
            _history.RemoveRange(_historyPointer + 1, removeCount);
        }

        public static bool AreRoutesEqual(string[] route1, string[] route2)
        {
            if (route1.Length != route2.Length)
                return false;

            for (var i = 0; i < route1.Length; i++)
            {
                if (route1[i] != route2[i])
                    return false;
            }

            return true;
        }

        // todo: probably should implement cancellation
        private async UniTask OpenCurrentRoute()
        {
            // todo: probably should rework this method
            
            // close all points which are not in current route
            var openPoints =
                _currentRouteAsObservable.CurrentValue != null
                    ? _currentRouteAsObservable.CurrentValue
                        .AsValueEnumerable()
                        .Select(id => _allPoints.AsValueEnumerable().First(p => p.PointId == id))
                        .ToList()
                    : _allPoints;
            var pointsToClose = openPoints
                .AsValueEnumerable()
                .Reverse()
                .Where(point => !CurrentRoute.AsValueEnumerable().Contains(point.PointId))
                .Where(point => point.ShouldToggle(false));
            foreach (var point in pointsToClose)
            {
                point.SetIsOn(false);
                var toggleDuration = point.GetToggleDurationSeconds();
                if (toggleDuration > 0)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(toggleDuration));
                }
                // todo: reset parenting and/or layers here if needed 
            }

            // open current route points
            foreach (var pointId in CurrentRoute.AsValueEnumerable())
            {
                var allRegisteredPoints = _allPoints
                    .AsValueEnumerable()
                    .Where(point => point.PointId == pointId)
                    .Where(point => point.ShouldToggle(true));
                foreach (var point in allRegisteredPoints)
                {
                    // todo: setup parenting and/or layers here if needed 
                    point.SetIsOn(true);
                    var toggleDuration = point.GetToggleDurationSeconds();
                    if (toggleDuration > 0)
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(toggleDuration));
                    }
                }
            }

            _currentRouteAsObservable.Value = CurrentRoute;
        }
    }
}