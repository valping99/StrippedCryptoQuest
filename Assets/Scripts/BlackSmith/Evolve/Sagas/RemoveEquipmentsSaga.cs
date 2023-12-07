using CryptoQuest.Battle.Components;
using CryptoQuest.BlackSmith.Evolve.UI;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using System;
using UniRx;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.Sagas
{
    public class RemoveEquipmentsSaga : SagaBase<RemoveEquipments>
    {
        private IInventoryController _inventoryController;
        private IPartyController _partyController;

        protected override void HandleAction(RemoveEquipments ctx)
        {
            foreach (var equipment in ctx.Equipments)
            {
                if (equipment == null || !equipment.IsValid()) return;

                // Since equipment can only equipping or in inventory so
                // if update in inventory success we dont need to update equipping
                if (TryUpdateInventory(equipment)) continue;
                UpdateEquipping(equipment);
            }
        }

        private bool TryUpdateInventory(IEquipment equipment)
        {
            _inventoryController ??= ServiceProvider.GetService<IInventoryController>();
            return equipment.RemoveFromInventory(_inventoryController);
        }

        private void UpdateEquipping(IEquipment equipment)
        {
            _partyController ??= ServiceProvider.GetService<IPartyController>();
            
            foreach (var slot in _partyController.Slots)
            {
                if (!slot.IsValid()) continue;

                var hero = slot.HeroBehaviour;
                foreach (var equipSlot in hero.GetEquipments().Slots)
                {
                    var equipping = equipSlot.Equipment;
                    if (equipping == null || !equipping.IsValid() || !equipping.Equals(equipment))
                        continue;
                    
                    var equipmentController = hero.GetComponent<EquipmentsController>();
                    equipmentController.Unequip(equipSlot.Type);
                    equipping.RemoveFromInventory(_inventoryController);
                    return;
                }
            }
        }
    }
}
