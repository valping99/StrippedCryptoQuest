using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace CryptoQuest.Item.Consumable
{
    [Serializable]
    public class ConsumableInfo : ItemInfo<ConsumableSO>
    {
        public event Action<ConsumableInfo> QuantityChanged;

        [SerializeField] private int _quantity = 1;

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                QuantityChanged?.Invoke(this);
            }
        }

        public AssetReferenceT<Sprite> Icon => Data.Image;
        public LocalizedString DisplayName => Data.DisplayName;
        public LocalizedString Description => Data.Description;

        public ConsumableInfo(ConsumableSO baseItemSO, int quantity = 1) : base(baseItemSO)
        {
            _quantity = quantity;
        }

        public ConsumableInfo() { }

        [Obsolete]
        public void SetQuantity(int quantity)
        {
            _quantity = quantity;
        }

        public void Consuming()
        {
            // I can pass this into the event
            Data.TargetSelectionEvent.RaiseEvent();
        }

        public override bool IsValid()
        {
            if (Data == null)
            {
                Debug.LogWarning("Consumable doesn't have Data");
                return false;
            }

            if (Quantity <= 0)
            {
                Debug.LogWarning($"Consumable {Data} is less than 0");
                return false;
            }

            return base.IsValid();
        }
    }
}