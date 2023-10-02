﻿using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.SelectCommand;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public class SelectCommand : StateBase, ISelectCommandCallback
    {
        public SelectCommand(HeroBehaviour hero, SelectHeroesActions fsm) : base(hero, fsm) { }

        private GameObject _lastSelectedCommand;

        public override void OnEnter()
        {
            Fsm.SelectCommandUI.SetCharacterName(Hero.Spec.Unit.Origin.DetailInformation.LocalizedName);
            Fsm.SelectCommandUI.RegisterCallback(this);
            if (_lastSelectedCommand != null)
                EventSystem.current.SetSelectedGameObject(_lastSelectedCommand);
            else
                Fsm.SelectCommandUI.SelectFirstButton();
            EnableCommandMenu();

            Fsm.BattleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed += CancelPressed;
            Debug.Log($"Select command for {Hero.Spec.Unit.Origin.name}");
        }

        public override void OnExit()
        {
            Fsm.BattleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed -= CancelPressed;
            Fsm.SelectCommandUI.RegisterCallback(null);
            DisableCommandMenu();
            Debug.Log($"Exit Select command for {Hero.Spec.Unit.Origin.name}");
        }

        private void CancelPressed(InputAction.CallbackContext obj)
        {
            Fsm.PopState();
        }

        public void OnAttackPressed()
        {
            Debug.Log("SelectCommandState::OnAttackPressed");
            _lastSelectedCommand = EventSystem.current.currentSelectedGameObject;
            DisableCommandMenu();
            Fsm.PushState(new AttackEnemy(Hero, Fsm));
        }

        public void OnSkillPressed()
        {
            Debug.Log("SelectCommandState::OnSkillPressed");
            _lastSelectedCommand = EventSystem.current.currentSelectedGameObject;
            DisableCommandMenu();
            Fsm.PushState(new SelectingSkill(Hero, Fsm));
        }

        public void OnItemPressed()
        {
            Debug.Log("SelectCommandState::OnItemPressed");
            _lastSelectedCommand = EventSystem.current.currentSelectedGameObject;
            NextHero();
        }

        public void OnGuardPressed()
        {
            Debug.Log("SelectCommandState::OnGuardPressed");
            Hero.SetCommand(new GuardCommand(Hero));
            NextHero();
        }

        public void OnRetreatPressed()
        {
            Debug.Log("SelectCommandState::OnRetreatPressed");
            NextHero();
        }

        private void NextHero()
        {
            if (Fsm.GetNextAliveHero(out var nextHero))
                Fsm.PushState(new SelectCommand(nextHero, Fsm));
            else
                Fsm.GoToPresentState();
        }


        private void EnableCommandMenu()
        {
            // _enemyPartyManager.EnemiesPresenter.Hide();
            Fsm.SelectCommandUI.SetActiveCommandsMenu(true);
        }

        private void DisableCommandMenu()
        {
            Fsm.SelectCommandUI.SetActiveCommandsMenu(false);
        }
    }
}