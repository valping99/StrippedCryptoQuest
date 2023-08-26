﻿using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.ActionTypes;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.UI.Menu.MenuStates.ItemStates;
using CryptoQuest.UI.Menu.Panels.Item.States;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class UIItemPresenter : MonoBehaviour, IActionPresenter
    {
        [SerializeField] private PresenterBinder _binder;
        [SerializeField] private PartySO _partySo;

        [Header("UI")]
        [SerializeField] private UIConsumableMenuPanel _uiConsumableMenuPanel;

        [SerializeField] private UIItemCharacterSelection _uiItemCharacterSelection;

        private UsableSO _item;

        private void Awake()
        {
            _uiConsumableMenuPanel.StateMachine.AddState(UISingleItemState.Item, new UISingleItemState(this));
            _binder.Bind(this);

            UIConsumableItem.Using += GetItem;
        }

        public void Show()
        {
            _uiConsumableMenuPanel.Interactable = false;
            _uiItemCharacterSelection.Init();

            _uiItemCharacterSelection.Clicked += ActiveAbility;
        }

        private void GetItem(UIConsumableItem currentItem)
        {
            _item = currentItem.Consumable.Data;
        }

        private void ActiveAbility(int index)
        {
            // TODO: REFACTORING PARTY SYSTEM
            // AbilitySystemBehaviour owner = _partySo.PlayerTeam.Members[index];
            //
            // CryptoQuestGameplayEffectSpec ability =
            //     (CryptoQuestGameplayEffectSpec)owner.MakeOutgoingSpec(_item.Ability.Effect);
            //
            // ability.SetParameters(_item.Ability.Info.SkillParameters);
            // owner.ApplyEffectSpecToSelf(ability);
            //
            // Hide();
        }

        public void Hide()
        {
            _uiConsumableMenuPanel.Interactable = true;
            _uiItemCharacterSelection.DeInit();

            _uiItemCharacterSelection.Clicked -= ActiveAbility;

            _uiConsumableMenuPanel.StateMachine.RequestStateChange(ItemMenuStateMachine.InventorySelection);
        }

        public void Execute()
        {
            _uiConsumableMenuPanel.StateMachine.RequestStateChange(UISingleItemState.Item);
        }
    }
}