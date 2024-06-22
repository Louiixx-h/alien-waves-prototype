using UnityEngine;

namespace _Game._Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyController : MonoBehaviour, IDamageable
    {
        public int hurtBoxDamage = 2;
        public GameObject expPrefab;
        public float distanceToStop = 1f;
        public float moveSpeed = 3f;
        public int health = 5;

        private float _timeToHurt;
        private bool _canHurt = true;
        private BoxCollider2D _myCollider;
        private PlayerController _player;
        private Rigidbody2D _myRigidBody;
        private StuffsContainer _stuffContainer;
        
        private const float CooldownToHurt = 1f;

        private void Start()
        {
            _timeToHurt = CooldownToHurt;
            _myCollider = GetComponent<BoxCollider2D>();
            _myRigidBody = GetComponent<Rigidbody2D>();
            _player = FindObjectOfType<PlayerController>();
        }

        private void Update()
        {
            if (!_canHurt)
            {
                _timeToHurt -= Time.deltaTime;
                if (_timeToHurt <= 0)
                {
                    _canHurt = true;
                    _timeToHurt = CooldownToHurt;
                }
            }
            
            VerifyShouldHurt();
        }

        private void FixedUpdate()
        {
            MoveToPlayer();
        }

        private void MoveToPlayer()
        {
            if (_player == null || Vector2.Distance(transform.position, _player.transform.position) <= distanceToStop)
            {
                _myRigidBody.velocity = Vector2.zero;
                return;
            }
            var direction = (_player.transform.position - transform.position).normalized;
            _myRigidBody.velocity = direction * moveSpeed;
        }
        
        public void ApplyDamage(float damage)
        {
            health -= (int) damage;
            if (health <= 0)
            {
                var instantiatedCoin = Instantiate(expPrefab, transform.position, Quaternion.identity);
                instantiatedCoin.transform.SetParent(_stuffContainer.transform);
                Destroy(gameObject);
            }
        }
        
        private void VerifyShouldHurt()
        {
            if (_player == null) return;
            if (!_canHurt) return;
            
            HurtBox();
        }
        
        private void HurtBox()
        {
            var bounds = _myCollider.bounds;
            var hitColliders = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player") && _player != null)
                {
                    _player.ApplyDamage(hurtBoxDamage);
                    _canHurt = false;
                }
            }
        }

        public EnemyController SetStuffContainer(StuffsContainer stuffContainer)
        {
            _stuffContainer = stuffContainer;
            return this;
        }
        
        public EnemyController SetParent(Transform parent)
        {
            transform.parent = parent;
            return this;
        }
    }
}