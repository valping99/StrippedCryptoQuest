using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Data
{
    public class ItemGenericSO : ScriptableObject
    {
        public int ID;

        [field: SerializeField] public LocalizedString DisplayName { get; private set; }
        public LocalizedString Description;
        [field: SerializeField] public Sprite Icon { get; private set; }

        [field: SerializeField] public bool IsNftItem { get; private set; }

        public void SetIcon(Sprite icon)
        {
            this.Icon = icon;
        }
    }
}