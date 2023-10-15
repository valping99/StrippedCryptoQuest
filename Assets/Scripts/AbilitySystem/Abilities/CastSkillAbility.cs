﻿using System;
using System.Collections;
using System.Linq;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.Battle.Core;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CryptoQuest.AbilitySystem.Abilities
{
    /// <summary>
    /// https://docs.google.com/spreadsheets/d/1EDrXrT1if63TLam1km_dLx2VvHt2rks2OWXEUq5OEPg/edit#gid=2122798992
    /// Base data class for character skill
    ///
    /// Ability to cast an effect on targets (self, enemy, ally, ...) which cost mana to cast and has a chance to fail
    /// </summary>
    public abstract class CastSkillAbility : AbilityScriptableObject
    {
        [SerializeField] private GameplayEffectContext _context;
        public GameplayEffectContext Context => _context;
        [field: SerializeField, Obsolete] public SkillInfo Parameters { get; private set; }
        [field: SerializeField] public float SuccessRate { get; private set; } = 100;
        [SerializeField] private float _mpToCast = 3f;
        public float MpToCast => _mpToCast;
        [field: SerializeField] public SkillTargetType TargetType { get; private set; }
    }

    public abstract class CastSkillAbilitySpec : GameplayAbilitySpec
    {
        private GameplayEffectDefinition _costEffect;
        private readonly CastSkillAbility _def;

        public CastSkillAbilitySpec(CastSkillAbility def) => _def = def;

        public override void InitAbility(AbilitySystemBehaviour owner, AbilityScriptableObject abilitySO)
        {
            base.InitAbility(owner, abilitySO);

            _costEffect = ScriptableObject.CreateInstance<GameplayEffectDefinition>();
            _costEffect.Policy = new InstantPolicy();
            _costEffect.EffectDetails = new EffectDetails()
            {
                Modifiers = new EffectAttributeModifier[]
                {
                    new()
                    {
                        Attribute = AttributeSets.Mana,
                        OperationType = EAttributeModifierOperationType.Add,
                        Value = -_def.MpToCast
                    }
                }
            };
        }

        public override bool CanActiveAbility()
        {
            return base.CanActiveAbility() && CheckCost();
        }

        /// <summary>
        /// GameplayAbility.cpp::CheckCost line 906
        /// Create a cost effect spec, use the modifier and calculate the remaining resources, if the remaining resources
        /// is greater than 0 apply the effect
        /// </summary>
        /// <returns>Return true if after subtracted attribute that the cost needs greater than 0</returns>
        private bool CheckCost()
        {
            if (_costEffect == null) return true;
            if (Owner == null) return true;
            if (Owner.CanApplyAttributeModifiers(_costEffect)) return true;

            Debug.Log($"Not enough {_costEffect.EffectDetails.Modifiers[0].Attribute.name} to cast this ability");
            BattleEventBus.RaiseEvent(new MpNotEnoughEvent());
            return false;
        }

        private bool CanCast()
        {
            var roll = Random.Range(0, 100);
            var result = roll < _def.SuccessRate;
            var resultMessage = result ? "Success" : "Failed";
            Debug.Log($"Casting {_def.name} with success rate {_def.SuccessRate} and roll {roll}: {resultMessage}");
            if (!result)
            {
                Debug.Log($"Failed to cast {_def.name}");
                BattleEventBus.RaiseEvent(new CastSkillFailedEvent());
            }

            return result;
        }

        private void ApplyCost()
        {
            if (_costEffect == null) return;

            ApplyGameplayEffectToOwner(_costEffect);
        }

        private AbilitySystemBehaviour[] _targets;
        protected AbilitySystemBehaviour[] Targets => _targets;

        public void Execute(params AbilitySystemBehaviour[] characters)
        {
            _targets = characters;
            TryActiveAbility();
        }

        protected override IEnumerator OnAbilityActive()
        {
            ApplyCost();
            if (CanCast() == false) yield break;
            
            foreach (var target in Targets)
            {
                if (IsTargetEvaded(target))
                {
                    BattleEventBus.RaiseEvent(new MissedEvent());
                    continue;
                }

                yield return InternalExecute(target);
            }
        }
        
        /// <summary>
        /// After all check has passed cost, cast success rate, evade, ... then execute the ability
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected abstract IEnumerator InternalExecute(AbilitySystemBehaviour target);


        private ActiveGameplayEffect GetActiveEffectWithTag(AbilitySystemBehaviour target, TagScriptableObject tag)
        {
            var appliedEffects = target.EffectSystem.AppliedEffects;
            return appliedEffects.FirstOrDefault(x => x.GrantedTags.Contains(tag));
        }


        private bool IsTargetEvaded(AbilitySystemBehaviour target)
        {
            var evadeBehaviour = target.GetComponent<IEvadable>();
            return evadeBehaviour != null && evadeBehaviour.TryEvade();
        }
    }
}