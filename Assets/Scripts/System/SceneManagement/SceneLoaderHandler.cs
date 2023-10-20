using CryptoQuest.System.SaveSystem;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.System.SceneManagement
{
    public class SceneLoaderHandler : LinearGameSceneLoader
    {
        [SerializeField] private SceneLoaderBus _sceneLoadBus;
        [SerializeField] private SaveSystemSO _saveSystemSO;
        [SerializeField] protected VoidEventChannelSO _sceneLoadedEvent;

        private void Awake()
        {
            ServiceProvider.Provide<ISaveSystem>(_saveSystemSO);
            _sceneLoadBus.SceneLoader = this;
            _sceneLoadedEvent.EventRaised += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            _sceneLoadedEvent.EventRaised -= OnSceneLoaded;
        }

        private void OnSceneLoaded()
        {
            if (SceneToLoad.SceneType == SceneScriptableObject.Type.Location)
            {
                Debug.Log("ISceneLoaderHandler, OnSceneLoaded save data...");
                _saveSystemSO.SaveScene(SceneToLoad);
            }
        }
    }
}