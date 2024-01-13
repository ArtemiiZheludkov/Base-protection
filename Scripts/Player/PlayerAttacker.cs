using BaseProtection.Weapons;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using DG.Tweening;

namespace BaseProtection
{
    public class PlayerAttacker : MonoBehaviour
    {
        [SerializeField] private float _fieldOfViewAngle = 90f;
        [SerializeField] private float _viewDistance = 10f;

        [Header("IK")]
        [SerializeField] private Transform _hand;
        [SerializeField] private MultiAimConstraint _IKController;
        [SerializeField] private Transform _IKTarget;

        [Header("WEAPONS")] 
        [SerializeField] private TargetOutline _targetOutline;
        [SerializeField] private Weapon _pickAxePrefab;
        [SerializeField] private Weapon _bowPrefab;
        
        private int _damage;
        private float _speed;
        private Animator _animator;
        private HashAnimation _animations;

        private Weapon _pickAxe;
        private Weapon _bow;
        private Weapon _currentWeapon;
        
        private float _animationSpeed;
        private float _attackWait;
        private float _nextAttackTime;
        private bool _hasTarget;

        private Tween _ikTween;

        public void Init(int damage, float speed, Animator animator, HashAnimation animations)
        {
            enabled = true;

            _damage = damage;
            _speed = speed;
            _animator = animator;
            _animations = animations;
            
            _attackWait = (1f / _speed) / 2f;
            _nextAttackTime = 0f;
            _hasTarget = false;

            _pickAxe = Instantiate(_pickAxePrefab, parent: _hand);
            _bow = Instantiate(_bowPrefab, parent: _hand);
            
            _pickAxe.Disable();
            _bow.Disable();

            _IKController.weight = 0f;
            InResourcesZone(false);
            
            _targetOutline.Deactivate();
        }

        public void Stop()
        {
            StopAllCoroutines();
            
            _animator.CrossFade(_animations.HandFree, 0.25f, 1);
            _ikTween = DOTween.To(() => _IKController.weight, x => _IKController.weight = x, 0f, 0.5f)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
            
            enabled = false;
        }

        public void InResourcesZone(bool inZone)
        {
            if (inZone == true)
                SetWeapon(_pickAxe);
            else
                SetWeapon(_bow);
        }
        
        private void FixedUpdate()
        {
            Attacking();
        }
        
        private void Attacking()
        {
            if (Time.time < _nextAttackTime)
                return;

            if (_hasTarget == false)
            {
                DetectTarget();
                IKRotate();
                SetAttackAnimation();
                
                if (_hasTarget == false)
                    _targetOutline.Deactivate();
            }
            else
            {
                DetectTarget();
                _hasTarget = false;
            }
            
            _nextAttackTime = Time.time + _attackWait;
        }

        private void DetectTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _viewDistance, _currentWeapon.AttackLayers);

            foreach (Collider collider in colliders)
            {
                Vector3 direction = collider.transform.position - transform.position;

                if (Vector3.Angle(direction, transform.forward) <= _fieldOfViewAngle * 0.5f)
                {
                    if (collider.TryGetComponent(out IDamageable target))
                    {
                        if (_hasTarget == true)
                            _currentWeapon.Attack(target);
                        else
                            _hasTarget = true;
                        
                        SetIKTarget(collider);
                        break;
                    }
                }
            }
        }

        private void SetIKTarget(Collider collider)
        {
            _IKTarget.localPosition = Vector3.zero;
            Vector3 localPosition = _IKTarget.InverseTransformPoint(collider.transform.position);
            localPosition.y = 1;
            _IKTarget.localPosition = localPosition;

            _targetOutline.Follow(collider.transform);
        }

        private void IKRotate()
        {
            if (_ikTween != null)
                _ikTween.Kill();
                
            if (_hasTarget == false)
                _ikTween = DOTween.To(()=> _IKController.weight, x=> _IKController.weight = x, 0f, 0.5f)
                    .SetEase(Ease.Linear)
                    .SetLink(gameObject);
            else
                _ikTween = DOTween.To(()=> _IKController.weight, x=> _IKController.weight = x, 1f, 0.5f)
                    .SetEase(Ease.Linear)
                    .SetLink(gameObject);
        }

        private void SetAttackAnimation()
        {
            if (_hasTarget == true)
                _animator.CrossFade(_currentWeapon.AnimationClip, 0.25f, 1);
            else
                _animator.CrossFade(_animations.HandFree, 0.25f, 1);
        }

        private void SetWeapon(Weapon weapon)
        {
            if (_currentWeapon == weapon)
                return;
            
            if (_currentWeapon != null)
                _currentWeapon.Disable();
            
            _currentWeapon = weapon;
            _currentWeapon.Init(_damage);
            _viewDistance = _currentWeapon.AttackDistance;

            InitAttackAnimation();
        }

        private void InitAttackAnimation()
        {
            _animationSpeed = 0f;
            AnimationClip[] animationClips = _animator.runtimeAnimatorController.animationClips;

            foreach (AnimationClip clip in animationClips)
            {
                if (clip.name == _currentWeapon.AnimationClip)
                {
                    _animationSpeed = _speed * clip.length;
                    break;
                }
            }

            _animator.SetFloat("Speed", _animationSpeed);
        }
    }
}