﻿using System.Collections.Generic;
using CryptoQuest.Quest.Categories;
using CryptoQuest.System.CutsceneSystem;
using CryptoQuest.System.CutsceneSystem.Events;

namespace CryptoQuest.Quest.Controller
{
    public class QuestCutsceneController : BaseQuestController
    {
        private List<CutsceneQuestInfo> _currentlyCutsceneQuests = new();
        private QuestCutsceneDef _currentCutscene;

        public void GiveQuest(CutsceneQuestInfo questInfo)
        {
            _currentlyCutsceneQuests.Add(questInfo);
        }

        public void TriggerCutscene(CutsceneQuestInfo questInfo)
        {
            QuestManager.TriggerQuest(questInfo.Data);
            _currentCutscene = questInfo.Data.CutSceneToLoad;

            CutsceneManager.CutsceneCompleted += OnQuestFinish;
        }

        protected override void OnQuestFinish()
        {
            foreach (var processingQuest in _currentlyCutsceneQuests)
            {
                if (processingQuest.Data.CutSceneToLoad != _currentCutscene) continue;
                HandleCutsceneResult(processingQuest);
                break;
            }
        }

        private void HandleCutsceneResult(CutsceneQuestInfo processingQuest)
        {
            processingQuest.FinishQuest();
            _currentlyCutsceneQuests.Remove(processingQuest);
            CutsceneManager.CutsceneCompleted -= OnQuestFinish;
        }
    }
}