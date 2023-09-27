using System.Collections;
using System.Collections.Generic;
using CryptoQuest.SocialLogin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Title
{
    public class UISignInPanel : MonoBehaviour
    {
        [field: SerializeField] public List<Selectable> Selectables { get; private set; }
        [field: SerializeField] public Button SignInButton { get; private set; }
        [field: SerializeField] public GameObject LoginFailedPanel { get; private set; }
        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        private int _currenSelectIndex = 0;

        private IEnumerator Start()
        {
            yield return true;
            Selectables[_currenSelectIndex].Select();
        }

        public void HandleDirection(float value)
        {
            _currenSelectIndex -= (int)value;
            _currenSelectIndex = _currenSelectIndex < 0 ? Selectables.Count - 1 : _currenSelectIndex;
            _currenSelectIndex = _currenSelectIndex >= Selectables.Count ? 0 : _currenSelectIndex;
            var selectable = Selectables[_currenSelectIndex];
            selectable.Select();
        }

        public AuthenticateFormInfo GetFormInfo()
        {
            return new AuthenticateFormInfo
            {
                Email = _emailInputField.text,
                Password = _passwordInputField.text
            };
        }
    }
}