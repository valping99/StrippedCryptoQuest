using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.BlackSmith.Commons.UI;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.UpgradeStone.UI
{
    public class UpgradableStonesPresenter : MonoBehaviour
    {
        public event Action<UIUpgradableStone> StoneSelected;
        public event Action<IMagicStone> StoneInspected;
        public event Action StoneDeselected;


        [SerializeField] private WalletSO _walletSO;
        [SerializeField] private CurrencySO _gold;
        [SerializeField] private CurrencySO _metad;
        [SerializeField] private UpgradableStoneDataMapping _stoneMappings;
        
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private UIUpgradableStone _stonePrefab;
        [SerializeField] private int _maxLevel = 9;
        public Transform Content => _scrollRect.content;
        protected IObjectPool<UIUpgradableStone> _itemPool;
        protected const float ERROR_PRONE_DELAY = 0.05f;
        private List<UIUpgradableStone> _cachedItems = new();

        private void Awake()
        {
            _itemPool ??= new ObjectPool<UIUpgradableStone>(OnCreate, OnGet,
                OnReturnToPool, OnDestroyPool);
        }

        public void RenderStones(List<IMagicStone> items)
        {
            foreach (var stone in items)
            {
                if (stone.Level >= _maxLevel) continue;
                var stoneUI = _itemPool.Get();
                stoneUI.Initialize(stone);
                stoneUI.MaterialTag.SetActive(false);
                stoneUI.Button.interactable = true;
                SetupStonePrice(stoneUI);
            }

            Invoke(nameof(SelectFirstButton), ERROR_PRONE_DELAY);
        }

        public void SetupStonePrice(UIUpgradableStone itemUI)
        {
            var currentGold = _walletSO[_gold].Amount;
            var currentMetad = _walletSO[_metad].Amount;

            var stoneMapping = _stoneMappings.Datas.FirstOrDefault(x => x.ID == itemUI.MagicStone.Level);

            var goldInfo = new CurrencyValueEnough()
            {
                Value = stoneMapping.Gold,
                IsEnough = currentGold >= stoneMapping.Gold
            };
            var metadInfo = new CurrencyValueEnough()
            {
                Value = stoneMapping.Metad,
                IsEnough = currentMetad >= stoneMapping.Metad
            };

            itemUI.InitPrice(goldInfo, metadInfo, stoneMapping);
        }

        protected void SelectFirstButton()
        {
            foreach (var item in _cachedItems)
            {
                if (item.Button.interactable)
                {
                    item.Button.Select();
                    StoneInspected?.Invoke(item.MagicStone);
                    return;
                }
            }
        }

        public void ClearStones(UIUpgradableStone exceptionUI = null)
        {
            foreach (var item in _cachedItems.ToList())
            {
                if (exceptionUI != null && item.MagicStone.Definition == exceptionUI.MagicStone.Definition &&
                    item.MagicStone.Level == exceptionUI.MagicStone.Level) continue;
                _itemPool.Release(item);
            }
        }

        private void OnInspectItem(UIUpgradableStone ui)
        {
            StoneInspected?.Invoke(ui.MagicStone);
        }

        private void OnDeselectItem()
        {
            StoneDeselected?.Invoke();
        }

        #region Pool-handler

        private UIUpgradableStone OnCreate()
        {
            var button = Instantiate(_stonePrefab, _scrollRect.content);
            return button;
        }

        private void OnGet(UIUpgradableStone item)
        {
            item.transform.SetAsLastSibling();
            item.gameObject.SetActive(true);
            item.Inspected += OnInspectItem;
            item.DeSelected += OnDeselectItem;
            item.Selected += OnSelectItem;
            _cachedItems.Add(item);
        }

        protected virtual void OnSelectItem(UIUpgradableStone item)
        {
            StoneSelected?.Invoke(item);
        }

        private void OnReturnToPool(UIUpgradableStone item)
        {
            item.gameObject.SetActive(false);
            item.Inspected -= OnInspectItem;
            item.DeSelected -= OnDeselectItem;
            item.Selected -= OnSelectItem;
            _cachedItems.Remove(item);
        }

        private void OnDestroyPool(UIUpgradableStone item)
        {
            item.Inspected -= OnInspectItem;
            item.DeSelected -= OnDeselectItem;
            item.Selected -= OnSelectItem;
            Destroy(item.gameObject);
        }

        #endregion
    }
}