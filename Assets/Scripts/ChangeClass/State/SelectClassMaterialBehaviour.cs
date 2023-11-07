using System.Collections.Generic;
using CryptoQuest.ChangeClass.View;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;


namespace CryptoQuest.ChangeClass.StateMachine
{
    public class SelectClassMaterialBehaviour : StateMachineBehaviour
    {
        [SerializeField] private LocalizedString _message;
        private ChangeClassStateController _stateController;
        private ChangeClassInputManager _input;
        private Animator _animator;
        private List<Button> _listButton;
        private static readonly int _submit = Animator.StringToHash("isConfirm");
        private static readonly int _exit = Animator.StringToHash("isChangeClass");
        public int index;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            index = 0;
            _animator = animator;
            _stateController = _animator.GetComponent<ChangeClassStateController>();
            _input = _stateController.Input;
            _input.SubmitEvent += SelectedClassMaterial;
            _input.CancelEvent += ExitState;
            _stateController.Presenter.EnableClassInteractable(false);
            _stateController.ConfirmMaterial.EnableButtonInteractable(true, index);
            _stateController.DialogController.Dialogue
                .SetMessage(_message).Show();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _input.SubmitEvent -= SelectedClassMaterial;
            _input.CancelEvent -= ExitState;
        }

        private void SelectedClassMaterial()
        {
            var currentCharacter = EventSystem.current.currentSelectedGameObject.GetComponent<UICharacter>();
            _stateController.ConfirmMaterial.EnableButtonInteractable(false, index);
            index++;
            
            if (index < _stateController.Presenter.ListClassMaterial.Count)
            {
                _stateController.ConfirmMaterial.FirstClassMaterialEvent?.Invoke(currentCharacter);
                _stateController.ConfirmMaterial.FilterClassMaterial(currentCharacter, index);
                currentCharacter.EnableButtonBackground(true);
            }
            else
            {
                _stateController.ConfirmMaterial.LastClassMaterialEvent?.Invoke(currentCharacter);
                currentCharacter.EnableButtonBackground(true);
                ChangeState();
            }
        }

        private void ChangeState()
        {
            _animator.SetTrigger(_submit);
        }

        private void ExitState()
        {
            _animator.SetTrigger(_exit);
        }
    }
}