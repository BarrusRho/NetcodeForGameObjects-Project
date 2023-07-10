using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetcodeForGameObjects.Utility
{
    public class DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] private float _timeToDestroy = 1f;

        private void Start()
        {
            Destroy(this.gameObject, _timeToDestroy);
        }
    }
}
