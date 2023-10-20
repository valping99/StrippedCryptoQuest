using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.BlackSmith.Upgrade;
using CryptoQuest.Character.Hero.AvatarProvider;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item.Equipment;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class CharacterPartyPresenter : MonoBehaviour
    {
        [SerializeField] private List<UIUpgradeCharacter> _listCharacter;
        private IPartyController _partyController;
        private IHeroAvatarProvider _heroAvatarProvider;

        private void OnEnable()
        {
            LoadPlayerPartyUI();
        }

        private void LoadPlayerPartyUI()
        {
            _heroAvatarProvider = GetComponent<IHeroAvatarProvider>();
            _partyController = ServiceProvider.GetService<IPartyController>();
            DisableCharacterObjects();
            LoadCharacterDetail();
        }

        private void DisableCharacterObjects()
        {
            for (int i = 0; i < _partyController.Slots.Length; i++)
            {
                _listCharacter[i].gameObject.SetActive(false);
            }
        }

        private void LoadCharacterDetail()
        {
            for (int i = 0; i < _partyController.Slots.Length; i++)
            {
                _listCharacter[i].gameObject.SetActive(true);
                _listCharacter[i].LoadCharacterDetail(_partyController.Slots[i].HeroBehaviour);

                StartCoroutine(CoLoadAvatar(_partyController.Slots[i].HeroBehaviour, _listCharacter[i]));
            }
        }

        private IEnumerator CoLoadAvatar(HeroBehaviour hero, UIUpgradeCharacter characterUI)
        {
            yield return _heroAvatarProvider.LoadAvatarAsync(hero);
            characterUI.SetAvatar(hero.Avatar);
        }

        private void PreviewCharacterStats(UIEquipmentItem item)
        {
            if (item == null) return;
            for (int i = 0; i < _partyController.Slots.Length; i++)
            {
                _listCharacter[i].Preview((EquipmentInfo)item.EquipmentData);
            }
        }

    }
}