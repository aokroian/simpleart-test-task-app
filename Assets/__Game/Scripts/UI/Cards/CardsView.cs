using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Initialization.EntryPoint.Listeners;
using UnityEngine;

namespace UI.Cards
{
    public class CardsView : MonoEntryPointDoneListener
    {
        [SerializeField] private Transform cardsParent;
        [SerializeField] private CardView cardPrefab;

        private readonly HashSet<CardView> _cardViewsPool = new();

        protected override UniTask OnEntryPointDone()
        {
            return UniTask.CompletedTask;
        }
    }
}