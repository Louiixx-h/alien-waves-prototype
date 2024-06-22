using _Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace _Game._Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour, IDamageable
    {
        public BulletController bulletPrefab;
        public int maxHealth = 20;
        public float moveSpeed = 5f;
        public float dashMultiplier = 5f;
        public float dashDuration = 0.2f;
        public float cooldownToShot = 1f;
        public float rangeDamage = 5f;

        private int _currentHealth;
        private int _level = 1;
        private int _exp;
        private bool _isDashState;
        private bool _canShot;
        private bool _isStopped;
        private float _timeDash;
        private float _timeToShot;
        private float _cooldownToShot;
        private Rigidbody2D _myRigidBody;
        private RadarController _radar;
        private Vector2 _dashDirection;
        private FloatingJoystick _joystick;
        private DashButton _dashButton;
        private LevelController _levelController;
        private PlayerHealthUI _playerHealthUI;
        private MoneyModel _moneyModel;

        private void Awake()
        {
            _levelController = FindFirstObjectByType<LevelController>();
            _joystick = FindFirstObjectByType<FloatingJoystick>();
            _dashButton = FindFirstObjectByType<DashButton>();
            _playerHealthUI = FindFirstObjectByType<PlayerHealthUI>();
            _radar = GetComponentInChildren<RadarController>();
            _myRigidBody = GetComponent<Rigidbody2D>();
            _moneyModel = MoneyModel.Instance;
        }

        private void Start()
        {
            _currentHealth = maxHealth;
            _cooldownToShot = cooldownToShot;
        }

        private void Update()
        {
            if (_isStopped) return;
            if (!_canShot)
            {
                _timeToShot -= Time.deltaTime;
                if (_timeToShot <= 0)
                {
                    _canShot = true;
                    _timeToShot = _cooldownToShot;
                }
            }

            if (_isDashState)
            {
                _timeDash += Time.deltaTime;
                if (_timeDash >= dashDuration)
                {
                    _isDashState = false;
                    _timeDash = 0;
                    _myRigidBody.velocity = Vector2.zero;
                }
            }

            if ((Input.GetKeyDown(KeyCode.Space) || _dashButton.IsPressed) && !_isDashState)
            {
                float x;
                float y;

                if (_joystick != null && _joystick.Horizontal != 0 && _joystick.Vertical != 0)
                {
                    x = _joystick.Horizontal;
                    y = _joystick.Vertical;
                }
                else
                {
                    x = Input.GetAxis("Horizontal");
                    y = Input.GetAxis("Vertical");
                }
                
                _dashDirection = new Vector2(x, y);
                _isDashState = true;
            }

            VerifyShouldShot();
        }

        private void FixedUpdate()
        {
            if (_isStopped)
            {
                _myRigidBody.velocity = Vector2.zero;
                return;
            }
            if (_isDashState)
            {
                Dash();
            }
            else
            {
                Move();
            }
        }

        private void Dash()
        {
            _myRigidBody.velocity = _dashDirection * (moveSpeed * dashMultiplier);
        }

        private void Move()
        {
            float x;
            float y;
            
            if (_joystick != null && _joystick.Horizontal != 0 && _joystick.Vertical != 0)
            {
                x = _joystick.Horizontal;
                y = _joystick.Vertical;
            }
            else
            {
                x = Input.GetAxis("Horizontal");
                y = Input.GetAxis("Vertical");
            }
            _myRigidBody.velocity = new Vector2(x, y) * moveSpeed;
        }

        private void VerifyShouldShot()
        {
            if (_canShot && _radar != null)
            {
                var closestTarget = _radar.GetTheClosestTarget();
                if (closestTarget != null)
                {
                    InstantiateBullet(closestTarget.transform.position);
                    _canShot = false;
                }
            }
        }

        private void InstantiateBullet(Vector3 targetPosition)
        {
            var bulletInstantiated = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            if (bulletInstantiated != null)
            {
                bulletInstantiated
                    .SetDirection(TargetDirection(targetPosition))
                    .SetDamage(rangeDamage);
            }
        }

        private Vector2 TargetDirection(Vector3 targetPosition)
        {
            return (targetPosition - transform.position).normalized;
        }

        public void ApplyDamage(float damage)
        {
            if (_isDashState) return;
            _currentHealth -= (int) damage;
            _playerHealthUI.SetHealth(_currentHealth, maxHealth);
            if (_currentHealth <= 0)
            {
                _levelController.GameOver();
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Exp") && other.TryGetComponent(out ExpController expController))
            {
                _moneyModel.CreditOperation(expController.expValue);
                _exp += expController.expValue;
                Destroy(other.gameObject);
                CalculateNextLevelWithLogarithm();
            }
        }

        private void CalculateNextLevelWithLogarithm()
        {
            var expToNextLevel = 7 * Mathf.Pow(2, _level);
            if (_exp >= expToNextLevel)
            {
                print($"Level up! Current level: {_level}");
                _level++;
                _exp = 0;
            }
        }
        
        public void AddCard(CardModelSo card)
        {
            var logColor = new Color(220, 50, 150);
            
            foreach (var buff in card.buffs)
            {
                switch (buff.type)
                {
                    case BuffType.MeleeDamage:
                        print($"<h2>Melee damage buff: {buff.value}</h2>");
                        print($"<color=#{logColor.ToHexString()}>Current melee damage = {rangeDamage}</color>");
                        break;
                    case BuffType.RangeDamage:
                        rangeDamage += buff.value;
                        print($"<h2>Range damage buff: {buff.value}</h2>");
                        print($"<color=#{logColor.ToHexString()}>Current range damage = {rangeDamage}</color>");
                        break;
                    case BuffType.AttackSpeed:
                        _cooldownToShot += _cooldownToShot * buff.value;
                        print($"<h2>Attack speed buff: {buff.value}</h2>");
                        print($"<color=#{logColor.ToHexString()}>Current attack speed = {_cooldownToShot}</color>");
                        break;
                }
            }
        }

        public void Stop()
        {
            _isStopped = true;
        }
        
        public void Resume()
        {
            _isStopped = false;
        }

        public void ResetHealth()
        {
            _currentHealth = maxHealth;
            _playerHealthUI.SetHealth(_currentHealth, maxHealth);
        }
    }
}