using System;
using System.Collections.Generic;
using UnityEngine;

namespace BaseProtection
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private StartPanel _start;
        [SerializeField] private GamePanel _game;
        [SerializeField] private WinPanel _win;
        [SerializeField] private LosePanel _lose;
        [SerializeField] public Joystick _joystick;

        private Dictionary<Type, UIPanel> _panels;
        private UIPanel _currentPanel;

        public void Init()
        {
            _panels = new Dictionary<Type, UIPanel>();
            _panels.Add(typeof(StartPanel), _start);
            _panels.Add(typeof(GamePanel), _game);
            _panels.Add(typeof(WinPanel), _win);
            _panels.Add(typeof(LosePanel), _lose);
        }

        public void ActivatePanel<T>() where T : UIPanel
        {
            DeactivatePanels();
            _currentPanel = _panels[typeof(T)];
            _currentPanel.Activate();
        }

        public void UpdatePanel<T>() where T : UIPanel
        {
            _panels[typeof(T)].UpdatePanel();
        }
        
        public void UpdateCurrentPanel()
        {
            if (_currentPanel == null)
                return;
            
            _currentPanel.UpdatePanel();
        }

        private void DeactivatePanels()
        {
            foreach(UIPanel panel in gameObject.GetComponentsInChildren<UIPanel>()) 
            {
                panel.gameObject.SetActive(false);
            }
        }
    }
}