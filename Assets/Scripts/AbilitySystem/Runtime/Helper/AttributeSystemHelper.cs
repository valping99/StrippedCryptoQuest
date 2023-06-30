using System.Collections.Generic;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    public static class AttributeSystemHelper
    {
        public static AttributeValue CalculateCurrentAttributeValue(AttributeValue attributeValue)
        {
            float overridingValue = attributeValue.Modifier.Overriding + attributeValue.CoreModifier.Overriding;
            if (overridingValue != 0)
            {
                attributeValue.CurrentValue = overridingValue;
                return attributeValue;
            }

            var coreValue = CaculateCoreAttributeValue(attributeValue);

            attributeValue.CurrentValue = (coreValue + attributeValue.Modifier.Additive) *
                                          (attributeValue.Modifier.Multiplicative + 1);
            return attributeValue;
        }

        public static float CaculateCoreAttributeValue(AttributeValue attributeValue)
        {
            var coreModifier = attributeValue.CoreModifier;

            if (coreModifier.Overriding != 0)
            {
                return coreModifier.Overriding;
            }
            return (attributeValue.BaseValue + coreModifier.Additive) *
                (coreModifier.Multiplicative + 1);
        }
    }
}
