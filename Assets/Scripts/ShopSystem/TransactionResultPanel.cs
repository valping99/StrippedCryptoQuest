﻿using CryptoQuest.UI.Dialogs.BattleDialog;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.ShopSystem
{
    public class TransactionResultPanel : MonoBehaviour
    {
        [SerializeField] private LocalizedString _strSuccess;
        [SerializeField] private LocalizedString _strFailed;
        [SerializeField] private UnityEvent _showing;
        [SerializeField] private UnityEvent _hiding;

        public TransactionResultPanel AddHideCallback(UnityAction callback)
        {
            _hiding.AddListener(callback);
            return this;
        }


        public void ShowSuccess()
        {
            GenericDialogController.Instance.InstantiateAsync(dialog =>
            {
                dialog
                    .WithHideCallback(() =>
                    {
                        DOVirtual.DelayedCall(0, () =>
                        {
                            _hiding?.Invoke();
                            _hiding?.RemoveAllListeners();
                        });
                    })
                    .RequireInput()
                    .WithMessage(_strSuccess)
                    .Show();
                _showing?.Invoke();
            });
        }

        public void ShowFailed()
        {
            GenericDialogController.Instance.InstantiateAsync(dialog =>
            {
                dialog
                    .WithHideCallback(() =>
                    {
                        _hiding?.Invoke();
                        _hiding?.RemoveAllListeners();
                    })
                    .RequireInput()
                    .WithMessage(_strFailed)
                    .Show();
                _showing?.Invoke();
            });
        }
    }
}