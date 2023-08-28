﻿using CryptoQuest.Gameplay.Character;
using UnityEngine;
using UnityEngine.Assertions;

namespace CryptoQuest.Gameplay.PlayerParty
{
    /// <summary>
    /// I do not want to re-initialize the character when the party is sorted, Simply change the parent of the character
    /// to another slot.
    /// </summary>
    public class PartySlot : MonoBehaviour
    {
        [SerializeField] private CharacterBehaviourBase _character;
        public CharacterBehaviourBase Character => _character;

        public void Init(CharacterSpec character)
        {
            _character.Init(character);
        }
        
        public bool IsValid()
        {
            return _character != null && _character.Spec.IsValid();
        }
    }
}