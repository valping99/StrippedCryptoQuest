﻿using System;
using System.Collections;
using CryptoQuest.Character;
using CryptoQuest.EditorTool;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace CryptoQuest.Quest.Actor.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Actor/NormalActorSO", fileName = "NormalActorSO")]
    public class NormalActorSO : ActorSO<NormalActorInfo>
    {
        [field: SerializeField] public string Name { get; private set; }

        [field: Header("Config Settings")]
        [field: SerializeField] public QuestSO Quest { get; private set; }

        public override ActorInfo CreateActor() => new NormalActorInfo(this);
    }

    [Serializable]
    public class NormalActorInfo : ActorInfo<NormalActorSO>
    {
        private AsyncOperationHandle<GameObject> _handle;
        public NormalActorInfo(NormalActorSO npcActorSO) : base(npcActorSO) { }
        public NormalActorInfo() { }

        public override IEnumerator Spawn(Transform parent)
        {
            _handle = Data.Prefab.InstantiateAsync(parent.position, parent.rotation, parent);

            yield return _handle;

            if (Data.Quest == null) yield break;

            NPCBehaviour npcBehaviour = parent.GetComponentInChildren<NPCBehaviour>();
            GiverActionCollider actionCollider = parent.GetComponentInChildren<GiverActionCollider>();

            actionCollider.SetQuest(Data.Quest);

            if (!parent.TryGetComponent<ShowCubeWireUtil>(out var showCubeWireUtil)) yield break;
            actionCollider.SetBoxSize(showCubeWireUtil.SizeBox);

            if (!npcBehaviour.TryGetComponent<QuestTrigger>(out var questGiver)) yield break;

            questGiver.Init(Data.Quest, actionCollider);
        }

        public override IEnumerator DeSpawn(GameObject parent)
        {
            if (!_handle.IsValid())
            {
                Object.Destroy(parent);
                yield break;
            }

            yield return _handle;

            if (!parent.TryGetComponent<ActorSpawner>(out var actorSpawner)) yield break;

            Object.Destroy(actorSpawner.gameObject);
        }
    }
}