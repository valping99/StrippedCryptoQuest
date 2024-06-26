﻿using System;
using CryptoQuest.Battle.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using UnityEngine;
using CharacterBehaviour = CryptoQuest.Battle.Components.Character;

namespace CryptoQuest.AbilitySystem.Abilities.PostNormalAttackPassive
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Passive/Condition Skill/Steal",
        fileName = "ConditionSkill")]
    public class StealPassiveAbility : PostNormalAttackPassiveBase
    {
        protected override GameplayAbilitySpec CreateAbility()
            => new StealPassiveSpec();
#if UNITY_EDITOR
        public override PostNormalAttackPassiveBase CreateInstance()
        {
            return (MultipleAttacksPassive)Activator.CreateInstance(this.GetType());
        }
#endif
    }

    public class StealPassiveSpec : PostNormalAttackPassiveSpecBase
    {
        private ActiveGameplayEffect _activeGameplayEffect;

        protected override void OnAttacked(DamageContext postAttackContext)
        {
            var target = postAttackContext.Target;
            if (!IsTargetValid(target)) return;
            var stealerBehaviour = Owner.GetComponent<IStealerBehaviour>();
            stealerBehaviour.Steal(target);
        }
    }
}