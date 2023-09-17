﻿using System;
using UnityEngine;

namespace CryptoQuest.Quest
{
    [Serializable]
    [CreateAssetMenu(fileName = "Quest", menuName = "Quest System/Quest")]
    public class Quest : ScriptableObject
    {
        public event IQuestDefinition.StatusChangedEvent StatusChanged;

        [SerializeField] private string _id;

        /// <summary>
        /// Only check objectives if this is false..
        /// </summary>
        public bool Completed;

        [Header("Details")]
        [SerializeField] private int _currentTaskIndex;

        [SerializeField, NonReorderable] private TaskContainer[] _tasks = Array.Empty<TaskContainer>();

        private void OnEnable()
        {
            StatusChanged += OnStatusChanged;
        }

        private void OnDisable()
        {
            StatusChanged -= OnStatusChanged;
        }

        private void OnStatusChanged(bool completed)
        {
            if (StatusChanged == null)
            {
                Debug.LogWarning("Quest status changed, but no listeners were found.");
                return;
            }

            StatusChanged(completed);
        }

        public bool HasTaskCompleted(Task task)
        {
            for (var index = 0; index < _tasks.Length; index++)
            {
                var configTask = _tasks[index];
                if (configTask.Task.CompareTo(task) != 0) continue;
                return configTask.Completed;
            }

            return false;
        }

        public bool CanCompleteTask(Task task)
        {
            for (var index = 0; index < _tasks.Length; index++)
            {
                var configTask = _tasks[index];
                if (configTask.Task.CompareTo(task) != 0) continue;

                if (index <= 0) continue;

                var previousTask = _tasks[index - 1];
                if (!previousTask.Completed)
                {
                    return false;
                }
            }

            return true;
        }

        public void CompleteTask(Task task)
        {
            for (var index = 0; index < _tasks.Length; index++)
            {
                var configTask = _tasks[index];
                if (configTask.Task.CompareTo(task) != 0) continue;
                configTask.Completed = true;

                if (index == _tasks.Length - 1)
                {
                    Completed = true;
                }

                break;
            }
        }
    }
}