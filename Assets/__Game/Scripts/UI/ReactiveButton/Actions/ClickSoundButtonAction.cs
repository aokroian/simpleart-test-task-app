using System.Threading;
using Constants;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.ReactiveButton.Actions
{
    public class ClickSoundButtonAction : MonoButtonAction
    {
        [SerializeField] private string soundID = Const.Sounds.ButtonClick;

        private AudioClip _clip;
        private static Camera _mainCam;

        protected override UniTask<bool> ExecuteInner(CancellationToken ct)
        {
            if (!_clip)
            {
                _clip = Resources.Load<AudioClip>(soundID);
            }

            if (_mainCam == null)
            {
                _mainCam = Camera.main;
            }

            AudioSource.PlayClipAtPoint(_clip, _mainCam!.transform.position);

            return UniTask.FromResult(true);
        }
    }
}