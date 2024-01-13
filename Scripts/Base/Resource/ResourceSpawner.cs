using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BaseProtection
{
    [RequireComponent(typeof(SphereCollider))]
    public class ResourceSpawner : MonoBehaviour
    {
        [SerializeField] private ResourceOre _ore;
        [SerializeField] private float _spawnRadius;
        [SerializeField] private float _spawnInterval;
        [SerializeField] private int _maxSpawnPositions;

        private List<Vector3> _spawnPositions = new List<Vector3>();

        private SphereCollider _collider;

        private void Start()
        {
            _collider = GetComponent<SphereCollider>();
            //_collider.radius = _spawnRadius;
            _collider.isTrigger = true;
        }

        public void Init()
        {
            StartCoroutine(SpawnObjectCoroutine());

            for (int i = 0; i < 3; i++)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();

                if (spawnPosition != Vector3.zero)
                {
                    ResourceOre instance = Instantiate(_ore, spawnPosition, Quaternion.identity, transform);
                    instance.Init();

                    _spawnPositions.Add(spawnPosition);
                }
            }
        }

        public void Stop()
        {
            StopAllCoroutines();
        }

        private IEnumerator SpawnObjectCoroutine()
        {
            while (true)
            {
                if (_spawnPositions.Count >= _maxSpawnPositions)
                {
                    yield return new WaitForSeconds(_spawnInterval);
                    continue;
                }

                Vector3 spawnPosition = GetRandomSpawnPosition();

                if (spawnPosition != Vector3.zero)
                {
                    ResourceOre instance = Instantiate(_ore, spawnPosition, Quaternion.identity, transform);
                    instance.Init();

                    _spawnPositions.Add(spawnPosition);
                }

                yield return new WaitForSeconds(_spawnInterval);
            }
        }

        private Vector3 GetRandomSpawnPosition()
        {
            Vector3 randomPosition = Vector3.zero;

            for (int i = 0; i < 10; i++)
            {
                randomPosition = transform.position + Random.insideUnitSphere * _spawnRadius;
                randomPosition.y = transform.position.y;

                if (IsFreePlace(randomPosition) == true)
                    return randomPosition;
            }

            return Vector3.zero;
        }

        private bool IsFreePlace(Vector3 position)
        {
            foreach (Vector3 spawnPosition in _spawnPositions)
            {
                if (Vector3.Distance(spawnPosition, position) < 1.0f)
                    return false;
            }

            return true;
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out Player player))
                player.Attacker.InResourcesZone(true);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player))
                player.Attacker.InResourcesZone(false);
        }
    }
}