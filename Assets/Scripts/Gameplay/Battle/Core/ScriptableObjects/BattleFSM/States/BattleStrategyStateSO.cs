using System.Collections;
using CryptoQuest.FSM;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.BattleFSM.States
{
    [CreateAssetMenu(fileName = "BattleStrategyStateSO", menuName = "Gameplay/Battle/FSM/States/Battle Strategy State")]
    public class BattleStrategyStateSO : BattleStateSO
    {
        [SerializeField] private VoidEventChannelSO _endStategyPhaseEventChannel;
        private Coroutine _unitPrepareCoroutine; 
        public override void OnEnterState(BaseStateMachine stateMachine)
        {
            base.OnEnterState(stateMachine);
            _unitPrepareCoroutine = stateMachine.StartCoroutine(BattleUnitsPrepare(stateMachine));
        }

        private IEnumerator BattleUnitsPrepare(BaseStateMachine stateMachine)
        {
            // Might sort _battleUnits by speed or team 1 will execute first by default
            BattleManager.OnNewTurn();
            foreach (var unit in BattleManager.BattleUnits)
            {
                BattleManager.CurrentUnit = unit;
                yield return unit.Prepare();
            }
            BattleManager.CurrentUnit = null;
            stateMachine.SetCurrentState(_nextState);
            _endStategyPhaseEventChannel.RaiseEvent();
        }
        
        public override void OnExitState(BaseStateMachine stateMachine)
        {
            base.OnExitState(stateMachine);
            stateMachine.StopCoroutine(_unitPrepareCoroutine);
        }
    }
}