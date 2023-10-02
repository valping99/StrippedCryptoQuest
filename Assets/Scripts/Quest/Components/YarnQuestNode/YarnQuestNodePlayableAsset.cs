﻿using CryptoQuest.Quest.Authoring;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryptoQuest.Quest.Components.YarnQuestNode
{
    public class YarnQuestNodePlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField] private YarnDialogWithQuestSo _yarnDialogWithQuest;
        [SerializeField] private YarnQuestNodePlayableBehaviour _template;
        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<YarnQuestNodePlayableBehaviour>.Create(graph, _template);

            _template.SetYarnQuestSo(_yarnDialogWithQuest);
            return playable;
        }
    }
}