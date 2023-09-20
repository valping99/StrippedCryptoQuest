﻿using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public abstract class CharacterComponentBase : MonoBehaviour
    {
        private ICharacter _character;
        public ICharacter Character => _character;

        private void Awake()
        {
            _character = GetComponent<ICharacter>();
        }

        public abstract void Init();
    }
}