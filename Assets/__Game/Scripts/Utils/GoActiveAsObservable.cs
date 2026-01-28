using R3;
using R3.Triggers;
using UnityEngine;

namespace Utils
{
    public class GoActiveAsObservable : MonoBehaviour
    {
        public ReadOnlyReactiveProperty<bool> IsActiveInHierarchy => _isActiveInHierarchy;
        private readonly ReactiveProperty<bool> _isActiveInHierarchy = new();

        private bool _isInitialized;
        
        public void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }
            _isActiveInHierarchy.Value = gameObject.activeInHierarchy;
            gameObject.OnEnableAsObservable()
                .Subscribe(_ => _isActiveInHierarchy.Value = true)
                .AddTo(this);
            gameObject.OnDisableAsObservable()
                .Subscribe(_ => _isActiveInHierarchy.Value = false)
                .AddTo(this);
            _isInitialized = true;
        }

        private void Awake()
        {
            Initialize();
        }
    }
}