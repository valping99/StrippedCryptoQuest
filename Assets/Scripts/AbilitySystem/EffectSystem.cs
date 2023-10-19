﻿using System.Collections.Generic;
using System.Linq;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem
{
    public class EffectSystem : EffectSystemBehaviour
    {
        private readonly HashSet<GameplayEffectDefinition> _effectWithLargestMagnitude = new();

        public override void UpdateAttributeSystemModifiers()
        {
            _effectWithLargestMagnitude.Clear();
            AttributeSystem.ResetAttributeModifiers();
            foreach (var effect in AppliedEffects.Where(effect => !effect.Expired))
            {
                effect.Spec.CalculateModifierMagnitudes();
                if (_effectWithLargestMagnitude.Contains(effect.Spec.Def)) continue;
                var largestMagnitudeEffect = GetLargestGameplayEffectMagnitude(effect.Spec);
                _effectWithLargestMagnitude.Add(largestMagnitudeEffect.Spec.Def);
                largestMagnitudeEffect.ExecuteActiveEffect();
            }
        }

        private ActiveGameplayEffect GetLargestGameplayEffectMagnitude(GameplayEffectSpec spec)
        {
            var largestMagnitudeEffect = new ActiveGameplayEffect();
            foreach (var effect in AppliedEffects)
            {
                if (effect.Spec.Def != spec.Def) continue;
                if (!largestMagnitudeEffect.IsValid())
                {
                    largestMagnitudeEffect = effect;
                    continue;
                }

                for (var index = 0; index < effect.ComputedModifiers.Count; index++)
                {
                    var otherEffectEvaluatedMod = effect.ComputedModifiers[index];
                    var currentLargestEffectEvaluatedMod = largestMagnitudeEffect.ComputedModifiers[index];
                    if (Mathf.Abs(otherEffectEvaluatedMod.Magnitude) > Mathf.Abs(currentLargestEffectEvaluatedMod.Magnitude))
                    {
                        largestMagnitudeEffect = effect;
                        break;
                    }
                }
            }

            return largestMagnitudeEffect;
        }
    }
}