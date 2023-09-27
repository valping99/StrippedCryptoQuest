﻿using System;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Gameplay.Character;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using UnityEngine;
using AttributeScriptableObject = IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects.AttributeScriptableObject;

namespace CryptoQuest.Gameplay
{
    /// <summary>
    /// Should be a component on scene so that we can use the update
    /// </summary>
    [Obsolete]
    public class CharacterBehaviourBase : MonoBehaviour
    {
        [SerializeField] private AttributeScriptableObject _hpAttribute;
        [field: SerializeField] public AbilitySystemBehaviour GameplayAbilitySystem { get; private set; }
        [field: SerializeField] public EffectSystemBehaviour EffectSystem { get; private set; }
        [field: SerializeField] public AttributeSystemBehaviour AttributeSystem { get; private set; }
        [SerializeField] private CharacterSpec _spec = new();
        public Elemental Element => _spec.Element;
        public CharacterSpec Spec => _spec;

        private void OnValidate()
        {
            if (GameplayAbilitySystem == null) GameplayAbilitySystem = GetComponent<AbilitySystemBehaviour>();
            if (EffectSystem == null) EffectSystem = GetComponent<EffectSystemBehaviour>();
            if (AttributeSystem == null) AttributeSystem = GetComponent<AttributeSystemBehaviour>();
        }

        /// <summary>
        /// Then we will need to add stats such as ATK, DEF, etc. these need to init after the base stats so when  <see cref="AttributeScriptableObject.CalculateInitialValue"/> get called, it will have the base stats value
        /// </summary>
        public virtual void Init(CharacterSpec character)
        {
            if (character.IsValid() == false) return;

            // should I clone this? because it's currently the same object in PartySO
            // if this get modified the spec in party SO will too
            _spec = character;
            _spec.Init(this);

            var components = GetComponents<ICharacterComponent>();
            foreach (var comp in components)
            {
                comp.Init(this);
            }
        }

        public bool IsDead()
        {
            if (!AttributeSystem.TryGetAttributeValue(_hpAttribute, out var hpValue))
                return true;

            return hpValue.CurrentValue <= 0;
        }
    }
}