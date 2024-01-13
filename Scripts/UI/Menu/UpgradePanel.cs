using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BaseProtection
{
    [Serializable]
    public class UpgradePanel
    {
        [HideInInspector] public PlayerAttribute Attribute;
        
        public TMP_Text PriceText;
        public TMP_Text StatText;
        public Button UpgradeButton;
        
        public void Init(PlayerAttribute attribute, UnityAction onClickAction)
        {
            Attribute = attribute;
            
            PriceText.text = Attribute.Price.ToString();
            StatText.text = Attribute.Value.ToString("F2");
            
            UpgradeButton.onClick.RemoveAllListeners();
            UpgradeButton.onClick.AddListener(onClickAction);
        }

        public void UpdatePanel()
        {
            PriceText.text = Attribute.Price.ToString();
            StatText.text = Attribute.Value.ToString("F2");
        }

        public void Upgraded()
        {
            Attribute.Price += (int)(Attribute.Price * Attribute.PriceIncrease);
            PriceText.text = Attribute.Price.ToString();
        }
    }
}