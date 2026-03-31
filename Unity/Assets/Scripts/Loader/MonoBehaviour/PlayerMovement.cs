using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody2D rb;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            Log.Info("Rigidbody2D");
        }

        void OnTriggerEnter(Collider other)
        {
            Log.Info("OnTriggerEnter");
        }

        void OnTriggerExit(Collider other)
        {
            Log.Info("OnTriggerExit");
        }
    }
}