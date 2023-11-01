﻿using CryptoQuest.Tavern.UI;
using UnityEngine;

namespace CryptoQuest.Tavern.States
{
    public class SelectCharacterState : StateMachineBehaviour
    {
        private Animator _animator;
        private TavernController _controller;

        private static readonly int OverviewState = Animator.StringToHash("Overview");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _animator = animator;

            _controller = animator.GetComponent<TavernController>();
            _controller.UICharacterReplacement.gameObject.SetActive(true);
            _controller.UICharacterReplacement.StateEntered();

            _controller.TavernInputManager.CancelEvent += CancelCharacterReplacement;

        }

        private void CancelCharacterReplacement()
        {
            _controller.UICharacterReplacement.gameObject.SetActive(false);
            _animator.Play(OverviewState);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _controller.UICharacterReplacement.StateExited();
            _controller.TavernInputManager.CancelEvent -= CancelCharacterReplacement;
        }
    }
}