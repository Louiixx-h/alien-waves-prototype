using _Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game._Scripts
{
    [RequireComponent(typeof(CounterController))]
    public class LevelController : MonoBehaviour
    {
        public EnemyController enemyPrefab;
        public SpawnPreviewController spawnPreviewPrefab;
        public WaveConfig[] waves;
        public Transform maxY;
        public Transform minY;
        public Transform maxX;
        public Transform minX;

        private int _waveIndex;
        private WaveConfig _currentWave;
        private CounterController _counter;
        private CoinsController _coinsController;
        private EnemyContainerController _enemyContainer;
        private StuffsContainer _stuffsContainer;
        private BuildScreen _buildScreen;
        private WinScreen _winScreen;
        private WaveCounterController _waveCounterController;
        private PlayerController _player;
        private bool _canSpawn;
        private bool _isWaveEnd;
        private float _timeToSpawn;
        
        private void Awake()
        {
            _counter = GetComponent<CounterController>();
            _enemyContainer = FindFirstObjectByType<EnemyContainerController>();
            _buildScreen = FindFirstObjectByType<BuildScreen>();
            _winScreen = FindFirstObjectByType<WinScreen>();
            _coinsController = FindFirstObjectByType<CoinsController>();
            _waveCounterController = FindFirstObjectByType<WaveCounterController>();
            _stuffsContainer = FindFirstObjectByType<StuffsContainer>();
            _player = FindFirstObjectByType<PlayerController>();
        }

        private void Start()
        {
            _currentWave = waves[_waveIndex];
            _timeToSpawn = _currentWave.cooldownToSpawn;
            _waveCounterController.SetWave(_waveIndex+1, waves.Length);
            _counter.SetTime(_currentWave.duration);
            _counter.StartCounter();
            _counter.OnTimeEnd += OnWaveEnd;
        }

        private void OnDestroy()
        {
            _counter.OnTimeEnd -= OnWaveEnd;
        }

        private void Update()
        {
            if (_isWaveEnd) return;
            if (!_canSpawn)
            {
                _timeToSpawn -= Time.deltaTime;
                if (_timeToSpawn <= 0)
                {
                    _canSpawn = true;
                    _timeToSpawn = _currentWave.cooldownToSpawn;
                }
            }
            
            VerifyShouldSpawn();
        }

        private void VerifyShouldSpawn()
        {
            if (_canSpawn && _enemyContainer.GetEnemiesCount() < _currentWave.maxEnemiesInScene)
            {
                var position = new Vector2(Random.Range(minX.position.x, maxX.position.x), Random.Range(minY.position.y, maxY.position.y));
                var enemyController = Instantiate(spawnPreviewPrefab, position, Quaternion.identity);
                enemyController.SetEnemyPrefab(enemyPrefab.gameObject)
                    .SetParent(_enemyContainer.transform)
                    .SetStuffContainer(_stuffsContainer)
                    .SetParent(_enemyContainer.transform);
                _canSpawn = false;
            }
        }
        
        private void OnWaveEnd()
        {
            _player.Stop();
            _isWaveEnd = true;
            if (_waveIndex >= waves.Length-1)
            {
                _winScreen.ShowWinScreen();
                return;
            }
            
            _waveIndex++;
            _currentWave = waves[_waveIndex];
            _counter.SetTime(_currentWave.duration);
            _buildScreen.ShowBuildScreen();
            _enemyContainer.DestroyAllEnemies();
            _stuffsContainer.DestroyAllStuffs();
            Time.timeScale = 0;
        }
        
        public void StartWave()
        {
            Time.timeScale = 1;
            _waveCounterController.SetWave(_waveIndex+1, waves.Length);
            _isWaveEnd = false;
            _counter.StartCounter();
            _buildScreen.HideBuildScreen();
            _player.ResetHealth();
            _player.Resume();
        }

        public void AddSelectedCard(CardModelSo card)
        {
            _player.AddCard(card);
        }

        public void GameOver()
        {
            Time.timeScale = 0;
            _counter.StopCounter();
            _winScreen.ShowWinScreen();
        }
    }
}