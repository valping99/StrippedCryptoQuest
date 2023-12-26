﻿using CryptoQuest.ShopSystem.Sagas;
using CryptoQuest.UI.Dialogs.ChoiceDialog;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

namespace CryptoQuest.ShopSystem
{
    public class BuyEquipmentPresenter : MonoBehaviour
    {
        [SerializeField] private BuyEquipmentModel _model;
        [SerializeField] private UIBuyEquipmentList _uiSellingList;
        [SerializeField] private LocalizedString _strConfirmBuy = new("ShopUI", "DIALOG_BUY_CONFIRM");
        [SerializeField] private BuyPanel _buyPanel;

        private UIChoiceDialog _confirmDialog;

        private void Awake()
        {
            ChoiceDialogController.Instance.InstantiateAsync(dialog => _confirmDialog = dialog);
        }

        private void OnEnable()
        {
            _uiSellingList.BuyingEquipment += BuyEquipment;
            _uiSellingList.Render(_model.SellingItems);
        }

        private void OnDisable()
        {
            _uiSellingList.BuyingEquipment -= BuyEquipment;
        }

        private void BuyEquipment(UIEquipmentShopItem item)
        {
            _buyPanel.enabled = false;
            var selectables = _uiSellingList.GetComponentsInChildren<Selectable>();
            foreach (var selectable in selectables) selectable.interactable = false;
            _strConfirmBuy["PRICE"] = new StringVariable() { Value = item.PriceText };
            _confirmDialog
                .WithNoCallback(() => EventSystem.current.SetSelectedGameObject(item.gameObject))
                .WithYesCallback(() => { ActionDispatcher.Dispatch(new BuyEquipmentAction(item.Info)); })
                .WithHideCallback(() =>
                {
                    foreach (var selectable in selectables) selectable.interactable = true;
                    EventSystem.current.SetSelectedGameObject(item.gameObject);
                    _buyPanel.enabled = true;
                })
                .SetMessage(_strConfirmBuy)
                .Show();
            var button = item.GetComponent<Button>();
            button.image.overrideSprite = button.spriteState.pressedSprite;
        }
    }
}