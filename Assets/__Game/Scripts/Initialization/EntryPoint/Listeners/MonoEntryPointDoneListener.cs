using System;
using Cysharp.Threading.Tasks;
using R3;
using Reflex.Attributes;
using UnityEngine;

namespace Initialization.EntryPoint.Listeners
{
    public abstract class MonoEntryPointDoneListener : MonoBehaviour
    {
        [Inject] private IEntryPoint _entryPoint;

        protected abstract UniTask OnEntryPointDone();

        [Inject]
        private void Inject()
        {
            _entryPoint.OnDone.Subscribe(OnEntryPointDoneInner).AddTo(this);
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