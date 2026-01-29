using System;
using R3;
using UnityEngine;
using UnityEngine.UI;
using ZLinq;

namespace UI.Utils
{
    public class CustomToggleGroup : MonoBehaviour
    {
        [SerializeField] private CustomToggleGroupEntry[] entries;

        private readonly ReactiveProperty<CustomToggleGroupEntry> _selectedEntry = new();

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            var selectedByDefault =
                entries.AsValueEnumerable()
                    .FirstOrDefault(v => v.isSelectedByDefault) ?? entries[0];
            _selectedEntry.Value = selectedByDefault;
            foreach (var entry in entries)
            {
                InitializeEntry(entry);
            }

            _selectedEntry.Subscribe(v =>
                {
                    foreach (var entry in entries)
                    {
                        entry.selectedGraphics.SetActive(entry == v);
                        entry.unSelectedGraphics.SetActive(entry != v);
                    }
                })
                .AddTo(this);
        }

        private void InitializeEntry(CustomToggleGroupEntry entry)
        {
            entry.buttonComponent.OnClickAsObservable()
                .Subscribe(_ => _selectedEntry.Value = entry);
        }
    }

    [Serializable]
    public class CustomToggleGroupEntry
    {
        public bool isSelectedByDefault;
        public Button buttonComponent;
        public GameObject selectedGraphics;
        public GameObject unSelectedGraphics;
    }
}