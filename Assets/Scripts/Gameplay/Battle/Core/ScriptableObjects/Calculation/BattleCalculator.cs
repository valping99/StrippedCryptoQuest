using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation
{
    public static class BattleCalculator
    {
        public static float CalculateBaseDamage(SkillParameters skillParameters, float attackPower, float modifierScale)
        {
            float damage =
                (skillParameters.BasePower + (attackPower - skillParameters.SkillPowerThreshold) *
                    skillParameters.PowerValueAdded);
            damage = Mathf.Clamp(damage, skillParameters.PowerLowerLimit, skillParameters.PowerUpperLimit);
            float baseDamage = damage + damage * modifierScale;
            return baseDamage;
        }

        public static float CalculateProbabilityOfRetreat(float targetMaxAttributeValue, float ownerAttributeValue)
        {
            return (50 - 50 * (targetMaxAttributeValue - ownerAttributeValue) / 100) / 100;
        }

        public static float CalculateEncounterRateBuff(float buff, float passiveBuff)
        {
            bool isZeroBuff = (buff == 0 && passiveBuff == 0);
            float buffDividen = isZeroBuff ? 1 : ((1 - buff) * (1 - passiveBuff));
            float buffRate = (buffDividen != 0) ? (1 / buffDividen) : 1;
            return buffRate;
        }

        public static float CalculatePercentPhysicalDamage(float baseDamage, float attack, float defence,
            float elementalRate)
        {
            float atkCorrection = BaseBattleVariable.CORRECTION_ATTACK_VALUE != 0
                ? BaseBattleVariable.CORRECTION_ATTACK_VALUE
                : 1;
            float defCorrection = BaseBattleVariable.CORRECTION_ATTACK_VALUE != 0
                ? BaseBattleVariable.CORRECTION_DEFENSE_VALUE
                : 1;
            float damage = ((attack / atkCorrection) - (defence / defCorrection)) * (baseDamage / 100) *
                           elementalRate;
            damage = damage < 0 ? 0 : damage;
            Debug.Log("baseDamage " + baseDamage + "attack " + attack + "defence " + defence + "elemental rate " +
                      elementalRate);
            return damage;
        }

        public static float CalculateFixedPhysicalDamage(float baseDamage, float elementalRate)
        {
            float damage = baseDamage * elementalRate;
            damage = damage < 0 ? 0 : damage;
            Debug.Log("baseDamage " + baseDamage + "elemental rate" + elementalRate);
            return damage;
        }

        public static float CalculateElementalRateFromParams(CustomExecutionParameters executionParams)
        {
            var character = executionParams.SourceAbilitySystemComponent.GetComponent<Character>();
            var characterElemental = character.Element;
            executionParams.TryGetAttributeValue(new CustomExecutionAttributeCaptureDef()
            {
                Attribute = characterElemental.AttackAttribute,
                CaptureFrom = EGameplayEffectCaptureSource.Source
            }, out var elementalAtk, 1f);
            executionParams.TryGetAttributeValue(new CustomExecutionAttributeCaptureDef()
            {
                Attribute = characterElemental.ResistanceAttribute,
                CaptureFrom = EGameplayEffectCaptureSource.Target
            }, out var elementalDef, 1f);

            Debug.Log("elemental attack" + elementalAtk.CurrentValue
                                         + " elemental def" + elementalDef.CurrentValue);
            var elementalRate = elementalAtk.CurrentValue / elementalDef.CurrentValue;
            elementalRate = elementalRate == 0 ? 1 : elementalRate;
            return elementalRate;
        }

        public static float GetEffectTypeValueCorrection(EEffectType effectType)
        {
            var mod = 1f;
            switch (effectType)
            {
                case EEffectType.Damage:
                case EEffectType.DeBuff:
                    mod = -1f;
                    break;
                default:
                    mod = 1f;
                    break;
            }

            return mod;
        }

        public static float CorrectAttack(this float attackPowerValue) =>
            attackPowerValue / BaseBattleVariable.CORRECTION_ATTACK_VALUE;

        public static float CorrectDefense(this float defenceValue) =>
            defenceValue / BaseBattleVariable.CORRECTION_DEFENSE_VALUE;
    }

    public static class BaseBattleVariable
    {
        public const float CORRECTION_ATTACK_VALUE = 2;
        public const float CORRECTION_DEFENSE_VALUE = 4;
        public const float CORRECTION_MAGIC_ATTACK_VALUE = 0.2f;
        public const float CORRECTION_ATTRIBUTE_VALUE = 1;
        public const float CORRECTION_PROBABILITY_VALUE = 100;
    }
}