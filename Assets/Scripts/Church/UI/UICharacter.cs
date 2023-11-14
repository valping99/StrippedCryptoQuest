using System;
using System.Collections;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Hero.AvatarProvider;
using CryptoQuest.UI.Character;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Church.UI
{
    public class UICharacter : MonoBehaviour
    {
        public event Action<UICharacter> OnSubmit;
        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private Image _avatar;
        [SerializeField] private UIAttributeBar _hpBar;
        [SerializeField] private UIAttributeBar _mpBar;
        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;
        public HeroBehaviour HeroBehaviour { get; private set; }
        public Button CharacterButton { get; private set; }

        public void ConfigureCharacter(HeroBehaviour hero, IHeroAvatarProvider avatar)
        {
            HeroBehaviour = hero;
            _displayName.StringReference = hero.LocalizedName;
            _avatar.sprite = hero.Avatar;
            _attributeChangeEvent.AttributeSystemReference = hero.GetComponent<AttributeSystemBehaviour>();
            StartCoroutine(CoLoadAvatar(avatar));
        }

        private IEnumerator CoLoadAvatar(IHeroAvatarProvider avatar)
        {
            yield return avatar.LoadAvatarAsync(HeroBehaviour);
            _avatar.sprite = HeroBehaviour.Avatar;
        }

        public void SetCurrentHp(float currentHp)
        {
            _hpBar.SetValue(currentHp);
        }

        public void SetMaxHp(float maxHp)
        {
            _hpBar.SetMaxValue(maxHp);
        }

        public void SetCurrentMp(float currentMp)
        {
            _mpBar.SetValue(currentMp);
        }

        public void SetMaxMp(float maxMp)
        {
            _mpBar.SetMaxValue(maxMp);
        }
    }
}
