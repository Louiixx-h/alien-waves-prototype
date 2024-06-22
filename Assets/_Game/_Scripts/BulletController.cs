using UnityEngine;

namespace _Game._Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BulletController : MonoBehaviour
    {
        public float velocity = 10f;
        
        private float _damage;
        private Rigidbody2D _myRigidBody;
        private Vector2 _direction;

        private void Start()
        {
            _myRigidBody = GetComponent<Rigidbody2D>();
            Destroy(gameObject, 5f);
        }

        private void FixedUpdate()
        {
            _myRigidBody.velocity = _direction * velocity;
        }

        public BulletController SetDirection(Vector2 direction)
        {
            _direction = direction;
            return this;
        }
        
        public BulletController SetDamage(float damage)
        {
            _damage = damage;
            return this;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") && other.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}