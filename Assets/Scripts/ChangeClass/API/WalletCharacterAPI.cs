using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.Networking;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;
using APIChangeClass = CryptoQuest.API.ChangeClass;

namespace CryptoQuest.ChangeClass.API
{
    public class WalletCharacterAPI : MonoBehaviour
    {
        private IRestClient _restAPINetworkController;

        public List<CharacterAPI> Data { get; private set; } = new();
        public bool IsFinishFetchData { get; private set; }

        public void LoadCharacterFromWallet()
        {
            Data.Clear();
            ActionDispatcher.Dispatch(new ShowLoading());
            IsFinishFetchData = false;
            _restAPINetworkController = ServiceProvider.GetService<IRestClient>();
            _restAPINetworkController
                .Get<CharacterResponseData>(APIChangeClass.LOAD_ALL_CHARACTER)
                .Subscribe(Authenticated, DispatchLoadFailed, DispatchLoadFinished);
        }

        private void Authenticated(CharacterResponseData response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            IsFinishFetchData = true;
            Data = response.data.characters;
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void DispatchLoadFailed(Exception obj)
        {
            IsFinishFetchData = true;
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void DispatchLoadFinished()
        {
            Debug.Log($"ChangeClass:: Load Character Success");
        }
    }
}
