﻿using CryptoQuest.Character.Attributes;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Inventory.Items;
using NUnit.Framework;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Character.Stats
{
    [TestFixture]
    public class DefaultAttributeFromLevelCalculatorTests
    {
        /// <summary>
        /// With the current formula, the value at level 10 should be 186.816 but it floor to 186 instead
        /// This cause the player .816 HP less than expected
        /// </summary>
        [Test]
        public void GetValueAtLevel_WithLevel10MinHP155MaxHP470_ShouldReturn185()
        {
            var defaultLevelCalculator = new DefaultAttributeFromLevelCalculator();
            var cappedAttributeDef = new CappedAttributeDef()
            {
                Attribute = ScriptableObject.CreateInstance<AttributeScriptableObject>(),
                MinValue = 155,
                MaxValue = 470,
            };
            var attributeDefs = new CappedAttributeDef[]
            {
                cappedAttributeDef
            };
            var stats = new StatsDef()
            {
                Attributes = attributeDefs,
                MaxLevel = 99
            };

            var value = defaultLevelCalculator.GetValueAtLevel(10, cappedAttributeDef, stats.MaxLevel);

            Assert.AreEqual(186, value);
        }

        [TestCase(0, 10, 45f, 75f,  48f)]
        [TestCase(3, 10, 45f, 75f,  54f)]
        public void GetValueAtLevel(int lvl, int maxLvl, float minVal, float maxVal, float expected)
        {
            var defaultLevelCalculator = new DefaultAttributeFromLevelCalculator();
            var cappedAttributeDef = new CappedAttributeDef()
            {
                Attribute = ScriptableObject.CreateInstance<AttributeScriptableObject>(),
                MinValue = minVal,
                MaxValue = maxVal,
            };

            var value = defaultLevelCalculator.GetValueAtLevel(lvl, cappedAttributeDef, maxLvl);

            Assert.AreEqual(expected, value);
        }
    }
}