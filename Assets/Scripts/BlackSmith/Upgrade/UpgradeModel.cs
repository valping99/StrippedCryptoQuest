using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UpgradeModel : MonoBehaviour, IUpgradeModel
    {
        private List<IUpgradeEquipment> _upgradeData = new();
        public List<IUpgradeEquipment> ListEquipment => _upgradeData;
        public void CoGetData(InventorySO inventory)
        {
            var listEquipment = inventory.Equipments;
            foreach (var equipment in listEquipment)
            {
                IUpgradeEquipment equipmentData = new MockUpgradeEquipment(equipment);
                _upgradeData.Add(equipmentData);
            }
        }
    }
}