﻿using System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] private QuestSO _quest;
        [SerializeField] private QuestEventChannelSO _giveQuestEventChannel;

        public void GiveQuest()
        {
            if (_quest == null) return;

            _giveQuestEventChannel.RaiseEvent(_quest);
        }
    }
}