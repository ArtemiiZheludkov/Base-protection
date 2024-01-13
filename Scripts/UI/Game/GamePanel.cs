using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace BaseProtection
{
    public class GamePanel : UIPanel
    {
        [SerializeField] private TMP_Text _coins;
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
            _coins.text = _game.Bank.LevelBalance.ToString();
            
            _coins.rectTransform
                .DOScale(Vector3.one * 1.1f, 0.1f)
                .OnComplete(() => _coins.rectTransform.DOScale(Vector3.one, 0.15f))
                .SetLink(_coins.gameObject);
        }

        private void OnMenuClicked() =>  _game.ReturnToStart();
    }
}