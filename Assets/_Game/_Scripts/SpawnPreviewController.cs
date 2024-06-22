using System;
using _Game._Scripts;
using UnityEngine;

namespace _Scripts
{
    public class SpawnPreviewController : MonoBehaviour
    {
        public float previewCooldown;
        
        private GameObject _prefabToSpawn;
        private Transform _parent;
        private StuffsContainer _stuffContainer;
        private float _timeToSpawn;

        private void Start()
        {
            _timeToSpawn = previewCooldown;
        }

        private void Update()
        {
            if (_prefabToSpawn == null) return;
            _timeToSpawn -= Time.deltaTime;
            if (_timeToSpawn < 0)
            {
                var instantiated = Instantiate(_prefabToSpawn, transform.position, Quaternion.identity);
                if (instantiated.TryGetComponent(out EnemyController enemyController))
                {
                    enemyController.SetStuffContainer(_stuffContainer)
                        .SetParent(_parent);
                }
                Destroy(gameObject);
            }
        }

        public SpawnPreviewController SetEnemyPrefab(GameObject prefab)
        {
            _prefabToSpawn = prefab;
            return this;
        }
        
        public SpawnPreviewController SetParent(Transform parent)
        {
            _parent = parent;
            return this;
        }
        
        public SpawnPreviewController SetStuffContainer(StuffsContainer stuffsContainer)
        {
            _stuffContainer = stuffsContainer;
            return this;
        }
        
        public SpawnPreviewController SetParent(GameObject parent)
        {
            transform.parent = parent.transform;
            return this;
        }
    }
}