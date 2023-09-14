﻿using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using UnityEngine;

namespace CryptoQuest.Character.Attributes
{
    [CreateAssetMenu(menuName = "Crypto Quest/Character/Elemental Attribute")]
    public class ElementalAttribute : AttributeScriptableObject
    {
        public override AttributeValue CalculateInitialValue(
            AttributeValue attributeValue,
            List<AttributeValue> otherAttributeValues)
        {
            var clone = attributeValue.Clone();
            clone.BaseValue = clone.CurrentValue = 1;
            return clone;
        }
    }
}