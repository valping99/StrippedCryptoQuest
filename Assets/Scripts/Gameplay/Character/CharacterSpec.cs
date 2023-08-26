﻿using System;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.UI.Menu.Panels.Home;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Character
{
    /// <summary>
    /// Use this to save game
    /// </summary>
    [Serializable]
    public class CharacterSpec
    {
        [field: SerializeField] public CharacterBase BaseInfo { get; set; }
        [field: SerializeField] public Elemental Element { get; set; }
        [field: SerializeField] public int Level { get; set; }
        [field: SerializeField] public StatsDef StatsDef { get; set; }
        [field: SerializeField] public CharacterEquipments Equipments { get; private set; }
        public Sprite Avatar => BaseInfo.Avatar;

        private ICharacter _characterComponent;
        public ICharacter CharacterComponent => _characterComponent;

        public float GetValueAtCurrentLevel(AttributeScriptableObject attributeDef)
        {
            // find the index of the attributeDef in the StatsDef
            var def = Array.Find(StatsDef.Attributes, v => v.Attribute == attributeDef);
            return GetValueAtLevel(Level, def);
        }

        public float GetValueAtCurrentLevel(int attributeDefIndex)
        {
            // find the index of the attributeDef in the StatsDef
            return GetValueAtLevel(Level, attributeDefIndex);
        }

        public float GetValueAtLevel(int currentLvl, int attributeDefIndex) =>
            GetValueAtLevel(currentLvl, StatsDef.Attributes[attributeDefIndex]);

        public float GetValueAtLevel(int currentLvl, CappedAttributeDef attributeDef)
        {
            currentLvl = Mathf.Clamp(currentLvl, 1, StatsDef.MaxLevel);
            var value = attributeDef.MinValue;

            value = Mathf.Floor((attributeDef.MaxValue - attributeDef.MinValue) / StatsDef.MaxLevel * currentLvl) +
                    attributeDef.MinValue;

            return value;
        }

        public bool IsValid()
        {
            return BaseInfo != null
                   && Element != null
                   && StatsDef.Attributes.Length > 0;
        }

        public void Bind(ICharacter characterBehaviour)
        {
            _characterComponent = characterBehaviour;
        }

        public void SetupUI(ICharacterInfo uiCharacterInfo)
        {
            uiCharacterInfo.SetElement(Element.Icon);
            uiCharacterInfo.SetLevel(Level);
            BaseInfo.SetupUI(uiCharacterInfo);
        }
    }
}