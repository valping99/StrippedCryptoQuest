﻿using System.Collections.Generic;
using CryptoQuest.Gameplay.PlayerParty;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.ActionTypes
{
    public class GroupAllAlliesAction : ActionDefinitionBase
    {
        [SerializeField] private PartySO _party;

        protected override ActionSpecificationBase CreateInternal()
        {
            return new GroupActionSpec(_party);
        }
    }

    public class GroupActionSpec : ActionSpecificationBase
    {
        private PartySO _party;

        public GroupActionSpec(PartySO party)
        {
            _party = party;
        }

        protected override void OnExecute()
        {
            UsableInfo item = ActionContext.Item;
            List<AbilitySystemBehaviour> members = _party.PlayerTeam.Members;

            foreach (var owner in members)
            {
                CryptoQuestGameplayEffectSpec ability =
                    (CryptoQuestGameplayEffectSpec)owner.MakeOutgoingSpec(item.Item.Ability.Effect);
                
                owner.ApplyEffectSpecToSelf(ability);
            }
        }
    }
}