using System.Collections.Generic;
using UnityEngine;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace IndiGames.GameplayAbilitySystem
{
    [CreateAssetMenu(fileName = "TargetContainter", menuName = "Indigames Ability System/Abilities/Target Containter")]
    public class TargetContainterSO : ScriptableObject
    {
        public List<AbilitySystemBehaviour> Targets {get; set;} = new();

        public void SetSingleTarget(AbilitySystemBehaviour target)
        {
            ResetTargets();
            Targets.Add(target);
        }
        
        public void SetMultipleTargets(List<AbilitySystemBehaviour> targets)
        {
            ResetTargets();
            Targets.AddRange(targets);
        }

        public void ResetTargets()
        {
            Targets.Clear();
        } 
    }
}