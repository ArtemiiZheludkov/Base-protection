using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BaseProtection
{
    public class LosePanel : UIPanel
    {
        [SerializeField] private TMP_Text _addCoins;
        [SerializeField] private TMP_Text _addExp; 
        [SerializeField] private TMP_Text _wave;
        [SerializeField] private Button _menu;
        
        private Game _game;

        private void OnDisable()
        {
            _menu.onClick.RemoveAllListeners();
        }
        
        public override void Activate()
        {
            if (_game == null)
                _game = Game.Instance;
            
            gameObject.SetActive(true);
            UpdatePanel();
            _menu.onClick.AddListener(OnMenuClicked);
        }

        public override void UpdatePanel()
        {
            _addCoins.text = "+ " + _game.Bank.EarnCoins;
            _addExp.text = "+ " + _game.Bank.EarnEXP;
            _wave.text = _game.ProgressManager.CurrentWave.ToString();
        }

        private void OnMenuClicked() => _game.ReturnToStart();
    }
}