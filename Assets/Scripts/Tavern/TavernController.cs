﻿using System;
using CryptoQuest.Character.Hero;
using CryptoQuest.Input;
using CryptoQuest.Inventory;
using CryptoQuest.Tavern.States;
using CryptoQuest.Tavern.UI;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Tavern
{
    public class TavernController : MonoBehaviour
    {
        public UnityAction ExitTavernEvent;

        [SerializeField] private Animator _stateMachine;
        [field: SerializeField] public HeroSpecInitializer SpecInitializer { get; private set; }

        [field: Header("Managers")]
        [field: SerializeField] public MerchantsInputManager MerchantInputManager { get; private set; }
        [field: SerializeField] public TavernDialogsManager DialogsManager { get; private set; }

        [field: Header("Overview State")]
        [field: SerializeField] public UIOverview TavernUiOverview { get; private set; }

        [field: Header("Character Replacement State")]
        [field: SerializeField] public UICharacterReplacement UICharacterReplacement { get; private set; }

        [field: SerializeField] public UICharacterList UIGameList { get; private set; }
        [field: SerializeField] public UICharacterList UIDboxList { get; private set; }

        [field: Header("Party Organization State")]
        [field: SerializeField] public UIPartyOrganization UIPartyOrganization { get; private set; }

        [field: SerializeField] public UICharacterList UIParty { get; private set; }
        [field: SerializeField] public UICharacterList UINonParty { get; private set; }

        private void OnDisable()
        {
            var behaviours = _stateMachine.GetBehaviours<StateMachineBehaviourBase>();
            foreach (var behaviour in behaviours) behaviour.Exit();
        }
    }
}