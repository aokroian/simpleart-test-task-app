using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using ReactiveGameObject;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using ZLinq;

namespace UI.ReactiveButton
{
    [RequireComponent(typeof(Button))]
    public class ReactiveButton : MonoBehaviour, IGameObjectStateSource
    {
        public ReadOnlyReactiveProperty<bool> IsOn => _isOn;
        private readonly ReactiveProperty<bool> _isOn = new(false);
        private IButtonAction[] _actions;
        private readonly ReactiveProperty<bool> _isExecutingListOfActions = new(false);
        private Button _buttonComponent;

        public void SimulateClick()
        {
            OnButtonClicked().Forget();
        }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            TryGetComponent(out _buttonComponent);

            SubscribeToButtonClick();

            _isOn.Subscribe(isOn => _buttonComponent.interactable = isOn).AddTo(this);

            _actions = GetComponents<IButtonAction>()
                .AsValueEnumerable()
                .OrderBy(el => el.Order)
                .ToArray();
            Assert.IsTrue(_actions.Length > 0, "Did not find any IButtonAction");

            var combinedCanExecuteObservables = new List<ReadOnlyReactiveProperty<bool>>();
            var canExecuteSourcesObservables = GetComponents<ICanExecuteSource>()
                .AsValueEnumerable()
                .Select(s => s.CanExecute)
                .ToList();
            var isExecutingNegatedObservables = _actions
                .AsValueEnumerable()
                .Select(s => s.IsExecuting.Select(isExecuting => !isExecuting).ToReadOnlyReactiveProperty())
                .ToList();

            combinedCanExecuteObservables.AddRange(canExecuteSourcesObservables);

            // remove this if you want the button's actions to be able
            // to run even if the button is already running
            // and all the actions haven't completed yet
            combinedCanExecuteObservables.AddRange(isExecutingNegatedObservables);
            combinedCanExecuteObservables.Add(_isExecutingListOfActions.Select(x => !x).ToReadOnlyReactiveProperty());

            Observable.CombineLatest(combinedCanExecuteObservables)
                .Select(arr => arr.AsValueEnumerable().All(c => c))
                .DistinctUntilChanged()
                .Subscribe(isOn => _isOn.Value = isOn)
                .AddTo(this);
        }

        private void SubscribeToButtonClick()
        {
            _buttonComponent.OnClickAsObservable()
                .Subscribe(_ => OnButtonClicked().Forget())
                .AddTo(this);
        }

        private async UniTask OnButtonClicked()
        {
            if (_actions.Length > 1)
            {
                _isExecutingListOfActions.Value = true;
            }

            foreach (var buttonAction in _actions.AsValueEnumerable())
            {
                if (!await buttonAction.Execute())
                    break;
            }

            _isExecutingListOfActions.Value = false;
        }
    }
}