﻿using System;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Ability;
using CryptoQuest.Core;
using UnityEngine;
#if UNITY_EDITOR || !UNITY_WEBGL || !PLATFORM_WEBGL
#endif

namespace CryptoQuest.Character
{
    public class SqliteSkillsProvider : MonoBehaviour, ISkillsProvider
    {
#if UNITY_EDITOR || !UNITY_WEBGL || !PLATFORM_WEBGL
        [SerializeField] private Database _database;

        private void Start()
        {
            _database.Initialized += OnDatabaseInitialized;
        }

        private void OnDestroy()
        {
            _database.Initialized -= OnDatabaseInitialized;
        }

        private void OnDatabaseInitialized()
        {
            throw new NotImplementedException();
        }

        public void GetSkills(HeroBehaviour hero, Action<List<CastableAbility>> skillsLoadedCallback) { }
#endif
    }
}