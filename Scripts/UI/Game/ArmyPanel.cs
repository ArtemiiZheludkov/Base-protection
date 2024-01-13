using UnityEngine;
using BaseProtection.Units;

namespace BaseProtection
{
    public class ArmyPanel : MonoBehaviour
    {
        [SerializeField] private GameUnitButton _unitButton;
        [SerializeField] private RectTransform _panel;
        
        private ArmySettings[] _army;
        private AlliesSpawner _spawner;
        
        public void Init(ArmySettings[] army)
        {
            _army = army;

            gameObject.SetActive(false);

            foreach (ArmySettings unit in _army)
            {
                string json = PlayerPrefs.GetString(unit.Unit.Id);
                UnitData data = new UnitData(JsonUtility.FromJson<UnitData>(json));
                
                if (data.Bought == true)
                    Instantiate(_unitButton, _panel).Init(unit.Unit.Type, unit.Unit.Icon, this);
            }
        }

        public void OpenPanel(AlliesSpawner spawner)
        {
            _spawner = spawner;
            gameObject.SetActive(true);
        }
        
        public void ClosePanel()
        {
            _spawner = null;
            gameObject.SetActive(false);
        }

        public void SelectedUnit(UnitType type)
        {
            foreach (ArmySettings unit in _army)
                if (unit.Unit.Type == type)
                    if (_spawner != null)
                        _spawner.InitSpawn(unit);

            ClosePanel();
        }
    }
}