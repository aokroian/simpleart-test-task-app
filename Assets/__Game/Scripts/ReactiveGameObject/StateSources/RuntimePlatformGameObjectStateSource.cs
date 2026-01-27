using Reflex.Attributes;
using UnityEngine;

namespace ReactiveGameObject.StateSources
{
    public class RuntimePlatformGameObjectStateSource : MonoGameObjectStateSource
    {
        [SerializeField] private RuntimePlatform targetPlatform;

        [Inject]
        private void Inject()
        {
            var currentPlatform = Application.platform;
            if (currentPlatform is RuntimePlatform.WindowsEditor
                or RuntimePlatform.OSXEditor
                or RuntimePlatform.LinuxEditor)
            {
                IsOnInner.Value = true;
                return;
            }

            IsOnInner.Value = currentPlatform == targetPlatform;
        }
    }
}