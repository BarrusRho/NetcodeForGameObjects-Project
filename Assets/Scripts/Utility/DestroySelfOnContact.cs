using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetcodeForGameObjects.Utility
{
    public class DestroySelfOnContact : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Destroy(this.gameObject);
        }
    }
}
