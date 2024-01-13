using UnityEngine;

namespace BaseProtection
{
    [RequireComponent(typeof(PlayerMover))]
    [RequireComponent(typeof(PlayerAttacker))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerConfig _config;
        [SerializeField] private Animator _animator;

        public PlayerMover Mover { get; private set; }
        public PlayerAttacker Attacker { get; private set; }
        
        private HashAnimation _animations;
        private BankService _bank;

        public void Init()
        {
            _animations = new HashAnimation();
            Mover = GetComponent<PlayerMover>();
            Mover.Init(_config.MoveSpeed.Value, _animator, _animations);
            Attacker = GetComponent<PlayerAttacker>();
            Attacker.Init((int)_config.AttackDamage.Value, _config.AttackSpeed.Value, _animator, _animations);

            _bank = Game.Instance.Bank;
        }
        
        public void TakeResource(int coins)
        {
            _bank.AddLevelCoins(coins);
        }

        public void LevelEnd(LevelEndType end)
        {
            Mover.Stop();
            Attacker.Stop();

            if (end == LevelEndType.Win)
                _animator.CrossFade(_animations.Win, 0.1f);
            else
                _animator.CrossFade(_animations.Lose, 0.1f);
        }
    }
}
