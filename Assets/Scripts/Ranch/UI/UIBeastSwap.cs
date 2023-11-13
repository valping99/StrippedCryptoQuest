using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Ranch.UI
{
    public class UIBeastSwap : UIAbstractFarm
    {
        [field: SerializeField] public UIBeastList WalletBeastList { get; private set; }
        [field: SerializeField] public UIBeastList InGameBeastList { get; private set; }

        [SerializeField] private Transform _walletBeatListContent;
        [SerializeField] private Transform _inGameBeatListContent;

        private List<int> _selectedInGameBeatIds = new();

        public List<int> SelectedInGameBeatIds
        {
            get => _selectedInGameBeatIds;
            private set => _selectedInGameBeatIds = value;
        }

        private List<int> _selectedWalletBeatIds = new();

        public List<int> SelectedWalletBeatIds
        {
            get => _selectedWalletBeatIds;
            private set => _selectedWalletBeatIds = value;
        }

        public void Transfer(UIBeastItem item)
        {
            Transform currentList = item.Parent;

            if (currentList == _inGameBeatListContent)
            {
                item.Transfer(_walletBeatListContent);
                _selectedInGameBeatIds.Add(item.Id);
            }
            else
            {
                item.Transfer(_inGameBeatListContent);
                _selectedWalletBeatIds.Add(item.Id);
            }


            InGameBeastList.SetEnableButtons(!(currentList == _inGameBeatListContent));
            WalletBeastList.SetEnableButtons(currentList == _inGameBeatListContent);

            Debug.Log($"game={_selectedInGameBeatIds.Count} -- wallet={_selectedWalletBeatIds.Count}");
        }

        public void SwitchList(Vector2 direction)
        {
            if (_inGameBeatListContent.childCount <= 0) return;
            if (_walletBeatListContent.childCount <= 0) return;

            switch (direction.x)
            {
                case > 0:
                    InGameBeastList.SetEnableButtons(false);
                    FocusList(WalletBeastList);
                    break;
                case < 0:
                    WalletBeastList.SetEnableButtons(false);
                    FocusList(InGameBeastList);
                    break;
            }
        }

        private void FocusList(UIBeastList list)
        {
            list.SetEnableButtons();
            list.SelectDefault();
        }

        public void Focus()
        {
            switch (_walletBeatListContent.childCount)
            {
                case > 0:
                    WalletBeastList.SetEnableButtons(false);
                    InGameBeastList.SelectDefault();
                    break;
                case <= 0:
                    WalletBeastList.SelectDefault();
                    break;
            }
        }

        public void ConfirmedTransmission()
        {
            InGameBeastList.UpdateList();
            WalletBeastList.UpdateList();
        }
    }
}