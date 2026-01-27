using Cysharp.Threading.Tasks;
using Initialization.EntryPoint;
using R3;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Assertions;
using ZLinq;

namespace ReactiveGameObject
{
    public class ReactiveGameObject : MonoBehaviour
    {
        // todo: maybe add toggle strategy later (debounce, throttle, delay, etc.)
        private GameObjectToggler _toggler;
        private IGameObjectStateSource[] _sources;
        [Inject] private IEntryPoint _entryPoint;

        [Inject]
        private void Inject()
        {
            _toggler = GetComponent<GameObjectToggler>();
            Assert.IsNotNull(_toggler, "Did not find GameObjectToggler");
            _sources = GetComponents<IGameObjectStateSource>();
            Assert.IsTrue(_sources.Length > 0, "Did not find any GameObjectStateSource");
            _entryPoint.OnDone.Subscribe(_ => Initialize());
        }

        private void Initialize()
        {
            var sourcesObservables = _sources
                .AsValueEnumerable()
                .Select(s => s.IsOn)
                .ToList();

            Observable.CombineLatest(sourcesObservables)
                .Select(arr => arr.AsValueEnumerable().All(c => c))
                .DistinctUntilChanged()
                .Subscribe(React)
                .AddTo(this);
        }

        private void React(bool isOn)
        {
            _toggler.Toggle(isOn, destroyCancellationToken).Forget();
        }
    }
}