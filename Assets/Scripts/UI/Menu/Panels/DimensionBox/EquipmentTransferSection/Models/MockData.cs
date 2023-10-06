using System;
using UnityEngine;
using UnityEngine.Localization;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Models
{
    [Serializable]
    public struct MockData : IData
    {
        private AssetReferenceT<Sprite> _icon;
        private LocalizedString _name;
        private bool _isEquipped;

        public MockData(AssetReferenceT<Sprite> icon, LocalizedString name, bool isEquipped)
        {
            _icon = icon;
            _name = name;
            _isEquipped = isEquipped;
        }

        public AssetReferenceT<Sprite> GetIcon() { return _icon; }
        public LocalizedString GetLocalizedName() { return _name; }
        public bool IsEquipped() { return _isEquipped; }
    }
}