using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class TriggerComponent : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log("撞上了: OnTriggerEnter2D");
        }
        
        void OnTriggerStay2D(Collider2D col)
        {
            Debug.Log("撞上了: OnTriggerStay2D");
        }
        
        void OnTriggerExit2D(Collider2D col)
        {
            Debug.Log("撞上了: OnTriggerExit2D");
        }
    }
}