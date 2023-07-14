using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetcodeForGameObjects.Utility
{
    public class SpawnOnDestroy : MonoBehaviour
    {
        [SerializeField] private GameObject _prefabToSpawn;

        private void OnDestroy()
        {
            Instantiate(_prefabToSpawn, transform.position, Quaternion.identity);
        }
    }
}
