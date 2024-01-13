using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseProtection;
using DG.Tweening;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Economy;
using Unity.Services.RemoteConfig;
using UnityEngine.UI;

namespace Dashboard
{
    public struct userAttributes 
    {public bool expansionFlag;}

    public struct appAttributes 
    {   public int level;
        public int score;
        public string appVersion;}
    
    public class LiveOpsManager : MonoBehaviour
    {
        public CurrencyElement Coins;
        public CurrencyElement Exp;
        public CurrencyElement Level;
        public CurrencyElement BestWave;

        [SerializeField] private PlayerAttributesManager _attributesManager;

        [Header("Load screen")] 
        [SerializeField] private Image _screen;
        [SerializeField] private Slider _slider;
        [SerializeField] private float _screenFade;
        
        public async Task Init()
        {
            try
            {
                await InitializeUnityServices();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async Task InitializeUnityServices()
        {
            _screen.gameObject.SetActive(true);

            _slider.value = 0;
            _slider.maxValue = 100;
            _slider.DOValue(10, 0.5f);
            
            await UnityServices.InitializeAsync();
            if (this == null) return;

            Debug.Log("Services Initialized.");

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                Debug.Log("Signing in...");
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Player id: {AuthenticationService.Instance.PlayerId}");
            }

            await EconomyService.Instance.Configuration.SyncConfigurationAsync();
            Debug.Log("EconomyService SyncConfigurationAsync completed");
            
            _slider.DOValue(50, 0.5f);
            
            RemoteConfigService.Instance.FetchCompleted += ApplyRemoteConfig;
            await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());

            await Coins.Init();
            await Exp.Init();
            await Level.Init();
            await BestWave.Init();
            
            _slider.DOValue(75, 0.5f);
            
            Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAllAsync();

            if (savedData.TryGetValue(_attributesManager.definitionId, out string json))
                _attributesManager.SetData(json);
            else
                _attributesManager.LoadRemoteConfig();
            
            _slider.DOValue(100, 0.25f);

            _screen.DOFade(0f, _screenFade).SetLink(_screen.gameObject)
                .OnComplete(() => _screen.gameObject.SetActive(false));
        }
        
        private void ApplyRemoteConfig(ConfigResponse configResponse)
        {
            switch (configResponse.requestOrigin) {
                case ConfigOrigin.Default:
                    Debug.Log ("No settings loaded this session and no local cache file exists; using default values.");
                    break;
                case ConfigOrigin.Cached:
                    Debug.Log ("No settings loaded this session; using cached values from a previous session.");
                    break;
                case ConfigOrigin.Remote:
                    Debug.Log ("New settings loaded this session; update values accordingly.");
                    break;
            }
        }
    }
}
