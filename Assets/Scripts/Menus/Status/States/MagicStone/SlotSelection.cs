﻿using System.Collections.Generic;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Menus.Status.UI;
using CryptoQuest.Menus.Status.UI.MagicStone;
using CryptoQuest.Sagas.Equipment;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Menus.Status.States.MagicStone
{
    public class SlotSelection : StatusStateBase
    {
        private UIEquipmentDetails _uiEquipmentDetails;

        public SlotSelection(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            StatusPanel.Input.MenuCancelEvent += BackToEquipmentSelection;
            StatusPanel.UIAttachList.AttachSlotSelectedEvent += ToNavigatingBetweenElements;

            _uiEquipmentDetails = StatusPanel.MagicStoneMenu.GetComponentInChildren<UIEquipmentDetails>();

            StatusPanel.UIAttachList.EnterSlotSelection();
            StatusPanel.UIAttachList.RenderCurrentAttachedStones(_uiEquipmentDetails.Equipment);
        }

        public override void OnExit()
        {
            StatusPanel.UIAttachList.ExitSlotSelection();
            StatusPanel.Input.MenuCancelEvent -= BackToEquipmentSelection;
            StatusPanel.UIAttachList.AttachSlotSelectedEvent -= ToNavigatingBetweenElements;
        }

        private void ToNavigatingBetweenElements(IMagicStone stoneData)
        {
            CallDetachAPI(stoneData);
            fsm.RequestStateChange(State.MAGIC_STONE_ELEMENT_NAVIGATION);
        }

        private void CallDetachAPI(IMagicStone stoneData)
        {
            if (stoneData == null) return;
            List<int> stoneIDs = new();
            Debug.Log($"<color=white>CallDetachAPI::stoneIDs={stoneIDs}</color>");
            stoneIDs.Add(stoneData.ID);
            ActionDispatcher.Dispatch(new DetachStones()
            {
                EquipmentID = _uiEquipmentDetails.Equipment.Id,
                StoneIDs = stoneIDs
            });
        }

        private void BackToEquipmentSelection()
        {
            StatusPanel.MagicStoneMenu.SetActive(false);
            StatusPanel.BackToPreviousState();
        }
    }
}