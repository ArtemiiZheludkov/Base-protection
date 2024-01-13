using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BaseProtection.Units
{
    public class Health : MonoBehaviour
    {
        public int Value { get; private set; }
        
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshPro[] _particleTexts;
        [SerializeField] private Animator[] _animatorTexts;
        
        private int _maxHeath;
        private int _textCounter;

        private string animKey = "TextPop";

        public void Init(int value)
        {
            _maxHeath = value;
            Value = _maxHeath;
            
            _slider.minValue = 0;
            _slider.maxValue = Value;
            _slider.value = Value;
            
            _textCounter = 0;
        }
        
        public void DecreaseHealth(int damage)
        {
            Value -= damage;

            if (Value < 1)
                Value = 0;

            _particleTexts[_textCounter].text = "-" + damage;
            UpdateUI();
        }

        private void UpdateUI()
        {
            _slider.value = Value;
            _animatorTexts[_textCounter].Play(animKey, 0, 0f);
            _textCounter += 1;

            if (_textCounter >= _particleTexts.Length)
                _textCounter = 0;
        }
    }
}