﻿using CryptoQuest.Item.Consumable;
using CryptoQuest.ShopSystem.Sagas;
using CryptoQuest.UI.Dialogs.ChoiceDialog;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

namespace CryptoQuest.ShopSystem.Buy.Consumable
{
    public class BuyConsumablePresenter : MonoBehaviour
    {
        [SerializeField] private int _maxQuantityToBuyPerOp = 99;
        [SerializeField] private UIQuantityDialog _quantityConfigDialog;
        [SerializeField] private BuyConsumableModel _model;
        [SerializeField] private UIBuyConsumableList _uiBuyableList;
        [SerializeField] private LocalizedString _strConfirmBuy = new("ShopUI", "DIALOG_BUY_CONFIRM");
        [SerializeField] private BuyPanel _buyPanel;

        private UIChoiceDialog _confirmDialog;

        private void Awake()
        {
            ChoiceDialogController.Instance.InstantiateAsync(dialog => _confirmDialog = dialog);
        }

        private void OnEnable()
        {
            _uiBuyableList.BuyingConsumable += BuyConsumable;
            _uiBuyableList.Render(_model.SellingConsumables);
        }

        private void OnDisable()
        {
            _uiBuyableList.BuyingConsumable -= BuyConsumable;
        }

        private void BuyConsumable(UIConsumableShopItem item)
        {
            _buyPanel.enabled = false;
            var selectables = _uiBuyableList.GetComponentsInChildren<Selectable>();
            foreach (var selectable in selectables) selectable.interactable = false;
            
            _quantityConfigDialog.gameObject.SetActive(true);
            _quantityConfigDialog
                .WithQuantityChangedCallback(quantity =>
                {
                    _strConfirmBuy["PRICE"] = new StringVariable { Value = $"{item.Price * quantity}G" };
                    _confirmDialog.SetMessage(_strConfirmBuy);
                })
                .Show(_maxQuantityToBuyPerOp);

            _confirmDialog
                .WithNoCallback(() => EventSystem.current.SetSelectedGameObject(item.gameObject))
                .WithYesCallback(() =>
                {
                    var selectedQuantity = _quantityConfigDialog.CurrentQuantity;
                    var consumableInfo = new ConsumableInfo(item.Info.Data, selectedQuantity);
                    ActionDispatcher.Dispatch(new BuyConsumableAction(consumableInfo));
                })
                .WithHideCallback(() =>
                {
                    _quantityConfigDialog.Hide();
                    _quantityConfigDialog.gameObject.SetActive(false);
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