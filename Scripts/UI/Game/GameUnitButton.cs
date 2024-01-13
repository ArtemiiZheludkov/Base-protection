using UnityEngine;
using UnityEngine.UI;
using BaseProtection.Units;

namespace BaseProtection
{
    public class GameUnitButton : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Button _button;

        private UnitType _type;
        private ArmyPanel _armyPanel;

        public void Init(UnitType type, Sprite sprite, ArmyPanel armyPanel)
        {
            _type = type;
            _icon.sprite = sprite;
            _armyPanel = armyPanel;
            
            _button.onClick.AddListener(OnClicked);
        }

        private void OnClicked()
        {
            _armyPanel.SelectedUnit(_type);
        }
    }
}