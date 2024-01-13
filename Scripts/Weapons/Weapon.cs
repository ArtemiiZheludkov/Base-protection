using BaseProtection.Weapons;
using UnityEngine;

namespace BaseProtection
{
    public abstract class Weapon : MonoBehaviour
    {
        public int AttackDistance;
        public string AnimationClip;
        public LayerMask AttackLayers;

        public abstract WeaponType Type();
        
        private protected int _damage;
        
        public virtual void Init(int damage)
        {
            _damage = damage;
            gameObject.SetActive(true);
        }
        
        public virtual void Disable()
        {
            gameObject.SetActive(false);
        }

        public abstract void Attack(IDamageable target);
    }
}