using UnityEngine;

namespace _Game._Scripts
{
    public class EnemyContainerController : MonoBehaviour
    {
        public int GetEnemiesCount()
        {
            return transform.childCount;
        }

        public void DestroyAllEnemies()
        {
            foreach (Transform enemy in transform)
            {
                Destroy(enemy.gameObject);
            }
        }
    }
}