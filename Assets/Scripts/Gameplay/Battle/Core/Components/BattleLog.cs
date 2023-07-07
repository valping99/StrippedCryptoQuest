using System.Collections;
using System.Collections.Generic;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattleLog : MonoBehaviour
    {
        [SerializeField] private StringEventChannelSO _gotNewLogEventChannel;

        /// <summary>
        /// Override this if you want to save all battle logs
        /// </summary>
        /// <param name="message"></param>
        public virtual void Log(string message)
        {
            _gotNewLogEventChannel.RaiseEvent(message);
        }
    }
}
