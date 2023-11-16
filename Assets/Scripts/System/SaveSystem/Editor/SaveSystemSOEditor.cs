using CryptoQuest.Networking;
using CryptoQuest.API;
using CryptoQuest.SaveSystem;
using CryptoQuest.System;
using CryptoQuest.System.SaveSystem.Savers;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.SaveSystem
{
    [CustomEditor(typeof(SaveSystemSO))]
    public class SaveSystemSOEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _uxml;
        private SaveSystemSO Target => target as SaveSystemSO;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _uxml.CloneTree(root);
            var saveButton = new Button(SaveToServer)
            {
                text = "Save"
            };
            root.Add(saveButton);


            var clearAllSaveButton = root.Q<Button>("clear-all-save-button");
            var openSaveFolderButton = root.Q<Button>("open-save-folder-button");

            clearAllSaveButton.clicked += OnClearAllSaveButtonOnclicked;
            openSaveFolderButton.clicked += OnOpenSaveFolderButtonOnclicked;


            return root;
        }

        private void SaveToServer()
        {
            Target.Save();
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient?.WithBody(new IntervalOnlineProgressionSaver.SaveDataBody() { GameData = Target.SaveData })
                .Post<IntervalOnlineProgressionSaver.SaveDataResult>(Accounts.USER_SAVE_DATA);
            EditorUtility.SetDirty(Target);
            AssetDatabase.SaveAssets();
        }

        private void OnOpenSaveFolderButtonOnclicked()
        {
            Application.OpenURL(Application.persistentDataPath);
        }

        private void OnClearAllSaveButtonOnclicked()
        {
            var so = new SerializedObject(Target);
            so.FindProperty("_saveData").boxedValue = new SaveData();
            so.ApplyModifiedProperties();
            Target.Save();
            EditorUtility.SetDirty(Target);
            AssetDatabase.SaveAssets();
        }
    }
}