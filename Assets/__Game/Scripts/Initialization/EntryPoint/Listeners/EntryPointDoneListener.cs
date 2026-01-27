using System;
using Cysharp.Threading.Tasks;
using R3;
using Reflex.Attributes;
using UnityEngine;

namespace Initialization.EntryPoint.Listeners
{
    public abstract class EntryPointDoneListener
    {
        [Inject] private IEntryPoint _entryPoint;

        protected abstract UniTask OnEntryPointDone();

        [Inject]
        private void Inject()
        {
            _entryPoint.OnDone.Subscribe(OnEntryPointDoneInner);
        }

        private void OnEntryPointDoneInner(Unit _)
        {
            try
            {
                OnEntryPointDone().Forget();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }
    }
}