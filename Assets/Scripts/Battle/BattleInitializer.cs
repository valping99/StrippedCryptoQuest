﻿using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Character.Enemy;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public interface IBattleInitializer
    {
        public IEnumerator LoadEnemies();
        public List<EnemySpec> Enemies { get; }
    }

    public class BattleInitializer : MonoBehaviour, IBattleInitializer
    {
        [SerializeField] private BattleBus _bus;
        [SerializeField] private EnemyPartyBehaviour _enemyPartyBehaviour;
        [SerializeField] private EnemyDatabase _enemyDatabase;
        public List<EnemySpec> Enemies => _loadedEnemies;
        private readonly List<EnemySpec> _loadedEnemies = new();

        public IEnumerator LoadEnemies()
        {
            yield return LoadEnemiesAssets();
            _enemyPartyBehaviour.Init(_loadedEnemies);
        }

        private IEnumerator LoadEnemiesAssets()
        {
            _loadedEnemies.Clear();
            var enemyParty = _bus.CurrentBattlefield;
            for (var index = 0; index < enemyParty.EnemyIds.Length; index++)
            {
                var enemyId = enemyParty.EnemyIds[index];
                yield return _enemyDatabase.LoadDataById(enemyId);
                var def = _enemyDatabase.GetDataById(enemyId);
                if (def == null)
                {
                    // TODO: Create mock enemy instead of skipping?
                    Debug.LogError($"failed to load enemy data with id {enemyId}, skipping...");
                    continue;
                }

                _loadedEnemies.Add(def.CreateCharacterSpec()); // TODO: UNLOAD ENEMY DATA
            }
        }
    }
}