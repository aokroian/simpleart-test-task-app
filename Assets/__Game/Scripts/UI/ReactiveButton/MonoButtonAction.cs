using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ReactiveButton
{
    [RequireComponent(typeof(Button))]
    public abstract class MonoButtonAction : MonoBehaviour, IButtonAction
    {
        public ReadOnlyReactiveProperty<bool> IsExecuting => _isExecuting;
        public int Order => order;
        public Observable<bool> OnExecuted => _onExecuted;

        [SerializeField] private int order;

        private readonly ReactiveProperty<bool> _isExecuting = new();
        private readonly Subject<bool> _onExecuted = new();

        private void Awake()
        {
            // this is added to that we would be able to just put
            // actions on buttons without any other logic
            if (!GetComponent<ReactiveButton>())
            {
                GetComponent<Button>()
                    .OnClickAsObservable()
                    .Subscribe(_ => { Execute(destroyCancellationToken).Forget(); })
                    .AddTo(this);
            }
        }

        public async UniTask<bool> Execute(CancellationToken ct = default)
        {
            _isExecuting.Value = true;
            if (ct == CancellationToken.None)
                ct = destroyCancellationToken;
            bool result;
            try
            {
                result = await ExecuteInner(ct);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result = false;
                throw;
            }

            _isExecuting.Value = false;
            _onExecuted.OnNext(result);
            return result;
        }

        protected abstract UniTask<bool> ExecuteInner(CancellationToken ct);
    }
}