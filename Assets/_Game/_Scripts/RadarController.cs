using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game._Scripts
{
    public class RadarController : MonoBehaviour
    {
        [SerializeField] private float detectionRadius = 5f;
        private readonly List<GameObject> _targets = new();

        private void Update()
        {
            DetectTargets();
        }

        private void DetectTargets()
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
            _targets.Clear();

            foreach (var otherCollider in colliders)
            {
                if (otherCollider.CompareTag("Enemy"))
                {
                    _targets.Add(otherCollider.gameObject);
                }
            }
        }

        public GameObject GetTheClosestTarget()
        {
            if (_targets.Count == 0)
            {
                return null;
            }

            GameObject closestTarget = null;
            var closestDistance = float.MaxValue;
            foreach (var target in _targets.ToList())
            {
                if (target == null)
                {
                    _targets.Remove(target);
                    continue;
                }

                var distance = Vector2.Distance(transform.position, target.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }

            return closestTarget;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}