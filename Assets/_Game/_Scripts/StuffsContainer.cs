using UnityEngine;

namespace _Game._Scripts
{
    public class StuffsContainer : MonoBehaviour
    {
        public void DestroyAllStuffs()
        {
            foreach (Transform stuff in transform)
            {
                Destroy(stuff.gameObject);
            }
        }
    }
}