using Dashboard;
using UnityEngine;

namespace BaseProtection
{
    public class PlayerAttributesManager : CloudElement
    {
        public PlayerConfig PlayerConfig;
        private Game _game;
        
        public void Init()
        {
            _game = Game.Instance;
            
            string json = PlayerPrefs.GetString(definitionId);
            
            if (!string.IsNullOrEmpty(json))
            {
                information = json;
                JsonUtility.FromJsonOverwrite(json, PlayerConfig);
            }
            else
            {
                information = JsonUtility.ToJson(PlayerConfig, true);
                PlayerPrefs.SetString(definitionId, information);
            }
        }

        public bool TryUpAttribute(PlayerAttribute attribute, float min)
        {
            if (_game.Bank.TryGetGameExp(attribute.Price) == false)
                return false;

            float up = attribute.Value * attribute.UpgradeCoefficient;
            
            if (up < min)
                up = min;
            
            attribute.Value += up;
            
            information = JsonUtility.ToJson(PlayerConfig, true);
            SaveData();
            
            return true;
        }

        public override void SetData(string json)
        {
            if (string.IsNullOrEmpty(json) == true)
            {
                LoadRemoteConfig();
                return;
            }
            
            information = json;
            JsonUtility.FromJsonOverwrite(json, PlayerConfig);
            
            SaveData();
        }
    }
}