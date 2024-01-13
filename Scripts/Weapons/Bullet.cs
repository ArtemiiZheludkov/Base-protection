using UnityEngine;

namespace BaseProtection.Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        private int _damage;
        
        public void Init(in Vector3 target, in float speed, in int damage)
        {
            _damage = damage;
            
            Vector3 direction = target - transform.position;
            direction.y = 0;
            _rigidbody.velocity = direction * speed;
            transform.LookAt(target);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable enemy))
            {
                enemy.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}