using System;
using TownTransfer;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace CryptoQuest.TownTransfer.UI
{
    public class UITownTransferOptionButton : MonoBehaviour
    {
        public event Action<TownTransferPath> Clicked;
        [SerializeField] private LocalizeStringEvent _townName;

        private TownTransferPath _location;

        public void SetTownName(TownTransferPath location)
        {
            _location = location;
            _townName.StringReference = location.MapName;
        }

        public void OnClicked()
        {
            Debug.Log($"TransferTownButton Clicked {_location}");
            Clicked?.Invoke(_location);
        }
    }
}