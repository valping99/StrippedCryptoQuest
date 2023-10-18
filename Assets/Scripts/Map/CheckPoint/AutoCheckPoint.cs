using CryptoQuest.System;
using IndiGames.Core.Events.ScriptableObjects;
using System.Linq;
using UnityEngine;

namespace CryptoQuest.Map.CheckPoint
{
    public class AutoCheckPoint : MonoBehaviour
    {
        [SerializeField]
        private VoidEventChannelSO _sceneLoadedEvent;
        [SerializeField] private int _lookDirection;

        private void Awake()
        {
            _sceneLoadedEvent.EventRaised += SaveCheckPoint;

            var colliders = GetComponents<Collider2D>();
            foreach (Collider2D collider in colliders.ToArray())
            {
                Destroy(collider);
            }    
        }

        private void OnDestroy()
        {
            _sceneLoadedEvent.EventRaised -= SaveCheckPoint;
        }

        private void SaveCheckPoint()
        {
            var checkPointProvider = ServiceProvider.GetService<ICheckPointProvider>();
            checkPointProvider.SaveCheckPoint(transform.position);
        }

        /// <summary>
        /// This will be called when SuperTiled2Unity has finished importing the component.
        /// </summary>
        /// <param name="Direction"></param>
        public void Direction(int direction)
        {
            _lookDirection = direction;
        }
    }
}
