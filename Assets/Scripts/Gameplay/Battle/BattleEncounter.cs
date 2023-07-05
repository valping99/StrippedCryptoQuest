using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattleEncounter : MonoBehaviour
    {
        [SerializeField] private TriggerBattleEncounterEventSO _triggerBattleEncounterEvent;
        [SerializeField] private BattleDataSO _battleData;
        [SerializeField] private BoxCollider2D _collider;

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(150, 0, 0, .3f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(new Vector3(_collider.offset.x, _collider.offset.y, -2), _collider.size);
        }
    }
}