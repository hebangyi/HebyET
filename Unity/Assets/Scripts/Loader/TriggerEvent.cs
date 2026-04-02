using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class TrigetEvent : MonoBehaviour
    {
        public Action<Collider2D> OnTriggerEnter2DAction { get; set; }
        public Action<Collider2D> OnTriggerStay2DAction { get; set; }
        public Action<Collider2D> OnTriggerExitAction { get; set; }
        
        
        // Start is called before the first frame update
        void Start()
        {
            Log.Info("abc");
        }

        // Update is called once per frame
        void Update()
        {
        }
        
        void OnTriggerEnter2D(Collider2D col)
        {
            OnTriggerEnter2DAction?.Invoke(col);
        }

        void OnTriggerStay2D(Collider2D col)
        {
            OnTriggerStay2DAction?.Invoke(col);
        }

        void OnTriggerExit2D(Collider2D col)
        {
            OnTriggerExitAction?.Invoke(col);
        }
    }
}
