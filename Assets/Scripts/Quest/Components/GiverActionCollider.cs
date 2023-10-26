﻿using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class GiverActionCollider : MonoBehaviour
    {
        [field: SerializeField] public BoxCollider2D BoxCollider2D { get; set; }
        
        [SerializeField] private QuestEventChannelSO _giveQuestEventChannel;
        [SerializeField] private QuestEventChannelSO _removeQuestEventChannel;

        private QuestSO _questData;
        private ECollideActionType _collideActionType;

        public void SetQuest(QuestSO questData) => _questData = questData;
        public void SetBoxSize(Vector2 componentSizeBox) => BoxCollider2D.size = componentSizeBox;
        
        private void OnTriggerEnter2D(Collider2D other) => Execute(other, ECollideActionType.OnEnter);
        private void OnTriggerExit2D(Collider2D other) => Execute(other, ECollideActionType.OnExit);

        private void Execute(Collider2D other, ECollideActionType collideType)
        {
            if (!other.CompareTag("Player")) return;

            if (collideType != ECollideActionType.OnEnter)
            {
                _removeQuestEventChannel.RaiseEvent(_questData);
                return;
            }

            _giveQuestEventChannel.RaiseEvent(_questData);
        }
    }
}