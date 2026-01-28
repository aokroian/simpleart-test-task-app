using Cysharp.Threading.Tasks;
using Initialization.EntryPoint.Listeners;
using TMPro;
using UnityEngine;

namespace UI.Cards
{
    public class CardsFilterButtonView : MonoEntryPointDoneListener
    {
        [SerializeField] private TextMeshProUGUI[] labelTextComponents;
        [SerializeField] private GameObject selectedGraphics;

        protected override UniTask OnEntryPointDone()
        {
            return UniTask.CompletedTask;
        }
    }
}