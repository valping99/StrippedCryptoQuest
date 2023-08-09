using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    public class ItemGenericSO : ScriptableObject
    {
        [Header("Item Generic Data")]
        public string ID;

        [field: SerializeField] public LocalizedString DisplayName { get; private set; }
        [field: SerializeField] public LocalizedString Description { get; private set; }
        [field: SerializeField] public Sprite Image { get; private set; }
        [field: SerializeField] public UsableActiveTypeSO Type { get; private set; }

        public void SetImage(Sprite icon)
        {
            this.Image = icon;
        }
    }
}