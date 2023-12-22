using System.Collections;
using System.Collections.Generic;
using CryptoQuest.ChangeClass.API;
using CryptoQuest.ChangeClass.View;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassPreviewPresenter : MonoBehaviour
    {
        [SerializeField] private ChangeClassSyncData _syncData;
        [SerializeField] private MerchantsInputManager _input;
        [SerializeField] private PreviewCharacterAPI _previewCharacterAPI;
        [SerializeField] private ChangeNewClassAPI _changeNewClassAPI;
        [SerializeField] private ChangeClassPresenter _changeClassPresenter;
        [SerializeField] private CalculatorCharacterStats _calculatorFirstCharacterStats;
        [SerializeField] private CalculatorCharacterStats _calculatorSecondCharacterStats;
        [SerializeField] private CalculatorCharacterStats _calculatorTooltipCharacter;
        [SerializeField] private UIPreviewCharacter _previewNewClass;
        [SerializeField] private UIPreviewCharacter _previewNewClassStatus;
        [SerializeField] private UIChangeClassTooltip _preview;
        [SerializeField] private InitializeNewCharacter _initializeCharacter;
        public UnityAction<UICharacter> FirstClassMaterialEvent;
        public UnityAction<UICharacter> LastClassMaterialEvent;
        private UICharacter _firstClassMaterial;
        private UICharacter _lastClassMaterial;
        public List<int> _materialsId { get; private set; } = new();

        private void OnEnable()
        {
            FirstClassMaterialEvent += GetFirstClassMaterial;
            LastClassMaterialEvent += GetLastClassMaterial;
        }

        private void OnDisable()
        {
            FirstClassMaterialEvent -= GetFirstClassMaterial;
            LastClassMaterialEvent -= GetLastClassMaterial;
        }

        private void GetFirstClassMaterial(UICharacter character)
        {
            _firstClassMaterial = character;
        }

        private void GetLastClassMaterial(UICharacter character)
        {
            _lastClassMaterial = character;
        }

        public void FilterClassMaterial(UICharacter character, UIClassMaterial classMaterial)
        {
            StartCoroutine(classMaterial.FilterClassMaterial(character));
            StartCoroutine(SelectDefaultButton(classMaterial));
        }

        private IEnumerator SelectDefaultButton(UIClassMaterial classMaterial)
        {
            yield return new WaitUntil(() => classMaterial.IsFilterClassMaterial);
            var materialNumber = classMaterial.ListClassCharacter.Count;
            if (materialNumber != 0)
                EnableButtonInteractable(true, classMaterial);
        }

        public void SetFirstClassMaterial(UIClassMaterial classMaterial, UICharacter character)
        {
            FirstClassMaterialEvent.Invoke(character);
            EnableButtonInteractable(false, classMaterial);
            HideDetail();
            character.EnableButtonBackground(true);
        }

        public void SetLastClassMaterial(UIClassMaterial classMaterial, UICharacter character)
        {
            EnableButtonInteractable(false, classMaterial);
            LastClassMaterialEvent?.Invoke(character);
            character.EnableButtonBackground(true);
        }

        public void EnableButtonInteractable(bool isEnable, UIClassMaterial classMaterial)
        {
            foreach (var button in classMaterial.ListClassCharacter)
            {
                button.GetComponent<Button>().interactable = isEnable;
            }

            var firstItemGO = classMaterial.ListClassCharacter[0].gameObject;
            EventSystem.current.SetSelectedGameObject(firstItemGO);
        }

        public void PreviewData()
        {
            StartCoroutine(PreviewNewClassData());
            GetClassMaterialId();
        }

        private void GetClassMaterialId()
        {
            _materialsId.Clear();
            _materialsId.Add(_firstClassMaterial.Class.Id);
            _materialsId.Add(_lastClassMaterial.Class.Id);
        }

        private IEnumerator PreviewNewClassData()
        {
            var avatar = _syncData.Avatar(_firstClassMaterial, _changeClassPresenter.Occupation);
            _previewCharacterAPI.LoadDataToPreviewCharacter(_firstClassMaterial, _lastClassMaterial);
            CheckElementImage();
            yield return new WaitUntil(() => _previewCharacterAPI.IsFinishFetchData);
            _previewNewClass.PreviewCharacter(_previewCharacterAPI.Data, _firstClassMaterial, avatar,
                CheckElementImage());
            _calculatorFirstCharacterStats.CalculatorStats(_firstClassMaterial);
            _calculatorSecondCharacterStats.CalculatorStats(_lastClassMaterial);
            GetDefaultExp(_previewNewClass);
        }

        private void GetDefaultExp(UIPreviewCharacter character)
        {
            var requiredExp = _calculatorFirstCharacterStats.GetRequiredExp(0);
            var currentExp = _calculatorFirstCharacterStats.GetCurrentExp(0);
            character.UpdateExpBar(currentExp, requiredExp);
        }

        public void ChangeClass()
        {
            StartCoroutine(ChangeNewClassAPI());
        }

        private IEnumerator ChangeNewClassAPI()
        {
            _input.DisableInput();
            _changeNewClassAPI.ChangeNewClassData(_firstClassMaterial, _lastClassMaterial,
                _changeClassPresenter.Occupation);
            yield return new WaitUntil(() => _changeNewClassAPI.IsFinishFetchData);
            _input.EnableInput();

            if (_changeNewClassAPI.Data == null) yield break;
            _syncData.SetNewClassData(_changeNewClassAPI.Data, _previewNewClassStatus);
            _initializeCharacter.GetStats(_changeNewClassAPI.Data);
            GetDefaultExp(_previewNewClassStatus);
        }

        public void ShowDetail(UICharacter character)
        {
            if (!_calculatorTooltipCharacter.gameObject.activeSelf)
            {
                _calculatorTooltipCharacter.gameObject.SetActive(true);
                _preview.ShowTooltip(character);
            }

            _calculatorTooltipCharacter.CalculatorStats(character);
        }

        public void HideDetail()
        {
            if (!_calculatorTooltipCharacter.gameObject.activeSelf) return;
            _calculatorTooltipCharacter.gameObject.SetActive(false);
        }

        private bool CheckElementImage()
        {
            return _firstClassMaterial.Class.Elemental == _lastClassMaterial.Class.Elemental;
        }
    }
}