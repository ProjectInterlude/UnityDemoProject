using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interlude
{
    public class Key : MonoBehaviour
    {
        public int id;
        public string secret;

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player" || other.tag == "LocalPlayer")
            {
                FindObjectOfType<ScavengerHuntExample>().PauseGame();
                ScavengerHunt.KeyFound(id, secret);
            }
        }

    }
}

