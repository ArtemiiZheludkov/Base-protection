using System;
using TMPro;
using UnityEngine;

namespace BaseProtection
{
    [RequireComponent(typeof(BoxCollider))]
    public class BuyZone : MonoBehaviour
    {
        [Header("BUY SETTINGS")]
        [SerializeField] private float _buyTime;
        [SerializeField] private float _particleCooldown;
        
        [Header("VISUAL SETTINGS")]
        [SerializeField] private TMP_Text _priceTxt;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private Transform _lookParticle;

        private Action _buy;
        private BankService Bank;
        
        private int _price;
        private int _currentPrice, _payment;
        private float _waitForPay;
        private float _nextPayTime;
        private float _particleTime;

        public void Init(int price, Action onBuy)
        {
            _price = price;
            _buy = onBuy;
            
            Bank = Game.Instance.Bank;

            _currentPrice = _price;
            _payment = _price / 10;

            if (_payment < 1)
                _payment = 1;

            _waitForPay = _buyTime / 10f;
            
            _priceTxt.text = _currentPrice.ToString();
            
            _nextPayTime = 0f;
            _particleTime = 0f;
        }

        private void PlayMoneyParticles(Transform to)
        {
            if (Time.time < _particleTime)
                return;
            
            Vector3 particlePos = to.position;
            particlePos.y = 1f;

            _particle.transform.position = particlePos;
            _particle.transform.LookAt(_lookParticle);
            _particle.Play();
                
            _particleTime = Time.time + _particleCooldown;
        }

        private void OnTriggerStay(Collider other)
        {
            if (Time.time < _nextPayTime) 
                return;
            
            if (other.TryGetComponent(out Player player))
            {
                if (Bank.TryGetLevelCoins(_payment) == false)
                    return;
                
                _currentPrice -= _payment;
                
                if (_currentPrice < 1)
                {
                    _buy?.Invoke();
                    _currentPrice += _price;
                }
                
                _priceTxt.text = _currentPrice.ToString();
                _nextPayTime = Time.time + _waitForPay;
                PlayMoneyParticles(player.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player))
                _particle.Stop();
        }
    }
}