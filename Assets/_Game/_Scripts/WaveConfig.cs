using System;

namespace _Game._Scripts
{
    [Serializable]
    public struct WaveConfig
    {
        public float duration;
        public int maxEnemiesInScene;
        public float cooldownToSpawn;
    }
}