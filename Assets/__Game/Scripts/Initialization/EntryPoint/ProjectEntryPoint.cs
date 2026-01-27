using System;
using Cysharp.Threading.Tasks;
using R3;
using Reflex.Attributes;
using UI.Routing;
using UnityEngine;

namespace Initialization.EntryPoint
{
    public class ProjectEntryPoint : IEntryPoint
    {
        public Observable<Unit> OnDone
        {
            get
            {
                _onDone ??= _isDone
                    .Where(v => v)
                    .Take(1)
                    .Select(_ => Unit.Default);
                return _onDone;
            }
        }

        private readonly ReactiveProperty<bool> _isDone = new();
        private Observable<Unit> _onDone;

        [Inject] private readonly UIRoutingService _uiRoutingService;

        [Inject]
        private async UniTask Inject()
        {
            var isGood = await InitializeApp();
            if (isGood)
            {
                _isDone.Value = true;
                await PostInitializeSetup();
            }
        }

        private async UniTask<bool> InitializeApp()
        {
            try
            {
                await _uiRoutingService.InitializeAsync();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }

            return true;
        }

        private UniTask PostInitializeSetup()
        {
            return UniTask.CompletedTask;
        }
    }
}