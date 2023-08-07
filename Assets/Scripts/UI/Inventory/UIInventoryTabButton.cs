using CryptoQuest.Data.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Inventory
{
    public class UIInventoryTabButton : MonoBehaviour
    {
        public event UnityAction<UsableTypeSO> Clicked;

        [SerializeField] private UsableTypeSO _typeMenuItem;

        public void OnClicked()
        {
            Clicked?.Invoke(_typeMenuItem);
        }

        public void Select()
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}