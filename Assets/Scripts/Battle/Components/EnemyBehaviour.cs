﻿using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Character.Enemy;
using CryptoQuest.Character.Tag;
using CryptoQuest.Gameplay.Loot;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using Spine.Unity;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.Battle.Components
{
    /// <summary>
    /// TODO: This name is too generic
    /// Simple stats provider which only return
    /// Attribute : Value
    /// </summary>
    public interface IStatsProvider
    {
        AttributeWithValue[] Stats { get; }
        void ProvideStats(AttributeWithValue[] attributeWithValues);
    }

    [DisallowMultipleComponent]
    public class EnemyBehaviour : Character, IStatsProvider
    {
        public AttributeWithValue[] Stats => _enemyDef.Stats;

        private string _displayName;
        public override string DisplayName => _displayName;
        private EnemySpec _spec = new();
        public EnemySpec Spec => _spec;


        private EnemyDef _enemyDef;
        public EnemyDef Def => _enemyDef;
        private GameObject _enemyModel;

        private SkeletonAnimation _skeletonAnimation;
        public SkeletonAnimation SkeletonAnimation => _skeletonAnimation;
        private AsyncOperationHandle<string> _localizedNameHandle;
        private AsyncOperationHandle<GameObject> _modelHandle;

        public void Init(EnemySpec enemySpec, string postfix)
        {
            // Data
            _spec = enemySpec;
            _enemyDef = _spec.Data;

            // Stats
            base.Init(_enemyDef.Element);

            // Visuals
            StartCoroutine(CoInstantiateModel());
            StartCoroutine(CoLoadName(postfix));
        }

        private IEnumerator CoInstantiateModel()
        {
            _modelHandle = _enemyDef.Model.InstantiateAsync(transform);
            yield return _modelHandle;
            _enemyModel = _modelHandle.Result;
            _skeletonAnimation = _enemyModel.GetComponentInChildren<SkeletonAnimation>();
        }

        private IEnumerator CoLoadName(string postfix)
        {
            if (_spec.Data.Name.IsEmpty)
            {
                Debug.LogWarning($"Localized string not set using default name {_spec.Data.Name}");
                _displayName = $"{_spec.Data.Name}{postfix}";
                yield break;
            }

            _localizedNameHandle = _spec.Data.Name.GetLocalizedStringAsync();
            yield return _localizedNameHandle;
            var loadedSuccess = _localizedNameHandle.IsValid() && _localizedNameHandle.IsDone &&
                                _localizedNameHandle.Result != null &&
                                _localizedNameHandle.Status == AsyncOperationStatus.Succeeded;
            if (!loadedSuccess)
            {
                Debug.LogWarning($"Failed to load localized string for enemy using default name {_spec.Data.Name}");
                _displayName = $"{_spec.Data.Name}{postfix}";
                yield break;
            }

            _displayName = $"{_localizedNameHandle.Result}{postfix}";
        }

        public Color Color => _skeletonAnimation.Skeleton.GetColor();

        public void SetAlpha(float alpha)
        {
            var color = _skeletonAnimation.Skeleton.GetColor();
            color.a = alpha;
            _skeletonAnimation.Skeleton.SetColor(color);
        }

        /// <returns>true if enemy has data, model loaded and is not dead</returns>
        public override bool IsValid()
        {
            return _spec.IsValid() && _enemyModel != null && !HasTag(TagsDef.Dead);
        }

        private void OnDestroy()
        {
            if (_modelHandle.IsValid()) Addressables.Release(_modelHandle);
            if (_localizedNameHandle.IsValid()) Addressables.Release(_localizedNameHandle);
            if (_spec.IsValid() == false) return; // party not always full
            Destroy(_enemyModel);
        }

        public void ProvideStats(AttributeWithValue[] attributeWithValues) { }

        public IEnumerable<LootInfo> GetLoots() => _spec.GetLoots();
    }
}