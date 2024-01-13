using System;
using System.Collections.Generic;
using Unity.Services.CloudSave;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Dashboard
{
    public abstract class CloudElement : MonoBehaviour
    {
        public string definitionId;
        [HideInInspector] public string information;

        public void SetDefaultData()
        {
            information = PlayerPrefs.GetString(definitionId);
        }

        public abstract void SetData(string json);

        public void LoadRemoteConfig()
        { 
            string json = RemoteConfigService.Instance.appConfig.GetJson(definitionId);
            SetData(json);
        }

        public async void SaveData()
        {
            PlayerPrefs.SetString(definitionId, information);

            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>(){{definitionId, information}};
                await CloudSaveService.Instance.Data.ForceSaveAsync(data);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}